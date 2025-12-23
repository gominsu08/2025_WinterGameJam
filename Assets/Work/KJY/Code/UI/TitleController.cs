using UnityEngine;
using Work.KJY.Code.Manager;

namespace Work.KJY.Code.UI
{
    public class TitleController : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        public void StartButton()
        {
            IrisFadeManager.Instance.FadeIn(2, sceneName);
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