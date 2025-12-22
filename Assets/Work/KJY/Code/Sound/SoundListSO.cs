using System.Collections.Generic;
using UnityEngine;

namespace Work.KJY.Code.Sound
{
    public enum SoundType
    {
        BGM,
        SFX
    }

    [CreateAssetMenu(fileName = "SoundList",menuName = "SO/Sound/SoundList")]
    public class SoundListSO : ScriptableObject
    {
        [Header("리스트 타입 (BGM 또는 SFX)")]
        public SoundType soundType;

        [Header("사운드 리스트")]
        public List<SoundSO> soundList = new List<SoundSO>();

        public SoundSO GetSoundData(string key)
        {
            return soundList.Find(x => x.key == key);
        }
    }
}