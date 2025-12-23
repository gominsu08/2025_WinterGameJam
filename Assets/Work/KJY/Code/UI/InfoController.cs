using System;
using TMPro;
using UnityEngine;
using Work.KJY.Code.Event;
using Work.KJY.Code.Manager;
using Work.Utils.EventBus;

namespace Work.KJY.Code.UI
{
    public class InfoController : MonoBehaviour
    {
        [SerializeField] private TMP_Text curMoneyText;

        private int _curMoney;

        private void Start()
        {
            IrisFadeManager.Instance.FadeOut();
            Bus<ChangeMoneyEvent>.Events += OnChangedMoney;
        }

        private void OnChangedMoney(ChangeMoneyEvent evt)
        {
            _curMoney = evt.CurMoney;
            curMoneyText.SetText(_curMoney.ToString());
        }

        public void ToggleSetting()
        {
            SettingManager.Instance.ToggleSettings();
        }
    }
}