using UnityEditor;
using Work.KJY.Code.Sound;

namespace Work.KJY.Code.Editor
{
    public class SoundAssetProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            bool needsUpdate = false;

            foreach (string path in importedAssets)
            {
                if (path.EndsWith(".asset") && AssetDatabase.LoadAssetAtPath<SoundSO>(path) != null)
                {
                    needsUpdate = true;
                    break;
                }
            }

            if (!needsUpdate)
            {
                foreach (string path in deletedAssets)
                {
                    if (path.EndsWith(".asset"))
                    {
                        needsUpdate = true;
                        break;
                    }
                }
            }

            if (!needsUpdate)
            {
                foreach (string path in movedAssets)
                {
                    if (path.EndsWith(".asset") && AssetDatabase.LoadAssetAtPath<SoundSO>(path) != null)
                    {
                        needsUpdate = true;
                        break;
                    }
                }
            }
            
            if (!needsUpdate)
            {
                foreach (string path in movedFromAssetPaths)
                {
                    if (path.EndsWith(".asset"))
                    {
                        needsUpdate = true;
                        break;
                    }
                }
            }

            if (needsUpdate)
            {
                UpdateSoundLists();
            }
        }

        [MenuItem("Tools/Update Sound Lists")]
        public static void UpdateSoundLists()
        {
            string[] guids = AssetDatabase.FindAssets("t:SoundListSO");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                SoundListSO soundList = AssetDatabase.LoadAssetAtPath<SoundListSO>(path);
                if (soundList != null)
                {
                    ClearAndRegisterSounds(soundList);
                }
            }
            AssetDatabase.SaveAssets();
        }

        private static void ClearAndRegisterSounds(SoundListSO soundList)
        {
            soundList.soundList.RemoveAll(item => item == null);
            soundList.soundList.Clear();
            
            string folderPath = soundList.soundType == SoundType.BGM
                ? "Assets/Work/KJY/99_Sounds/BGM"
                : "Assets/Work/KJY/99_Sounds/SFX";

            string[] soundGuids = AssetDatabase.FindAssets("t:SoundSO", new[] { folderPath });
            foreach (string soundGuid in soundGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(soundGuid);
                SoundSO sound = AssetDatabase.LoadAssetAtPath<SoundSO>(path);

                if (sound != null && !soundList.soundList.Contains(sound))
                {
                    soundList.soundList.Add(sound);
                }
            }
            EditorUtility.SetDirty(soundList);
        }
    }
}
