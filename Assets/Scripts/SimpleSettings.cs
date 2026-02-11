using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SimpleSettings : MonoBehaviour
{
    [Header("UI References")]
    public Slider volumeSlider;
    public TMP_Dropdown qualityDropdown;

    void Start()
    {
        // --- 1. SETUP AUDIO ---
        float savedVol = PlayerPrefs.GetFloat("Volume", 1.0f);
        volumeSlider.value = savedVol;
        AudioListener.volume = savedVol;

        // --- 2. SETUP GRAPHICS (The Renaming Part 🎨) ---
        qualityDropdown.ClearOptions();

        // instead of asking Unity for names, we force our own names.
        // Index 0 = "Low" (Maps to Mobile)
        // Index 1 = "High" (Maps to PC)
        List<string> customNames = new List<string> { "Low", "High" };
        qualityDropdown.AddOptions(customNames);

        // Load saved quality (Default to 0 / Low)
        int savedQuality = PlayerPrefs.GetInt("Quality", 0); 
        
        qualityDropdown.value = savedQuality;
        QualitySettings.SetQualityLevel(savedQuality);
        
        // --- 3. LISTENERS ---
        volumeSlider.onValueChanged.AddListener(SetVolume);
        qualityDropdown.onValueChanged.AddListener(SetQuality);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetQuality(int index)
    {
        // Even though the UI says "High", Unity receives index 1
        // which matches "PC" in your project settings.
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("Quality", index);
        
        Debug.Log("Switched to Quality Index: " + index);
    }
}