using TMPro;
using UnityEngine;
using Work.GMS.Code.Snowballs;

public class RadiuseTextUI : MonoBehaviour
{
    [SerializeField] private Snowball snowball;

    [SerializeField] private TextMeshProUGUI textMeshPro;

    public void Update()
    {
        textMeshPro.text = $"{snowball.CurrentSnowRadius:0.00} m";
    }
}
