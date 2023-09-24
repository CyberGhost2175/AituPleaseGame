using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public Dropdown resolDropdown;
    Resolution[] resolutions;
    public Slider sensitivitySlider;
    public float minSensitivity = 1f;
    public float maxSensitivity = 10f;



    void Start()
    {
        resolDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " " +
                resolutions[i].refreshRateRatio + "Hz";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {

                currResolutionIndex = i;
            }
        }
        resolDropdown.AddOptions(options);
        resolDropdown.RefreshShownValue();
        sensitivitySlider.value = GetCurrentSensitivity();
        LoadSettings(currResolutionIndex);



    }
    private void Update()
    {

        void Update()
        {
            float currentSensitivity = GetCurrentSensitivity();

            float moveSpeed = 5f * currentSensitivity;
            float rotationSpeed = 100f * currentSensitivity;

        }
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    public void SetResolution(int resolIndex)
    {
        Resolution resolution = resolutions[resolIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResoutionPreference", resolDropdown.value);
        PlayerPrefs.SetInt("FullScreenPreference", System.Convert.ToInt32(Screen.fullScreen));
    }

    public void LoadSettings(int currResolIndex)
    {
        if (PlayerPrefs.HasKey("ResoutionPreference"))
        {

            resolDropdown.value = PlayerPrefs.GetInt("ResoutionPreference");
        }
        else
        {
            resolDropdown.value = currResolIndex;
        }
        if (PlayerPrefs.HasKey("FullScreenPreference"))
        {

            Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullScreenPreference"));
        }
        else { 
        Screen.fullScreen = true;
        }
    }

    public float GetCurrentSensitivity()
    {
        float sensitivity = Mathf.Lerp(minSensitivity, maxSensitivity, sensitivitySlider.value);
        return sensitivity;
    }
}
