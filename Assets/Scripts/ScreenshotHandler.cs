using System.Collections;
using System.IO;
using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    public Canvas mainUICanvas;

    public void TakeScreenshot()
    {
        StartCoroutine(CaptureAndSave());
    }

    private IEnumerator CaptureAndSave()
    {
        // Hide UI (optional)
        if (mainUICanvas != null)
            mainUICanvas.enabled = false;

        yield return new WaitForEndOfFrame();

        // Capture screen
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        if (mainUICanvas != null)
            mainUICanvas.enabled = true;

        byte[] bytes = ss.EncodeToPNG();
        Destroy(ss);

        string fileName = "AR_Screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

#if UNITY_ANDROID
        string folderPath = "/storage/emulated/0/Pictures/MyARGame/";
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string fullPath = Path.Combine(folderPath, fileName);
#else
        string fullPath = Path.Combine(Application.persistentDataPath, fileName);
#endif

        File.WriteAllBytes(fullPath, bytes);
        Debug.Log("Saved to: " + fullPath);

#if UNITY_ANDROID
        // Refresh gallery
        using (AndroidJavaClass mediaScan = new AndroidJavaClass("android.media.MediaScannerConnection"))
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                mediaScan.CallStatic("scanFile", context, new string[] { fullPath }, null, null);
            }
        }
#endif
    }
}
