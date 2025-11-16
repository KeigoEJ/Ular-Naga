using UnityEngine;
using System.Collections;

public class ARShareScreenshot : MonoBehaviour
{
    public GameObject uiPanel;  // Panel yang mau disembunyikan

    private string filePath;

    public void TakeScreenshotAndShare()
    {
        StartCoroutine(CaptureAndShare());
    }

    private IEnumerator CaptureAndShare()
    {
        // 1. Sembunyikan UI
        if (uiPanel != null)
            uiPanel.SetActive(false);

        yield return new WaitForEndOfFrame();

        // 2. Ambil screenshot
        Texture2D ss = ScreenCapture.CaptureScreenshotAsTexture();

        // 3. Simpan file
        filePath = System.IO.Path.Combine(Application.temporaryCachePath, "ARShare.png");
        System.IO.File.WriteAllBytes(filePath, ss.EncodeToPNG());
        Destroy(ss);

        // 4. Share + callback selesai share
        new NativeShare()
            .AddFile(filePath)
            .SetSubject("My AR Screenshot")
            .SetText("Check out my AR screenshot!")
            .SetCallback((result, shareTarget) =>
            {
                // 5. Balikin UI setelah share selesai
                if (uiPanel != null)
                    uiPanel.SetActive(true);
            })
            .Share();
    }
}
