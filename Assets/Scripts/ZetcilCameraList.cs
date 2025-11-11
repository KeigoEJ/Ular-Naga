using UnityEngine;
using TMPro;  

public class ZetcilCameraList : MonoBehaviour
{
    public TMP_Dropdown cameraDropdown;

    void Start()
    {
        cameraDropdown.ClearOptions();
        WebCamDevice[] devices = WebCamTexture.devices;
        foreach (WebCamDevice device in devices)
        {
            cameraDropdown.options.Add(new TMP_Dropdown.OptionData(device.name));
        }
        cameraDropdown.RefreshShownValue();
    }
}

