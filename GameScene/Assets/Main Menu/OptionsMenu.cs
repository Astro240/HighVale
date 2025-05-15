using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public GameObject OptionsPanel;
    public TMP_Dropdown ResolutionDropDown;
    public Toggle FullScreenToogle;
    public Slider VolumeSlider;

    Resolution[] resolutions;

    void Start()
    {
        // Get all available screen resolutions
        resolutions = Screen.resolutions;
        ResolutionDropDown.ClearOptions();

        int currentResolutionIndex = 0;
        var options = new System.Collections.Generic.List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        ResolutionDropDown.AddOptions(options);
        ResolutionDropDown.value = currentResolutionIndex;
        ResolutionDropDown.RefreshShownValue();

        // Set initial values
        FullScreenToogle.isOn = Screen.fullScreen;
        VolumeSlider.value = AudioListener.volume;
    }

    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void OpenOptions()
    {
        OptionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        OptionsPanel.SetActive(false);
    }
}
