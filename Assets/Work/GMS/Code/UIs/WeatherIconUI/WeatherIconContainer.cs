using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Work.Utils.EventBus;

namespace Assets.Work.GMS.Code.UIs.WeatherIconUI
{
    public class WeatherIconContainer : MonoBehaviour
    {
        [SerializeField] private Image weatherIcon, weatherIconShadow;

        [SerializeField] private Sprite sunnyIcon, rainyIcon, snowyIcon, cloudyIcon;

        [SerializeField] private TextMeshProUGUI text; 


        private const string SunnyText = "화창한 날";
        private const string SnowText = "눈오는 날";
        private const string RainyText = "비오는 날";
        private const string CloudyText = "구름낀 날";

        private void Awake()
        {
            Bus<WeatherChangeEvent>.Events += HandleWeatherChange;
        }

        private void OnDestroy()
        {
            
            Bus<WeatherChangeEvent>.Events -= HandleWeatherChange;
        }

        private void HandleWeatherChange(WeatherChangeEvent evt)
        {
            switch (evt.NewWeather)
            {
                case WeatherType.Sunny:
                    weatherIcon.sprite = sunnyIcon;
                    weatherIconShadow.sprite = sunnyIcon;
                    text.SetText(SunnyText);
                    break;
                case WeatherType.Rainy:
                    weatherIcon.sprite = rainyIcon;
                    weatherIconShadow.sprite = rainyIcon;
                    text.SetText(RainyText);
                    break;
                case WeatherType.Snowy:
                    weatherIcon.sprite = snowyIcon;
                    weatherIconShadow.sprite = snowyIcon;
                    text.SetText(SnowText);
                    break;
                case WeatherType.Cloudy:
                    weatherIcon.sprite = cloudyIcon;
                    weatherIconShadow.sprite = cloudyIcon;
                    text.SetText(CloudyText);
                    break;
            }
        }
    }
}