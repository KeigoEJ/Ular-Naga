using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ARPauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pausePanel;   // pause UI panel
    public RawImage freezeImage;    // RawImage for frozen frame

    private bool isPaused = false;
    private Texture2D freezeTexture;

    public void TogglePause()
    {
        if (isPaused) Resume();
        else StartCoroutine(CaptureAndPause());
    }

    public void Resume()
    {
        if (!isPaused) return;
        StartCoroutine(ResumeRoutine());
    }

    private IEnumerator CaptureAndPause()
    {
        if (isPaused) yield break;

        // Wait for full frame render to complete
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;

        if (freezeTexture != null)
        {
            Destroy(freezeTexture);
            freezeTexture = null;
        }

        // Create RenderTexture safely for ARCamera
        RenderTexture rt = new RenderTexture(width, height, 24);
        ScreenCapture.CaptureScreenshotIntoRenderTexture(rt);

        // Copy pixels into Texture2D
        RenderTexture.active = rt;
        freezeTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
        freezeTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        freezeTexture.Apply();
        RenderTexture.active = null;
        rt.Release();

        // 🔥 Fix orientation (flip horizontally) <-- **** THIS IS THE CHANGED LINE ****
        freezeTexture = FlipTextureHorizontally(freezeTexture);

        // Apply to UI and stretch to fill
        if (freezeImage != null)
        {
            freezeImage.texture = freezeTexture;
            freezeImage.gameObject.SetActive(true);

            // Force stretch to full canvas
            freezeImage.rectTransform.anchorMin = Vector2.zero;
            freezeImage.rectTransform.anchorMax = Vector2.one;
            freezeImage.rectTransform.offsetMin = Vector2.zero;
            freezeImage.rectTransform.offsetMax = Vector2.zero;
        }

        // Show pause UI
        if (pausePanel != null)
            pausePanel.SetActive(true);

        // Pause Vuforia tracking
        if (VuforiaBehaviour.Instance != null)
            VuforiaBehaviour.Instance.enabled = false;

        isPaused = true;
    }

    private IEnumerator ResumeRoutine()
    {
        if (!isPaused) yield break;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (freezeImage != null)
            freezeImage.gameObject.SetActive(false);

        if (VuforiaBehaviour.Instance != null)
            VuforiaBehaviour.Instance.enabled = true;

        yield return null;

        if (freezeTexture != null)
        {
            Destroy(freezeTexture);
            freezeTexture = null;
        }

        isPaused = false;
    }

    // **** THIS IS THE NEW FUNCTION I ADDED ****
    private Texture2D FlipTextureHorizontally(Texture2D original)
    {
        Texture2D flipped = new Texture2D(original.width, original.height, original.format, false);
        int w = original.width;
        int h = original.height;
        
        for (int y = 0; y < h; y++) // Iterate row by row
        {
            // Get the original row of pixels
            Color[] originalPixels = original.GetPixels(0, y, w, 1);
            
            // Create a new array for the flipped pixels
            Color[] flippedPixels = new Color[w];
            
            // Reverse the order of pixels in the row
            for (int x = 0; x < w; x++)
            {
                flippedPixels[x] = originalPixels[w - 1 - x];
            }
            
            // Set the flipped row of pixels in the new texture
            flipped.SetPixels(0, y, w, 1, flippedPixels);
        }
        
        flipped.Apply();
        Destroy(original); // Destroy the original texture to save memory
        return flipped;
    }

    private Texture2D FlipTextureVertically(Texture2D original)
    {
        Texture2D flipped = new Texture2D(original.width, original.height, original.format, false);
        int w = original.width;
        int h = original.height;
        for (int y = 0; y < h; y++)
        {
            flipped.SetPixels(0, y, w, 1, original.GetPixels(0, h - y - 1, w, 1));
        }
        flipped.Apply();
        Destroy(original); // Destroy the original texture to save memory
        return flipped;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    private void OnDestroy()
    {
        if (freezeTexture != null)
            Destroy(freezeTexture);
    }
}