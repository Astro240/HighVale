using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuOptionsManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject optionsPanel;
    public GameObject pausePanel;

    [Header("Settings Controls")]
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Slider volumeSlider;

    private Resolution[] resolutions;

    void Start()
    {
        // Load all screen resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

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

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Set UI initial states
        fullscreenToggle.isOn = Screen.fullScreen;
        volumeSlider.value = AudioListener.volume;
    }

    void Update()
    {
        // Close options with ESC if it's currently open
        if (optionsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseOptions();
        }
    }

    // Called by Resolution dropdown OnValueChanged
    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    // Called by Fullscreen toggle
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // Called by Volume slider
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    // Called by "Options" button in Pause Panel
    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    // Called by "Back" button in Options Panel or ESC
    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
}