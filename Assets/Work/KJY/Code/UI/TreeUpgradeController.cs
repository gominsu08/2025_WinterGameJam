using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using Work.KJY.Code.Event;
using Work.KJY.Code.Manager;
using Work.Utils.EventBus;

namespace Work.KJY.Code.UI
{
    public class TreeUpgradeController : MonoBehaviour
    {
        [SerializeField] private GameObject interactPanel;
        [SerializeField] private TMP_Text curLevelText;
        [SerializeField] private TMP_Text levelUpText;

        [SerializeField] private Transform[] levelUpCameraPositions;
        [SerializeField] private Transform[] endingCutScene;
        [SerializeField] private GameObject endingCanvas;

        [SerializeField] private CanvasGroup uiCanvasGroup;
        
        private void Start()
        {
            Inventory.Instance.AddMoney(100000);
            interactPanel.SetActive(false);
            Bus<TreeInteractEvent>.Events += OnCanInteract;
            Bus<PlazaLevelUpgradedEvent>.Events += OnPlazaLevelUpgraded;
        }

        private void OnDestroy()
        {
            Bus<TreeInteractEvent>.Events -= OnCanInteract;
            Bus<PlazaLevelUpgradedEvent>.Events -= OnPlazaLevelUpgraded;
        }
        
        private void OnCanInteract(TreeInteractEvent evt)
        {
            interactPanel.SetActive(evt.IsInPlayer);
            if (evt.IsInPlayer)
            {
                UpdateUI();
            }
        }
        
        private void OnPlazaLevelUpgraded(PlazaLevelUpgradedEvent evt)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            string defaultLevelText = "Lv.";
            string curLevel = (PlazaManager.Instance.GetCurLevel() - 1).ToString();
            curLevelText.SetText(defaultLevelText + curLevel);

            if (PlazaManager.Instance.IsMaxLevel)
            {
                string text = "엔딩 보기";
                levelUpText.color = Color.white;
                levelUpText.SetText(text);
            }
            else
            {
                string defaultText = "레벨 업     ";
                int needMoney = PlazaManager.Instance.GetNeedMoney();
                
                if (Inventory.Instance.CheckMoney(needMoney))
                {
                    levelUpText.color = Color.green;
                }
                else
                {
                    levelUpText.color = Color.red;
                }
            
                levelUpText.SetText(defaultText + needMoney);
            }
        }

        public void LevelUpPlaza()
        {
            if(PlazaManager.Instance.IsMaxLevel) StartCoroutine(Co_Ending());

            int needMoney = PlazaManager.Instance.GetNeedMoney();
            if (needMoney > 0 && Inventory.Instance.SpendMoney(needMoney))
            {
                int level = PlazaManager.Instance.LevelUp();
                if(level != -1) StartCoroutine(Co_CameraMove(level));
            }
        }

        IEnumerator Co_CameraMove(int level)
        {
            yield return null;
            Camera.main.GetComponent<CinemachineBrain>().enabled = false;
            Camera.main.transform.DOMove(levelUpCameraPositions[level - 2].position, 1f);
            Camera.main.transform.DORotate(levelUpCameraPositions[level - 2].eulerAngles, 1f);
            yield return new WaitForSeconds(2f);
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;
        }

        IEnumerator Co_Ending()
        {
            yield return null;
            uiCanvasGroup.DOFade(0, 1f);
            yield return new WaitForSeconds(1.5f);
            Camera.main.GetComponent<CinemachineBrain>().enabled = false;
            for (int i = 0; i < endingCutScene.Length; i++)
            {
                Camera.main.transform.DOMove(endingCutScene[i].position, 3f);
                Camera.main.transform.DORotate(endingCutScene[i].eulerAngles, 3f);
                yield return new WaitForSeconds(3f);
            }
            endingCanvas.SetActive(true);

            yield return new WaitForSeconds(5f);
            IrisFadeManager.Instance.FadeIn(1f, "TitleScene");
        }
    }
}