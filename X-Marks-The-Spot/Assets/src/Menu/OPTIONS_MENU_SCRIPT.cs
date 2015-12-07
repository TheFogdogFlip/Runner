using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OPTIONS_MENU_SCRIPT : MonoBehaviour 
{
    public Canvas startMenu;
    public Canvas optionsMenu;

    public Button backText;

    public Slider masterVolSlider;
    public Text masterVolText;

    public Slider effectsVolSlider;
    public Text effectsVolText;

    public Slider musicVolSlider;
    public Text musicVolText;

    public START_MENU_SCRIPT startMenuObj;
    public OPTIONS_MENU_SCRIPT optionsMenuObj;

	void Start() 
    {
        startMenuObj = GameObject.Find("START_MENU_CANVAS").GetComponent<START_MENU_SCRIPT>();
        optionsMenuObj = GameObject.Find("OPTIONS_MENU_CANVAS").GetComponent<OPTIONS_MENU_SCRIPT>();

        GlobalGameSettings.LoadSettings();
        startMenu               = startMenu.GetComponent<Canvas>();
        optionsMenu             = optionsMenu.GetComponent<Canvas>();

        backText                = backText.GetComponent<Button>();

        masterVolSlider         = masterVolSlider.GetComponent<Slider>();
        effectsVolSlider        = effectsVolSlider.GetComponent<Slider>();
        musicVolSlider          = musicVolSlider.GetComponent<Slider>();

        int masterVol           = GlobalGameSettings.GetMasterVolume();
        int effectsVol          = GlobalGameSettings.GetEffectsVolume();
        int musicVol            = GlobalGameSettings.GetMusicVolume();
        
        masterVolSlider.value   = masterVol;
        effectsVolSlider.value  = effectsVol;
        musicVolSlider.value    = musicVol;

        masterVolText.text      = masterVol.ToString();
        effectsVolText.text     = effectsVol.ToString();
        musicVolText.text       = musicVol.ToString();
	}

    public void SetSettings()
    {
        GlobalGameSettings.SetMasterVolume(40);
        GlobalGameSettings.SetEffectsVolume(30);
        GlobalGameSettings.SetMusicVolume(20);
    }

    public void BackPress()
    {
        optionsMenu.enabled = false;
        startMenu.enabled = true;
        DisableOptions();
        startMenuObj.EnableStart();
    }

    public void SetMasterVolume()
    {
        int val = (int)masterVolSlider.value;
        masterVolText.text = val.ToString();
        GlobalGameSettings.SetMasterVolume(val);
        GlobalGameSettings.SaveSettings();
    }

    public void SetSoundEffectsVolume()
    {
        int val = (int)effectsVolSlider.value;
        effectsVolText.text = val.ToString();
        GlobalGameSettings.SetEffectsVolume(val);
        GlobalGameSettings.SaveSettings();
    }

    public void SetMusicVolume()
    {
        int val = (int)musicVolSlider.value;
        musicVolText.text = val.ToString();
        GlobalGameSettings.SetMusicVolume(val);
        GlobalGameSettings.SaveSettings();
    }

    public void DisableOptions()
    {
        backText.enabled = false;
        masterVolSlider.enabled = false;
        musicVolSlider.enabled = false;
        effectsVolSlider.enabled = false;
    }

    public void EnableOptions()
    {
        backText.enabled = true;
        masterVolSlider.enabled = true;
        musicVolSlider.enabled = true;
        effectsVolSlider.enabled = true;
    }
}
