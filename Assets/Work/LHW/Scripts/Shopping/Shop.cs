 using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop Instance;

    [Header("Shop Items Data")]
    [SerializeField] private ShoppingData[] shoppingDatas;
    [SerializeField] private ItemType filterType;


    [Header("UI Elements")]
    [SerializeField] private GameObject shopUI;
    [SerializeField] private Transform itemsParent;
    [SerializeField] private GameObject shopItemPrefab;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        InitUI();
    }

    //<summary>
    // 필터링에 맞춰 아이템 데이터 초기화
    //</summary>
    private void InitUI()
    {
        foreach(Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        foreach(var data in shoppingDatas)
        {
            if(data.item.itemType != filterType) continue;

            GameObject itemObj = Instantiate(shopItemPrefab, itemsParent);
            itemObj.GetComponent<ShopItemButtonInitiator>().Init(data);
        }
    }

    /// <summary>
    /// 아이템 구매 메서드
    /// </summary>
    /// <param name="data">구매 데이터</param>
    public bool PurchaseItem(ShoppingData data)
    {
        //구매 로직
        if(Inventory.Instance.SpendMoney(data.price))
        {
            Inventory.Instance.AddItem(data.item);
            Debug.Log("구매 완료: " + data.item.itemName + " | 가격: " + data.price + " | 남은 돈: " + Inventory.Instance.money);
            return true;
        }
        else
        {
            Debug.LogWarning("돈이 부족합니다!");
            return false;
        }
    }

    /// <summary>
    /// 필터 타입 변경 메서드
    /// </summary>
    /// <param name="type">0 = Deco, 1 = Buff</param>
    public void ChangeFilterType(int type)
    {
        filterType = (ItemType)type;
        InitUI();
    }
}
