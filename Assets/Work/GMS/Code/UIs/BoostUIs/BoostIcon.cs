using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Work.GMS.Code.UIs.BoostUIs
{
    public class BoostIcon : MonoBehaviour
    {
        [SerializeField] private Image ming;

        public void SetIcon(Sprite str)
        {
            ming.sprite = (str);
        }

        public void SetColor(Color color)
        {
            //iconImage.color = color;
        }
    }
}