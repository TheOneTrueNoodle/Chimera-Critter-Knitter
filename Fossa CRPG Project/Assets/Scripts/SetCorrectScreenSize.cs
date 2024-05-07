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
        Resolution currentMonitorResolution;

        currentMonitorResolution = Screen.currentResolution;
        int width = currentMonitorResolution.width;
        int height = currentMonitorResolution.height;

        Screen.SetResolution(width, height, PlayerPrefs.GetInt("Fullscreen", 1) == 1);
        ToggleFullscreen(PlayerPrefs.GetInt("Fullscreen") == 1);
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
