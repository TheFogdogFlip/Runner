using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class OptionsMenu : MonoBehaviour
{
    /**---------------------------------------------------------------------------------
     * GameObjects associated with the script.
     */
    private GameObject backGameObject;
    private GameObject masterVolGameObject;
    private GameObject effectsVolGameObject;
    private GameObject musicVolGameObject;
    private GameObject startMenuGameObject;
    private GameObject optionsMenuGameObject;

    /**---------------------------------------------------------------------------------
     * Canvas associated with the script.
     */
    private Canvas startMenu;
    private Canvas optionsMenu;

    /**---------------------------------------------------------------------------------
     * Buttons associated with the script.
     */
    private Button backText;

    /**---------------------------------------------------------------------------------
     * Master volume slider used in Audio Settings.
     */
    private Slider masterVolSlider;
    private Text masterVolText;

    /**---------------------------------------------------------------------------------
     * Sound effects volume slider used in Audio Settings.
     */
    private Slider effectsVolSlider;
    private Text effectsVolText;

    /**---------------------------------------------------------------------------------
     * Music volume slider used in Audio Settings.
     */
    private Slider musicVolSlider;
    private Text musicVolText;

    /**---------------------------------------------------------------------------------
     * Class objects used by the script.
     */
    private StartMenu startMenuObj;
    private OptionsMenu optionsMenuObj;

    /**---------------------------------------------------------------------------------
     * EventSystem associated with the script.
     */
    public EventSystem eventSys;

    /**---------------------------------------------------------------------------------
     * Executed when the script starts.
     */
    void 
    Start() 
    {
        //Empty
	}

    /**---------------------------------------------------------------------------------
     * Should only be executed once.
     * Loads all components necessary for the script.
     * Changing the name of a GameObject in the scene will require changing the string in the respective GameObject.Find() call.
     * Loads audio settings from GlobalGameSettings.
     */
    public void 
    LoadComponents()
    {
        GlobalGameSettings.LoadSettings();
        startMenuGameObject             = GameObject.Find("StartMenu_Canvas");
        optionsMenuGameObject           = GameObject.Find("OptionsMenu_Canvas");
        backGameObject                  = GameObject.Find("OptionsBack_TextBtn");
        masterVolGameObject             = GameObject.Find("SoundMaster_Slider");
        masterVolText                   = GameObject.Find("SoundMasterCounter_Text").GetComponent<Text>();
        musicVolGameObject              = GameObject.Find("SoundMusic_Slider");
        musicVolText                    = GameObject.Find("SoundMusicCounter_Text").GetComponent<Text>();
        effectsVolGameObject            = GameObject.Find("SoundEffects_Slider");
        effectsVolText                  = GameObject.Find("SoundEffectsCounter_Text").GetComponent<Text>();

        startMenu                       = startMenuGameObject.GetComponent<Canvas>();
        optionsMenu                     = optionsMenuGameObject.GetComponent<Canvas>();

        startMenuObj                    = startMenuGameObject.GetComponent<StartMenu>();
        optionsMenuObj                  = optionsMenuGameObject.GetComponent<OptionsMenu>();

        backText                        = backGameObject.GetComponent<Button>();
        masterVolSlider                 = masterVolGameObject.GetComponent<Slider>();
        effectsVolSlider                = effectsVolGameObject.GetComponent<Slider>();
        musicVolSlider                  = musicVolGameObject.GetComponent<Slider>();

        int masterVol                   = GlobalGameSettings.GetMasterVolume();
        int effectsVol                  = GlobalGameSettings.GetEffectsVolume();
        int musicVol                    = GlobalGameSettings.GetMusicVolume();

        masterVolSlider.value           = masterVol;
        effectsVolSlider.value          = effectsVol;
        musicVolSlider.value            = musicVol;

        masterVolText.text              = masterVol.ToString();
        effectsVolText.text             = effectsVol.ToString();
        musicVolText.text               = musicVol.ToString();
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Options back button is pressed. 
     * Disables the options menu and enables the start menu.
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
     * Executed when the Sound Master volume slider is moved. 
     * Sets the master volume in GlobalGameSettings and calls GlobalGameSettings.SaveSettings.
     */
    public void 
    SetMasterVolume()
    {
        int val = (int)masterVolSlider.value;
        masterVolText.text = val.ToString();
        GlobalGameSettings.SetMasterVolume(val);
        GlobalGameSettings.SaveSettings();
        AudioManager.Instance.SetMasterVolume(val);
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Sound Effects volume slider is moved. 
     * Sets the sound effects volume in GlobalGameSettings and calls GlobalGameSettings.SaveSettings.
     */
    public void 
    SetSoundEffectsVolume()
    {
        int val = (int)effectsVolSlider.value;
        effectsVolText.text = val.ToString();
        GlobalGameSettings.SetEffectsVolume(val);
        GlobalGameSettings.SaveSettings();
        AudioManager.Instance.SetSoundFXVolume(val);
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Sound Music volume slider is moved. 
     * Sets the music volume in GlobalGameSettings and calls GlobalGameSettings.SaveSettings.
     */
    public void 
    SetMusicVolume()
    {
        int val = (int)musicVolSlider.value;
        musicVolText.text = val.ToString();
        GlobalGameSettings.SetMusicVolume(val);
        GlobalGameSettings.SaveSettings();
        AudioManager.Instance.SetMusicVolume(val);
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Options menu needs to be enabled.
     * Enables the canvas and all the components associated with the script.
     */
    public void
    EnableOptions()
    {
        backText.enabled = true;
        masterVolSlider.enabled = true;
        musicVolSlider.enabled = true;
        effectsVolSlider.enabled = true;
        eventSys.SetSelectedGameObject(backGameObject);
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Options menu needs to be disabled.
     * Disables the canvas and all the components associated with the script.
     */
    public void 
    DisableOptions()
    {
        backText.enabled = false;
        masterVolSlider.enabled = false;
        musicVolSlider.enabled = false;
        effectsVolSlider.enabled = false;
    }
}
