using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Work.KJY.Code.Core;
using Work.KJY.Code.Sound;

namespace Work.KJY.Code.Manager
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        [Header("사운드 리스트")]
        [SerializeField] private SoundListSO bgmListSO;
        [SerializeField] private SoundListSO sfxListSO;

        [Header("AudioSource 프리팹")]
        [SerializeField] private AudioSource bgmSourcePrefab;
        [SerializeField] private AudioSource sfxSourcePrefab;
        
        [Header("AudioSource 설정")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private int bgmPoolSize = 3;
        
        private List<AudioSource> _bgmSources;

        private void Awake()
        {
            _bgmSources = new List<AudioSource>();
            for (int i = 0; i < bgmPoolSize; i++)
            {
                var bgmSource = Instantiate(bgmSourcePrefab, transform);
                _bgmSources.Add(bgmSource);
            }
        }

        private void Update()
        {
            if (Keyboard.current.wKey.wasPressedThisFrame)
                PlayBGM("TEST");
        }

        public void PlayBGM(string key)
        {
            var data = bgmListSO.GetSoundData(key);
            if (data == null || data.clip == null)
            {
                Debug.LogWarning($"[SoundManager] BGM '{key}'를 찾을 수 없습니다.");
                return;
            }

            AudioSource availableSource = _bgmSources.Find(source => !source.isPlaying);

            if (availableSource == null)
            {
                Debug.LogWarning("[SoundManager] 모든 BGM용 AudioSource가 사용중입니다.");
                // 새로운 AudioSource를 생성하여 풀에 추가
                availableSource = Instantiate(bgmSourcePrefab, transform);
                _bgmSources.Add(availableSource);
                return;
            }
            
            foreach (var source in _bgmSources)
            {
                if (source.isPlaying && source.clip == data.clip)
                {
                    return;
                }
            }

            availableSource.clip = data.clip;
            availableSource.volume = data.volume;
            availableSource.loop = data.loop;
            availableSource.Play();
        }

        public void StopBGM()
        {
            foreach (var source in _bgmSources)
            {
                source.Stop();
            }
        }

        public void Play2DSFX(string key)
        {
            var data = sfxListSO.GetSoundData(key);
            if (data == null || data.clip == null)
            {
                Debug.LogWarning($"[SoundManager] SFX '{key}'를 찾을 수 없습니다.");
                return;
            }

            sfxSource.PlayOneShot(data.clip, data.volume);
        }

        public void Play3DSFX(string key, Vector3 position)
        {
            var data = sfxListSO.GetSoundData(key);
            if (data == null || data.clip == null)
            {
                Debug.LogWarning($"[SoundManager] SFX '{key}'를 찾을 수 없습니다.");
                return;
            }

            if (sfxSourcePrefab == null)
            {
                Debug.LogError("[SoundManager] SFX AudioSource Prefab이 설정되지 않았습니다!");
                return;
            }

            AudioSource source = Instantiate(sfxSourcePrefab, position, Quaternion.identity);
            source.clip = data.clip;
            source.volume = data.volume;
            source.loop = data.loop;

            source.spatialBlend = 1f;
            source.minDistance = data.min_ListenDistance;
            source.maxDistance = data.max_ListenDistance;
            source.rolloffMode = AudioRolloffMode.Logarithmic;

            source.Play();

            if (!data.loop)
                Destroy(source.gameObject, data.clip.length);
        }
    }
}
