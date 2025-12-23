using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Work.GMS.Code.UIs.BoostUIs
{
    public class BoostIcon : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI text;

        public void SetIcon(string str)
        {
            text.SetText(str);
        }

        public void SetColor(Color color)
        {
            iconImage.color = color;
        }
    }
}