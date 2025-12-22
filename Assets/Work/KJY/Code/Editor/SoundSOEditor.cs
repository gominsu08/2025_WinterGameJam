using UnityEditor;
using UnityEngine;
using Work.KJY.Code.Sound;

namespace Work.KJY.Code.Editor
{
    [CustomEditor(typeof(SoundSO))]
    public class SoundSOEditor : UnityEditor.Editor
    {
        private static AudioSource _previewer;

        void OnEnable()
        {
            // Create a hidden, static AudioSource to play the preview if it doesn't exist
            if (_previewer == null)
            {
                GameObject previewObject = new GameObject("AudioPreview");
                previewObject.hideFlags = HideFlags.HideAndDontSave;
                _previewer = previewObject.AddComponent<AudioSource>();
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SoundSO soundSO = (SoundSO)target;

            if (soundSO.clip != null)
            {
                if (GUILayout.Button("미리듣기"))
                {
                    PlayClip(soundSO.clip);
                }
            }

            if (GUILayout.Button("정지"))
            {
                StopClip();
            }
        }

        private void PlayClip(AudioClip clip)
        {
            if (_previewer != null)
            {
                _previewer.Stop();
                _previewer.clip = clip;
                _previewer.volume = ((SoundSO)target).volume; // Set volume from SoundSO
                _previewer.Play();
            }
        }

        private void StopClip()
        {
            if (_previewer != null && _previewer.isPlaying)
            {
                _previewer.Stop();
            }
        }
    }
}
