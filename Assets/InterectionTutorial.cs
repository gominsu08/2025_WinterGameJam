using TMPro;
using UnityEngine;
using UnityEngine.Video;
using Work.KJY.Code.Manager;

public class InterectionTutorial : MonoBehaviour
{
    private MeshRenderer renderer;

    [TextArea(3,10)]
    public string tutorialDesc;

    public TMP_Text tutorialText;

    public GameObject tutorialVideo;
    public VideoPlayer videoPlayer;
    public VideoClip videoClip;

    public Material disableMaterial;
    public Material enableMaterial;

    public bool isGoal = false;

    void Start()
    {
        renderer = GetComponent<MeshRenderer>();

        IrisFadeManager.Instance.FadeOut(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(isGoal) IrisFadeManager.Instance.FadeIn(1, "Merge");
            else{
                tutorialText.text = tutorialDesc;
                renderer.material = enableMaterial;
                videoPlayer.clip = videoClip;
                videoPlayer.Play();
                tutorialVideo.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialVideo.SetActive(false);
            videoPlayer.Stop();
            renderer.material = disableMaterial;
            
        }
    }
}
