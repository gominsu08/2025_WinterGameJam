using System;
using TMPro;
using UnityEngine;
using Work.KJY.Code.Manager;

namespace Work.KJY.Code.UI
{
    public class InfoController : MonoBehaviour
    {
        [SerializeField] private TMP_Text curMoneyText;

        private void Start()
        {
            FadeManager.Instance.FadeOut();
        }

        public void ToggleSetting()
        {
            SettingManager.Instance.ToggleSettings();
        }
    }
}