using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Settings Variables")]
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private double currentRefreshRate;
    private int currentResolutionIndex = 0;

    [Header("UI References")]
    public TMP_Dropdown resolutionDropdown;

    //Before Applying Settings
    private int newResolutionIndex;

    private void OnEnable()
    {
        GetResolutionOptions();
    }

    public void ApplySettings()
    {
        currentResolutionIndex = newResolutionIndex;
    }

    #region Resolution Settings
    private void GetResolutionOptions()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRateRatio.value + " Hz";
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
        newResolutionIndex = resolutionIndex;
    }
    #endregion
}
