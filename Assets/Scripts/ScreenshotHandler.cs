using System.Collections;
using System.IO; // Required for file operations
using UnityEngine;
using UnityEngine.UI; // Required for the optional UI part

public class ScreenshotHandler : MonoBehaviour
{
    [Header("Optional UI")]
    [Tooltip("Drag your main UI Canvas here if you want to hide it during the screenshot.")]
    public Canvas mainUICanvas;

    /// <summary>
    /// This is the public function your button will call
    /// </summary>
    public void TakeScreenshot()
    {
        // Start the coroutine that handles the screenshot logic
        StartCoroutine(CaptureAndSave());
    }

    private IEnumerator CaptureAndSave()
    {
        // --- 1. HIDE THE UI (Optional) ---
        // If you assigned a Canvas, this will hide it
        if (mainUICanvas != null)
        {
            mainUICanvas.enabled = false;
        }

        // --- 2. WAIT FOR THE SCREEN TO RENDER ---
        // We must wait for the end of the frame *after* the UI is hidden
        // to make sure the screen renders without the UI.
        yield return new WaitForEndOfFrame();

        // --- 3. CAPTURE THE SCREEN ---
        // Create a texture to hold the screenshot
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        
        // Read the pixels from the screen into the texture
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        // --- 4. SHOW THE UI AGAIN (Optional) ---
        // Re-enable the canvas immediately so the user doesn't see it flicker
        if (mainUICanvas != null)
        {
            mainUICanvas.enabled = true;
        }

        // --- 5. ENCODE AND SAVE THE FILE ---
        // Encode the texture to a PNG file
        byte[] bytes = ss.EncodeToPNG();
        
        // IMPORTANT: Clean up the texture from memory
        Destroy(ss);

        // Create a unique filename
        string fileName = "MyAR_Screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        
        // Application.persistentDataPath is a safe folder on all devices (iOS, Android, etc.)
        string path = Path.Combine(Application.persistentDataPath, fileName);
        
        // Save the file
        File.WriteAllBytes(path, bytes);
        Debug.Log($"Screenshot saved to: {path}");

        // --- 6. REFRESH THE ANDROID GALLERY ---
        // This is the magic part that makes it show up in the phone's gallery
        #if UNITY_ANDROID
        using (AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass classMediaScanner = new AndroidJavaClass("android.media.MediaScannerConnection"))
                {
                    classMediaScanner.CallStatic("scanFile", objActivity, new string[] { path }, null, null);
                }
            }
        }
        #endif
    }
}