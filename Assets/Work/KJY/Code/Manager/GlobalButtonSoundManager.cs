using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using Work.KJY.Code.Core;

namespace Work.KJY.Code.Manager
{
    public class GlobalButtonSoundManager : MonoSingleton<GlobalButtonSoundManager>
    {
        [SerializeField]
        private string buttonClickSoundKey = "ButtonClick";

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            AssignSoundToAllButtons();
        }

        private void Start()
        {
            AssignSoundToAllButtons();
        }

        private void AssignSoundToAllButtons()
        {
            Button[] allButtons = FindObjectsOfType<Button>(true);
            
            foreach (Button button in allButtons)
            {
                button.onClick.AddListener(PlayButtonClickSound);
            }
        }

        private void PlayButtonClickSound()
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.Play2DSFX(buttonClickSoundKey);
            }
            else
            {
                Debug.LogWarning("[GlobalButtonSoundManager] SoundManager.Instance가 존재하지 않습니다.");
            }
        }
    }
}
