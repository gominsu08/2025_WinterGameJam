using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Work.Utils.EventBus;

namespace Assets.Work.GMS.Code.UIs.WeatherIconUI
{
    public class WeatherIconContainer : MonoBehaviour
    {
        [SerializeField] private Image weatherIcon, weatherIconShadow;

        [SerializeField] private Sprite sunnyIcon, rainyIcon, snowyIcon, cloudyIcon;


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
                    break;
                case WeatherType.Rainy:
                    weatherIcon.sprite = rainyIcon;
                    weatherIconShadow.sprite = rainyIcon;
                    break;
                case WeatherType.Snowy:
                    weatherIcon.sprite = snowyIcon;
                    weatherIconShadow.sprite = snowyIcon;
                    break;
                case WeatherType.Cloudy:
                    weatherIcon.sprite = cloudyIcon;
                    weatherIconShadow.sprite = cloudyIcon;
                    break;
            }
        }
    }
}