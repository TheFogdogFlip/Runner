using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class OptionsMenu : MonoBehaviour 
{
    private Canvas startMenu;
    private Canvas optionsMenu;
    
    private Button backText;
    
    private Slider masterVolSlider;
    public Text masterVolText;
    
    private Slider effectsVolSlider;
    public Text effectsVolText;
    
    private Slider musicVolSlider;
    public Text musicVolText;
    
    private StartMenu startMenuObj;
    private OptionsMenu optionsMenuObj;

    private GameObject backGameObj;
    private GameObject masterVolGameObj;
    private GameObject effectsVolGameObj;
    private GameObject musicVolGameObj;
    private GameObject startMenuGameObj;
    private GameObject optionsMenuGameObj;


    public EventSystem eventSys;

    /**---------------------------------------------------------------------------------
     * 
     */
	void 
    Start() 
    {
        //Empty
	}

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    LoadComponents()
    {
        GlobalGameSettings.LoadSettings();
        startMenuGameObj            = GameObject.Find("START_MENU_CANVAS");
        optionsMenuGameObj          = GameObject.Find("OPTIONS_MENU_CANVAS");
        backGameObj                 = GameObject.Find("OPTIONS_BACK_TEXT");
        masterVolGameObj            = GameObject.Find("SOUND_MASTER_SLIDER");
        musicVolGameObj             = GameObject.Find("SOUND_MUSIC_SLIDER");
        effectsVolGameObj           = GameObject.Find("SOUND_EFFECTS_SLIDER");

        startMenu                   = startMenuGameObj.GetComponent<Canvas>();
        optionsMenu                 = optionsMenuGameObj.GetComponent<Canvas>();

        startMenuObj                = startMenuGameObj.GetComponent<StartMenu>();
        optionsMenuObj              = optionsMenuGameObj.GetComponent<OptionsMenu>();

        backText                    = backGameObj.GetComponent<Button>();
        masterVolSlider             = masterVolGameObj.GetComponent<Slider>();
        effectsVolSlider            = effectsVolGameObj.GetComponent<Slider>();
        musicVolSlider              = musicVolGameObj.GetComponent<Slider>();

        int masterVol               = GlobalGameSettings.GetMasterVolume();
        int effectsVol              = GlobalGameSettings.GetEffectsVolume();
        int musicVol                = GlobalGameSettings.GetMusicVolume();

        masterVolSlider.value       = masterVol;
        effectsVolSlider.value      = effectsVol;
        musicVolSlider.value        = musicVol;

        masterVolText.text          = masterVol.ToString();
        effectsVolText.text         = effectsVol.ToString();
        musicVolText.text           = musicVol.ToString();
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    SetSettings()
    {
        GlobalGameSettings.SetMasterVolume(40);
        GlobalGameSettings.SetEffectsVolume(30);
        GlobalGameSettings.SetMusicVolume(20);
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    BackPress()
    {
        optionsMenu.enabled = false;
        startMenu.enabled = true;
        DisableOptions();
        startMenuObj.EnableStart();
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    SetMasterVolume()
    {
        int val = (int)masterVolSlider.value;
        masterVolText.text = val.ToString();
        GlobalGameSettings.SetMasterVolume(val);
        GlobalGameSettings.SaveSettings();
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    SetSoundEffectsVolume()
    {
        int val = (int)effectsVolSlider.value;
        effectsVolText.text = val.ToString();
        GlobalGameSettings.SetEffectsVolume(val);
        GlobalGameSettings.SaveSettings();
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    SetMusicVolume()
    {
        int val = (int)musicVolSlider.value;
        musicVolText.text = val.ToString();
        GlobalGameSettings.SetMusicVolume(val);
        GlobalGameSettings.SaveSettings();
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    DisableOptions()
    {
        backText.enabled = false;
        masterVolSlider.enabled = false;
        musicVolSlider.enabled = false;
        effectsVolSlider.enabled = false;
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    EnableOptions()
    {
        backText.enabled = true;
        masterVolSlider.enabled = true;
        musicVolSlider.enabled = true;
        effectsVolSlider.enabled = true;
        eventSys.SetSelectedGameObject(backGameObj);
    }
}
