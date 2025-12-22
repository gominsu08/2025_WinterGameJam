using System;
using UnityEngine;
using System.Collections;
using System.IO;

public class SnowmanScreenshot : MonoBehaviour
{
    public GameObject decoCanvas;
    public GameObject designCanvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            ScreenShot();
        }
    }

    public void ScreenShot()
    {
        StartCoroutine(Capture());
    }

    IEnumerator Capture()
    {
        decoCanvas.SetActive(false);
        designCanvas.SetActive(true);

        // 캔버스 상태가 실제로 렌더되도록 한 프레임 대기
        yield return new WaitForEndOfFrame();

        string dir = Application.dataPath + "/Screenshot";
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string path =
            $"{dir}/Snowman_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";

        ScreenCapture.CaptureScreenshot(path);

        Debug.Log($"Screenshot saved: {path}");

        // 다시 UI 복구
        yield return new WaitForEndOfFrame();
        decoCanvas.SetActive(true);
        designCanvas.SetActive(false);
    }
}
