using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SnowmanDecoration : MonoBehaviour
{
    public static SnowmanDecoration Instance;
    private DecorationType currentDecoType;

    private SnowmanData currentSnowmanData;

    [Header("Snow Man")]
    [SerializeField, Range(0, 1f)] private float snowmanMergeRatio;
    [SerializeField] private float snowmanRotationSpeed = 50f;
    [SerializeField] private Transform snowmanRoot;
    [SerializeField] private GameObject snowmanObject;
    [SerializeField] private Transform snowmanUpPart;
    [SerializeField] private Transform snowmanDownPart;

    [Header("Decoration Button UI")]
    [SerializeField] private GameObject uiButtonPrefab;
    [SerializeField] private Transform uiButtonParent;

    [Header("Decoration Filter Button UI")]
    [SerializeField] private GameObject filterButtonPrefab;
    [SerializeField] private Transform filterButtonParent;

    [Header("Snowman Decoration Objects")]
    [SerializeField] private GameObject[] decoMufflers;
    [SerializeField] private GameObject[] decoArms;
    [SerializeField] private GameObject[] decoHats;
    [SerializeField] private GameObject[] decoButtons;
    [SerializeField] private GameObject[] decoEyes;
    [SerializeField] private GameObject[] decoMouths;
    [SerializeField] private GameObject[] decoNoses;

    // =========================
    // Free Placement (NEW)
    // =========================
    [Header("Free Placement (Eye/Nose/Mouth/Arm etc)")]
    [SerializeField] private LayerMask snowmanSurfaceMask;    // 스노우맨 표면 레이어
    [SerializeField] private Transform placeablesRoot;        // 생성될 배치형 데코 부모(비워도 됨)
    [SerializeField] private float maxRayDistance = 200f;
    [SerializeField] private float defaultSurfaceOffset = 0.02f;

    private DecorationItem pendingPlaceItem;
    private GameObject previewInstance;

    // 싱글톤/단일 설치 타입(코/입 같은거 “1개만 유지” 하고 싶으면 여기에 등록)
    // 필요없으면 비워둬도 됨.
    private readonly HashSet<DecorationType> singletonPlaceTypes = new()
    {
        DecorationType.Nose,
        DecorationType.Mouth
        // DecorationType.Eye, // 눈도 1개만 하려면 추가
        // DecorationType.Arm  // 팔도 1개만 하려면 추가
    };

    public void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 기본 선택 타입(원하면 여기서 원하는 초기값 지정)
        // currentDecoType = DecorationType.Muffler;

        InitDecoUI();
        InitSnowmanData(35, 35);
    }

    private void Update()
    {
        HandlePlacementUpdate();
        RotationSnowman();
    }

    public void RotationSnowman()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        snowmanRoot.rotation *= Quaternion.Euler(new Vector3(v, -h, 0) * snowmanRotationSpeed * Time.deltaTime);
    }

    public void InitSnowmanData(float upSize, float downSize)
    {
        currentSnowmanData = new SnowmanData(upSize, downSize, new List<DecorationItem>());
        snowmanUpPart.localScale = new Vector3(upSize, upSize, upSize);
        snowmanDownPart.localScale = new Vector3(downSize, downSize, downSize);

        //중앙 위치 맞춰서 스노우맨의 파츠가 정렬됨
        snowmanUpPart.localPosition = new Vector3(0, (downSize / 2) - ((downSize) * snowmanMergeRatio), 0);
        snowmanDownPart.localPosition = new Vector3(0, -downSize / 2, 0);

        //Camera.main.fieldOfView = 60 + (upSize + downSize) * 2;
        Camera.main.transform.localPosition = new Vector3(0, 0, -10 - (upSize + downSize) * 1.2f);
    }

    private void InitDecoUI()
    {
        // currentDecoType 기준으로 필터된 아이템
        List<ItemBase> items = Inventory.Instance.GetAllItems(currentDecoType);

        // 기존 버튼 삭제
        for (int i = filterButtonParent.childCount - 1; i >= 0; i--)
            Destroy(filterButtonParent.GetChild(i).gameObject);

        for (int i = uiButtonParent.childCount - 1; i >= 0; i--)
            Destroy(uiButtonParent.GetChild(i).gameObject);

        // 필터 버튼 생성
        foreach (DecorationType type in System.Enum.GetValues(typeof(DecorationType)))
        {
            DecorationType localType = type; // 캡쳐 안전
            GameObject filterButtonObj = Instantiate(filterButtonPrefab, filterButtonParent);

            // 텍스트
            var txt = filterButtonObj.GetComponentInChildren<TMP_Text>();
            if (txt != null) txt.text = localType.ToString();

            // 클릭
            var btn = filterButtonObj.GetComponent<UnityEngine.UI.Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    currentDecoType = localType;
                    InitDecoUI();
                });
            }
        }

        // 아이템 버튼 생성
        foreach (var item in items)
        {
            if (item is not DecorationItem) continue;

            GameObject itemButtonObj = Instantiate(uiButtonPrefab, uiButtonParent);

            // 기존 방식 유지: 버튼 스크립트가 item을 Init하고, 그 스크립트가 SnowmanDecoration.Instance.DecoSnowman(...) 호출한다고 가정
            itemButtonObj.GetComponent<DecorationButtonInitiator>().Init(item as DecorationItem);
        }
    }

    public void DecoSnowman(DecorationItem item)
    {
        // === 1) 자유 배치 타입이면 배치 모드로 전환 ===
        if (IsFreePlaceType(item.decorationType))
        {
            BeginPlacement(item);
            return;
        }

        // === 2) 기존 토글 방식(목도리/모자/단추 등)은 그대로 유지 ===
        switch (item.decorationType)
        {
            case DecorationType.Muffler:
                foreach (GameObject muffler in decoMufflers) muffler.SetActive(false);
                decoMufflers[item.decorationObjectIndex].SetActive(!decoMufflers[item.decorationObjectIndex].activeSelf);
                break;

            case DecorationType.Arm:
                foreach (GameObject arm in decoArms) arm.SetActive(false);
                decoArms[item.decorationObjectIndex].SetActive(!decoArms[item.decorationObjectIndex].activeSelf);
                break;

            case DecorationType.Hat:
                foreach (GameObject hat in decoHats) hat.SetActive(false);
                decoHats[item.decorationObjectIndex].SetActive(!decoHats[item.decorationObjectIndex].activeSelf);
                break;

            case DecorationType.Button:
                foreach (GameObject button in decoButtons) button.SetActive(false);
                decoButtons[item.decorationObjectIndex].SetActive(!decoButtons[item.decorationObjectIndex].activeSelf);
                break;

            case DecorationType.Eye:
                foreach (GameObject eye in decoEyes) eye.SetActive(false);
                decoEyes[item.decorationObjectIndex].SetActive(!decoEyes[item.decorationObjectIndex].activeSelf);
                break;

            case DecorationType.Mouth:
                foreach (GameObject mouth in decoMouths) mouth.SetActive(false);
                decoMouths[item.decorationObjectIndex].SetActive(!decoMouths[item.decorationObjectIndex].activeSelf);
                break;

            case DecorationType.Nose:
                foreach (GameObject nose in decoNoses) nose.SetActive(false);
                decoNoses[item.decorationObjectIndex].SetActive(!decoNoses[item.decorationObjectIndex].activeSelf);
                break;
        }

        currentSnowmanData.AddDecorationItem(item);
        Debug.Log("데코레이션 적용 완료: " + item.itemName);
    }

    /// <summary>
    /// 자유 배치 타입인지 여부 확인
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private bool IsFreePlaceType(DecorationType type)
    {
        // "눈코입이나 팔"을 자유 배치로 하려면 여기 true
        return type == DecorationType.Eye
            || type == DecorationType.Nose
            || type == DecorationType.Mouth
            || type == DecorationType.Arm;
    }

/// <summary>
/// 자유 배치 모드 시작
/// </summary>
/// <param name="item"></param>
    private void BeginPlacement(DecorationItem item)
    {
        pendingPlaceItem = item;

        if (previewInstance != null)
        {
            Destroy(previewInstance);
            previewInstance = null;
        }

        Debug.Log($"배치 모드 시작: {item.itemName} (좌클릭=설치, 우클릭=취소)");
    }

    /// <summary>
    /// 자유 배치 모드 업데이트 처리
    /// </summary>
    private void HandlePlacementUpdate()
    {
        if (pendingPlaceItem == null) return;

        // UI 위 클릭은 무시
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        // 표면 히트면 프리뷰 이동
        if (TryGetSnowmanSurfaceHit(out RaycastHit hit))
        {
            if (previewInstance == null)
            {
                // NOTE: DecorationItem에 프리팹 필드가 없으면 아래 방식(기존 배열)로는 “마음대로 배치”가 불가능함.
                //       그래서 자유 배치 타입은 반드시 item에 프리팹이 있어야 함(DecorationItem에 placeablePrefab 같은 필드 추가 필요).
                if (!TryGetPlacePrefab(pendingPlaceItem, out GameObject prefab))
                {
                    Debug.LogError($"배치 프리팹이 없습니다: {pendingPlaceItem.itemName} (DecorationItem에 placeablePrefab 필드 필요)");
                    pendingPlaceItem = null;
                    return;
                }

                previewInstance = Instantiate(prefab, placeablesRoot ? placeablesRoot : transform);
                SetPreviewVisual(previewInstance, true);
            }

            float offset = GetSurfaceOffset(pendingPlaceItem);
            Vector3 pos = hit.point + hit.normal * offset;

            // 표면에 붙는 회전(원하면 up축 처리 더 정교하게 가능)
            Quaternion rot = Quaternion.LookRotation(-hit.normal, Vector3.up);

            previewInstance.transform.SetPositionAndRotation(pos, rot);
        }

        // 좌클릭: 설치 확정
        if (Input.GetMouseButtonDown(0) && previewInstance != null)
        {
            // 싱글톤 타입이면 이전 설치 삭제
            if (singletonPlaceTypes.Contains(pendingPlaceItem.decorationType))
            {
                RemovePlacedSingleton(pendingPlaceItem.decorationType);
            }

            SetPreviewVisual(previewInstance, false);
            TagPlacedInstance(previewInstance, pendingPlaceItem.decorationType);

            currentSnowmanData.AddDecorationItem(pendingPlaceItem);
            Debug.Log("데코레이션 적용 완료(자유 배치): " + pendingPlaceItem.itemName);

            previewInstance = null;
            pendingPlaceItem = null;
        }

        // 우클릭: 취소
        if (Input.GetMouseButtonDown(1))
        {
            if (previewInstance != null) Destroy(previewInstance);
            previewInstance = null;
            pendingPlaceItem = null;
            Debug.Log("배치 모드 취소");
        }
    }

    /// <summary>
    /// 스노우맨 표면에 대한 레이캐스트 히트 시도
    /// </summary>
    /// <param name="hit"></param>
    /// <returns></returns>
    private bool TryGetSnowmanSurfaceHit(out RaycastHit hit)
    {
        hit = default;
        if (Camera.main == null) return false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit, maxRayDistance, snowmanSurfaceMask))
            return false;

        // 스노우맨 오브젝트(또는 Up/Down 파트) 하위인지 체크해서 이상한 곳에 설치 방지
        if (snowmanObject != null)
        {
            if (!hit.transform.IsChildOf(snowmanObject.transform) && hit.transform != snowmanObject.transform)
                return false;
        }
        else
        {
            // snowmanObject 미지정이면 Up/Down 기준으로라도 체크
            if (snowmanUpPart != null && hit.transform.IsChildOf(snowmanUpPart)) return true;
            if (snowmanDownPart != null && hit.transform.IsChildOf(snowmanDownPart)) return true;

            // 둘 다 아니면 실패
            return false;
        }

        return true;
    }



    /// <summary>
    /// 표면 오프셋 값 가져오기
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private float GetSurfaceOffset(DecorationItem item)
    {
        // DecorationItem에 offset 필드가 없다면 기본값 사용
        // (있으면 그걸로 쓰게 TryGetSurfaceOffset 같은 걸로 확장 가능)
        return defaultSurfaceOffset;
    }

    /// <summary>
    /// 프리뷰 시각적 처리
    /// </summary>
    /// <param name="go">설치된 프리뷰 게임 오브젝트</param>
    /// <param name="isPreview">데코레이션 타입</param>
    private void SetPreviewVisual(GameObject go, bool isPreview)
    {
        // 프리뷰 중엔 콜라이더 꺼서 레이캐스트/겹침 방해 줄이기
        foreach (var col in go.GetComponentsInChildren<Collider>())
            col.enabled = !isPreview;
    }

    /// <summary>
    /// 설치된 인스턴스 태그 지정
    /// </summary>
    /// <param name="go">설치된 프리뷰 게임 오브젝트</param>
    /// <param name="type">데코레이션 타입</param>
    private void TagPlacedInstance(GameObject go, DecorationType type)
    {
        // 나중에 싱글톤 제거/관리용으로 name에 타입 넣기(변수명/레퍼런스 영향 없음)
        go.name = $"Placed_{type}_{go.name}";
    }


/// <summary>
/// 싱글톤 타입 설치된 오브젝트 제거
/// </summary>
/// <param name="type"></param>
    private void RemovePlacedSingleton(DecorationType type)
    {
        Transform root = placeablesRoot ? placeablesRoot : transform;
        // 뒤에서부터 찾아서 삭제(여러 개 있으면 전부 삭제)
        for (int i = root.childCount - 1; i >= 0; i--)
        {
            var child = root.GetChild(i);
            if (child != null && child.name.StartsWith($"Placed_{type}_"))
            {
                Destroy(child.gameObject);
            }
        }
    }
    /// <summary>
    /// 배치 프리팹 가져오기
    /// </summary>
    /// <param name="item">아이템 데이터</param>
    /// <param name="prefab">프리팹 게임 오브젝트</param>
    private bool TryGetPlacePrefab(DecorationItem item, out GameObject prefab)
    {
        prefab = item.placeablePrefab;
        return prefab != null;
    }
}
