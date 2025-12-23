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

        [SerializeField] private TextMeshProUGUI weatherText;

        private const string s = "화창한 날";
        private const string b = "비오는 날";
        private const string snow = "눈오는 날";
        private const string cl = "구름낀 날";


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
                    weatherText.text = s;
                    break;
                case WeatherType.Rainy:
                    weatherIcon.sprite = rainyIcon;
                    weatherIconShadow.sprite = rainyIcon;
                    weatherText.text = b;
                    break;
                case WeatherType.Snowy:
                    weatherIcon.sprite = snowyIcon;
                    weatherIconShadow.sprite = snowyIcon;
                    weatherText.text = snow;
                    break;
                case WeatherType.Cloudy:
                    weatherIcon.sprite = cloudyIcon;
                    weatherIconShadow.sprite = cloudyIcon;
                    weatherText.text = cl;
                    break;
            }
        }
    }
}