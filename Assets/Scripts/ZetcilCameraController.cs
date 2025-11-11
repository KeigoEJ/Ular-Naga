using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ZetcilCameraController : MonoBehaviour
{
    [Header("Main Settings")]
    public RawImage cameraDisplay;      
    public TMP_Dropdown cameraDropdown; 

    private WebCamTexture webcamTexture;
    private bool isCameraActive = false;
    private int selectedCameraIndex = 0;

    void Start()
    {
        if (cameraDropdown != null)
            cameraDropdown.onValueChanged.AddListener(OnCameraSelected);
    }

    void OnCameraSelected(int index)
    {
        selectedCameraIndex = index;
        if (isCameraActive)
        {
            // Restart kamera dengan pilihan baru
            webcamTexture.Stop();
            StartCamera(selectedCameraIndex);
        }
    }

    public void StartCamera()
    {
        if (!isCameraActive)
        {
            StartCamera(selectedCameraIndex);
        }
    }

    public void StopCamera()
    {
        if (isCameraActive)
        {
            webcamTexture.Stop();
            isCameraActive = false;
        }
    }

    void StartCamera(int index)
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > 0 && index < devices.Length)
        {
            webcamTexture = new WebCamTexture(devices[index].name);
            cameraDisplay.texture = webcamTexture;
            webcamTexture.Play();
            isCameraActive = true;
        }
        else
        {
            Debug.LogWarning("No camera devices found or invalid index!");
        }
    }
}
