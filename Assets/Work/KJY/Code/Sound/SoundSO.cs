using UnityEngine;

namespace Work.KJY.Code.Sound
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "SO/Sound/SoundData")]
    public class SoundSO : ScriptableObject
    {
        [Header("사운드 기본 정보")]
        public string key;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        public bool loop = false;
        public int min_ListenDistance = 5;
        public int max_ListenDistance = 500;
    }
}