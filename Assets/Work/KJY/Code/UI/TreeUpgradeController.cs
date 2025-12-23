using TMPro;
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
            string curLevel = PlazaManager.Instance.GetCurLevel().ToString();
            curLevelText.SetText(defaultLevelText + curLevel);

            if (PlazaManager.Instance.IsMaxLevel)
            {
                string text = "최대 레벨입니다.";
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
            int needMoney = PlazaManager.Instance.GetNeedMoney();
            if (needMoney > 0 && Inventory.Instance.SpendMoney(needMoney))
            {
                PlazaManager.Instance.LevelUp();
            }
        }
    }
}