using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Menu Manager")]
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private GameMenuManager gameMenuManager;
    [SerializeField] private GameObject applySettingsPopup;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private double currentRefreshRate;

    [Header("UI References")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private TMP_Text masterVolumePercent;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private TMP_Text musicVolumePercent;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private TMP_Text SFXVolumePercent;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle dogVisionToggle;
    [SerializeField] private Toggle removeInfectionToggle;
    [SerializeField] private Toggle invertCamYToggle;
    [SerializeField] private Toggle invertCamXToggle;
    [SerializeField] private Slider cameraSensitivitySlider;
    [SerializeField] private TMP_Text camSensitivityValue;
    [SerializeField] private Toggle removeGoreToggle;
    [SerializeField] private Toggle removeFilmGrainToggle;

    //Before Applying Settings
    public bool changedSettings;

    [Range(0, 1)] private float newMasterVolume;
    [Range(0, 1)] private float newMusicVolume;
    [Range(0, 1)] private float newSFXVolume;

    private int newResolutionIndex;
    private bool newFullscreen; //0 = Windowed, 1 = Fullscreen

    private bool newDogVision; //0 = Off, 1 = On
    private bool newInfectionEffect; //0 = Off, 1 = On

    private bool newInvertCamY; //0 = Off, 1 = On
    private bool newInvertCamX; //0 = Off, 1 = On
    private float newCamSens;

    private bool newRemoveGore; //0 = Off, 1 = On
    private bool newRemoveGrain; //0 = Off, 1 = On

    private void OnEnable()
    {
        GetCurrentSettings();
    }

    public void GetCurrentSettings()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("Master Volume", 1);
        masterVolumePercent.text = (PlayerPrefs.GetFloat("Master Volume", 1) * 100).ToString("F0") + "%";
        musicVolumeSlider.value = PlayerPrefs.GetFloat("Music Volume", 1);
        musicVolumePercent.text = (PlayerPrefs.GetFloat("Music Volume", 1) * 100).ToString("F0") + "%";
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFX Volume", 1);
        SFXVolumePercent.text = (PlayerPrefs.GetFloat("SFX Volume", 1) * 100).ToString("F0") + "%";

        dogVisionToggle.isOn = PlayerPrefs.GetInt("Dog Vision", 0) == 1;
        removeInfectionToggle.isOn = PlayerPrefs.GetInt("Infection Effect", 0) == 1;
        removeGoreToggle.isOn = PlayerPrefs.GetInt("Remove Gore", 0) == 1;
        removeFilmGrainToggle.isOn = PlayerPrefs.GetInt("Remove Grain", 0) == 1;

        invertCamYToggle.isOn = PlayerPrefs.GetInt("Invert Camera Y", 0) == 1;
        invertCamXToggle.isOn = PlayerPrefs.GetInt("Invert Camera X", 0) == 1;

        if (PlayerPrefs.GetFloat("Camera Sensitivity", 3) < 0.1f) { PlayerPrefs.SetFloat("Camera Sensitivity", 0.1f); }

        cameraSensitivitySlider.value = PlayerPrefs.GetFloat("Camera Sensitivity", 3);
        camSensitivityValue.text = PlayerPrefs.GetFloat("Camera Sensitivity").ToString("F2");

        GetResolutionOptions();

        changedSettings = false;
    }

    public void ResetSettings()
    {
        PlayerPrefs.SetFloat("Master Volume", 1);
        PlayerPrefs.SetFloat("Music Volume", 1);
        PlayerPrefs.SetFloat("SFX Volume", 1); 
        PlayerPrefs.SetInt("Dog Vision", 0);
        PlayerPrefs.SetInt("Infection Effect", 0);
        PlayerPrefs.SetInt("Remove Gore", 0);
        PlayerPrefs.SetInt("Remove Grain", 0);
        PlayerPrefs.SetInt("Invert Camera Y", 0);
        PlayerPrefs.SetInt("Invert Camera X", 0);
        PlayerPrefs.SetFloat("Camera Sensitivity", 3);

        changedSettings = false;
        ClearChanges();

        GetCurrentSettings();
    }

    public void TryCloseSettings()
    {
        if (changedSettings)
        {
            //Show menu where settings have been changed but not applied
            applySettingsPopup.SetActive(true);
        }
        else
        {
            //Just close the damn menu
            if (menuManager != null)
            {
                menuManager.SettingsBack();
            }
            else
            {
                gameMenuManager.SettingsBack();
            }
        }
    }

    public void ApplySettings()
    {
        PlayerPrefs.SetFloat("Master Volume", newMasterVolume);
        PlayerPrefs.SetFloat("Music Volume", newMusicVolume);
        PlayerPrefs.SetFloat("SFX Volume", newSFXVolume);
        PlayerPrefs.SetInt("Resolution ID", newResolutionIndex);
        PlayerPrefs.SetInt("Fullscreen", newFullscreen ? 1 : 0);
        PlayerPrefs.SetInt("Dog Vision", newDogVision ? 1 : 0);
        PlayerPrefs.SetInt("Infection Effect", newInfectionEffect ? 1 : 0);
        PlayerPrefs.SetInt("Invert Camera Y", newInvertCamY ? 1 : 0);
        PlayerPrefs.SetInt("Invert Camera X", newInvertCamX ? 1 : 0);
        if (newCamSens < 0.1f) { newCamSens = 0.1f; }
        PlayerPrefs.SetFloat("Camera Sensitivity", newCamSens);
        PlayerPrefs.SetInt("Remove Gore", newRemoveGore ? 1 : 0);
        PlayerPrefs.SetInt("Remove Grain", newRemoveGrain ? 1 : 0);

        changedSettings = false;
    }

    public void RevertChanges()
    {
        Screen.fullScreen = !newFullscreen;
        newFullscreen = !newFullscreen;

        Resolution resolution = filteredResolutions[PlayerPrefs.GetInt("Resolution ID")];
        Screen.SetResolution(resolution.width, resolution.height, PlayerPrefs.GetInt("Fullscreen") == 1);

        ClearChanges();
        GetCurrentSettings();
        changedSettings = false;
    }

    private void ClearChanges()
    {
        newMasterVolume = PlayerPrefs.GetFloat("Master Volume");
        newMusicVolume = PlayerPrefs.GetFloat("Music Volume");
        newSFXVolume = PlayerPrefs.GetFloat("SFX Volume");
        newResolutionIndex = PlayerPrefs.GetInt("Resolution ID");
        newFullscreen = PlayerPrefs.GetInt("Fullscreen") == 1;
        newDogVision = PlayerPrefs.GetInt("Dog Vision") == 1;
        newInfectionEffect = PlayerPrefs.GetInt("Infection Effect") == 1;
        newInvertCamY = PlayerPrefs.GetInt("Invert Camera Y") == 1;
        newInvertCamX = PlayerPrefs.GetInt("Invert Camera X") == 1;
        newCamSens = PlayerPrefs.GetFloat("Camera Sensitivity");
        newRemoveGore = PlayerPrefs.GetInt("Remove Gore") == 1;
        newRemoveGrain = PlayerPrefs.GetInt("Remove Grain") == 1;
    }

    #region Volume
    public void SetMasterVolume(float volume)
    {
        newMasterVolume = volume;
        changedSettings = true;
        masterVolumePercent.text = (volume * 100).ToString("F0") + "%";
    }
    public void SetMusicVolume(float volume)
    {
        newMusicVolume = volume;
        changedSettings = true;
        musicVolumePercent.text = (volume * 100).ToString("F0") + "%";
    }
    public void SetSFXVolume(float volume)
    {
        newSFXVolume = volume;
        changedSettings = true;
        SFXVolumePercent.text = (volume * 100).ToString("F0") + "%";
    }
    #endregion

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
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + (int)filteredResolutions[i].refreshRateRatio.value + " Hz";
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                PlayerPrefs.GetInt("Resolution ID", i);
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("Resolution ID");
        resolutionDropdown.RefreshShownValue();
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, PlayerPrefs.GetInt("Fullscreen") == 1);
        newResolutionIndex = resolutionIndex;
        changedSettings = true;
    }

    public void ToggleFullscreen(bool value)
    {
        Screen.fullScreen = value;
        newFullscreen = value;
        changedSettings = true;
    }
    #endregion

    #region Toggles
    public void ToggleDogVision(bool value)
    {
        newDogVision = value;
        changedSettings = true;
    }
    public void ToggleInfectionEffect(bool value)
    {
        newInfectionEffect = value;
        changedSettings = true;
    } 
    public void ToggleGore(bool value)
    {
        newRemoveGore = value;
        changedSettings = true;
    }
    public void ToggleGrain(bool value)
    {
        newRemoveGrain = value;
        changedSettings = true;
    }
    #endregion

    #region Camera Settings
    public void toggleInvertCamY(bool value)
    {
        newInvertCamY = value;
        changedSettings = true;
    }
    public void toggleInvertCamX(bool value)
    {
        newInvertCamX = value;
        changedSettings = true;
    }
    public void SetCameraSensitivity(float sensitivity)
    {
        newCamSens = sensitivity;
        camSensitivityValue.text = sensitivity.ToString("F2");
        changedSettings = true;
    }
    #endregion
}
