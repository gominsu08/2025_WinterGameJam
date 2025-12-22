using UnityEngine;
using Work.KJY.Code.Manager;

namespace Work.KJY.Code.UI
{
    public class TitleController : MonoBehaviour
    {
        public void StartButton()
        {
            FadeManager.Instance.FadeIn(2, "Boost Item Test");
        }

        public void SettingButton()
        {
            SettingManager.Instance.ToggleSettings();
        }

        public void QuitButton()
        {
            Application.Quit();
        
        }
    }
}