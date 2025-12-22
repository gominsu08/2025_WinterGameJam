using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public int hour;

    public Gradient timeColorGradient;
    public Light directionalLight;

    public void ChangeHour(float hourNormalized)
    {
        Color lightColor = timeColorGradient.Evaluate(hourNormalized / 24f);
        directionalLight.color = lightColor;
    }
}
