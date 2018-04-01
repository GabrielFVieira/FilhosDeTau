using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    private GameManager manager;

    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Dropdown resolutionsDropDown;

    private Resolution[] resolutions;

    [SerializeField]
    private Toggle fullscreenToggle;

    private int iter = 0;
	// Use this for initialization
	void Start () {
        if (Screen.fullScreen)
            fullscreenToggle.isOn = true;

        else
            fullscreenToggle.isOn = false;

        resolutions = Screen.resolutions;

        resolutionsDropDown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;

            if (options.Contains(option.ToLower()))
                continue;

            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = iter;

            iter++;
        }

        resolutionsDropDown.AddOptions(options);
        resolutionsDropDown.value = currentResolutionIndex;
        resolutionsDropDown.RefreshShownValue();
	}
	
	// Update is called once per frame
	void Update () {
        if (manager == null)
            manager = GameObject.Find("MenuManager").GetComponent<GameManager>();
	}

    public void LoadSave()
    {
        manager.LoadGame();
    }

    public void NewSave()
    {
        manager.NewGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        var width = int.Parse(resolutionsDropDown.options[resolutionIndex].text.Split(' ')[0]);
        var height = int.Parse(resolutionsDropDown.options[resolutionIndex].text.Split(' ')[2]);

        Screen.SetResolution(width, height, Screen.fullScreen);
    }
}
