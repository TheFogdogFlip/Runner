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

    public Slider fieldOfViewSlider; 
    public Text fieldOfViewText;

	// Use this for initialization
	void Start() 
    {
        SetSettings();
        startMenu               = startMenu.GetComponent<Canvas>();
        optionsMenu             = optionsMenu.GetComponent<Canvas>();

        backText                = backText.GetComponent<Button>();

        masterVolSlider         = masterVolSlider.GetComponent<Slider>();
        effectsVolSlider        = effectsVolSlider.GetComponent<Slider>();
        musicVolSlider          = musicVolSlider.GetComponent<Slider>();
        fieldOfViewSlider       = fieldOfViewSlider.GetComponent<Slider>();

        int masterVol           = GlobalGameSettings.GetMasterVolume();
        int effectsVol          = GlobalGameSettings.GetEffectsVolume();
        int musicVol            = GlobalGameSettings.GetMusicVolume();
        int fov                 = GlobalGameSettings.GetFieldOfView();
        
        masterVolSlider.value   = masterVol;
        effectsVolSlider.value  = effectsVol;
        musicVolSlider.value    = musicVol;
        fieldOfViewSlider.value = fov;

        masterVolText.text      = masterVol.ToString();
        effectsVolText.text     = effectsVol.ToString();
        musicVolText.text       = musicVol.ToString();
        fieldOfViewText.text    = fov.ToString();
	}

    public void SetSettings()
    {
        GlobalGameSettings.SetMasterVolume(40);
        GlobalGameSettings.SetEffectsVolume(30);
        GlobalGameSettings.SetMusicVolume(20);
        GlobalGameSettings.SetFieldOfView(10);
    }

    public void BackPress()
    {
        optionsMenu.enabled = false;
        startMenu.enabled = true;
    }

    public void SetMasterVolume()
    {
        int val = (int)masterVolSlider.value;
        masterVolText.text = val.ToString();
        GlobalGameSettings.SetMasterVolume(val);
    }

    public void SetSoundEffectsVolume()
    {
        int val = (int)effectsVolSlider.value;
        effectsVolText.text = val.ToString();
        GlobalGameSettings.SetEffectsVolume(val);
    }

    public void SetMusicVolume()
    {
        int val = (int)musicVolSlider.value;
        musicVolText.text = val.ToString();
        GlobalGameSettings.SetMusicVolume(val);
    }

    public void SetFieldOfView()
    {
        int val = (int)fieldOfViewSlider.value;
        fieldOfViewText.text = val.ToString();
        GlobalGameSettings.SetFieldOfView(val);
    }

}
