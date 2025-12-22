using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum WeatherType
{
    Sunny,
    Rainy,
    Snowy,
    Cloudy,
}

public class WeatherSystem : MonoBehaviour
{
    private WeatherType currentWeather;
    public GameObject rainEffect;
    public GameObject snowEffect;
    public GameObject cloudyEffect;
    public GameObject sunnyEffect;

    void Start()
    {
        StartCoroutine(Co_TestRoutine());
    }

    IEnumerator Co_TestRoutine()
    {
        ChangeWeather(WeatherType.Sunny);
        while (true)
        {
            yield return new WaitForSeconds(5f);
            ChangeWeather((WeatherType)Random.Range(0, 4));
        }
    }
    /// <summary>
    /// 날씨 변경 메서드
    /// </summary>
    /// <param name="newWeather">변경할 날씨 타입</param>
    public void ChangeWeather(WeatherType newWeather)
    {
        currentWeather = newWeather;

        rainEffect.SetActive(currentWeather == WeatherType.Rainy);
        snowEffect.SetActive(currentWeather == WeatherType.Snowy);
        cloudyEffect.SetActive(currentWeather == WeatherType.Cloudy);
        sunnyEffect.SetActive(currentWeather == WeatherType.Sunny);

        //여기다 추가 로직 작성하심 될거같아여

        Debug.Log("날씨가 " + newWeather.ToString() + "(으)로 변경되었습니다.");
    }
}
