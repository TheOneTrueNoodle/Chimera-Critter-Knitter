using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCorrectScreenSize : MonoBehaviour
{
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private double currentRefreshRate;

    void Start()
    {
        GetResolutionOptions();
    }

    private void GetResolutionOptions()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

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
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + (int)filteredResolutions[i].refreshRateRatio.value + " Hz";
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                PlayerPrefs.GetInt("Resolution ID", i);
            }
        }

        ToggleFullscreen(PlayerPrefs.GetInt("Fullscreen") == 1);
        SetResolution(PlayerPrefs.GetInt("Resolution ID"));
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, PlayerPrefs.GetInt("Fullscreen") == 1);
    }

    public void ToggleFullscreen(bool value)
    {
        Screen.fullScreen = value;
    }
}
