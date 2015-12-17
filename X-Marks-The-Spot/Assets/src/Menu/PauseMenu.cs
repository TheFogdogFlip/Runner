using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.IO;

public class PauseMenu : MonoBehaviour 
{
    /**---------------------------------------------------------------------------------
     * GameObjects associated with the script.
     */
    private GameObject pauseMenuGameObject;
    private GameObject audioMenuGameObject;

    private GameObject exitGameObject;
    private GameObject resumeGameObject;
    private GameObject audioGameObject;
    private GameObject saveMapGameObject;

    private GameObject backGameObject;
    private GameObject masterVolGameObject;
    private GameObject effectsVolGameObject;
    private GameObject musicVolGameObject;

    /**---------------------------------------------------------------------------------
     * Canvas associated with the script.
     */
    private Canvas pauseMenu;
    private Canvas audioMenu;

    /**---------------------------------------------------------------------------------
     * Buttons associated with the script.
     */
    private Button exitGame;
    private Button resumeGame;
    private Button audioSettings;
    private Button saveMapText;
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
     * The EventSystem used by the script.
     */
    public EventSystem eventSys;

    /**---------------------------------------------------------------------------------
     * Variables used by the script.
     */
    private bool gameIsPaused;
    private bool keyboardActive;
    private bool gamepadActive;
    private StandaloneInputModule[] inputModuleArray;
    /**---------------------------------------------------------------------------------
     * Should only be executed once.
     * Executed automatically on the startup of the script.
     */
	void 
    Start () 
    {
        Time.timeScale = 1;
        LoadComponents();
        DisablePauseMenu();
        DisableAudioSettings();
	}

    /**---------------------------------------------------------------------------------
     * Should only be executed once.
     * Loads components necessary for the script. 
     * Changing the name of a GameObject in the scene will require changing the string in the respective GameObject.Find() call.
     * Loads audio settings from GlobalGameSettings.
     */
    public void 
    LoadComponents()
    {
        gamepadActive = false;
        keyboardActive = false;
        inputModuleArray = eventSys.GetComponents<StandaloneInputModule>();
        inputModuleArray[0].enabled = false;     //Keyboard
        inputModuleArray[1].enabled = false;     //Gamepad

        pauseMenuGameObject             = GameObject.Find("PauseMenu");
        audioMenuGameObject             = GameObject.Find("AudioSettings");
        exitGameObject                  = GameObject.Find("ExitGame_TextBtn");
        resumeGameObject                = GameObject.Find("ResumeGame_TextBtn");
        audioGameObject                 = GameObject.Find("AudioSettings_TextBtn");
        saveMapGameObject               = GameObject.Find("SaveMap_TextBtn");

        backGameObject                  = GameObject.Find("SettingsBack_TextBtn");
        masterVolGameObject             = GameObject.Find("SoundMaster_Slider");
        masterVolText                   = GameObject.Find("SoundMasterCounter_Text").GetComponent<Text>();
        musicVolGameObject              = GameObject.Find("SoundMusic_Slider");
        musicVolText                    = GameObject.Find("SoundMusicCounter_Text").GetComponent<Text>();
        effectsVolGameObject            = GameObject.Find("SoundEffects_Slider");
        effectsVolText                  = GameObject.Find("SoundEffectsCounter_Text").GetComponent<Text>();
        

        pauseMenu                       = pauseMenuGameObject.GetComponent<Canvas>();
        audioMenu                       = audioMenuGameObject.GetComponent<Canvas>();

        exitGame                        = exitGameObject.GetComponent<Button>();
        resumeGame                      = resumeGameObject.GetComponent<Button>();
        audioSettings                   = audioGameObject.GetComponent<Button>();
        saveMapText                     = saveMapGameObject.GetComponent<Button>();

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
     * Executed every frame.
     * Checks if CancelButton is down, in which case it enables the pause menu.
     */
	void 
    Update () 
    {
        if (gameIsPaused && eventSys.currentSelectedGameObject == null)
        {
            eventSys.SetSelectedGameObject(resumeGameObject);
        }

        if (gameIsPaused && !keyboardActive && CheckForKeyboardInput())     //Setting keyboard as active input module
        {
            inputModuleArray[0].enabled = true;
            inputModuleArray[1].enabled = false;
            keyboardActive = true;
            gamepadActive = false;
        }

        if (gameIsPaused && !gamepadActive && CheckForGamepadInput())       //Setting gamepad as active input module
        {
            inputModuleArray[0].enabled = false;
            inputModuleArray[1].enabled = true;
            gamepadActive = true;
            keyboardActive = false;
        }
        if ((Input.GetButtonDown("Cancel_Menu_kb") || Input.GetButtonDown("Cancel_Menu_gp")) && !gameIsPaused)
        {
            gameIsPaused = true;
            Time.timeScale = 0;
            EnablePauseMenu();
            eventSys.SetSelectedGameObject(resumeGameObject);
            AudioManager.Instance.pauseVolume();
        }
        else if ((Input.GetButtonDown("Cancel_Menu_kb") || Input.GetButtonDown("Cancel_Menu_gp")) && gameIsPaused)
        {
            gameIsPaused = false;
            Time.timeScale = 1;
            DisablePauseMenu();
            AudioManager.Instance.UnPauseVolume();
        }
	}

    /**---------------------------------------------------------------------------------
    * Checks for keyboard input.
    */
    private bool
    CheckForKeyboardInput()
    {
        return (Input.GetButtonDown("Submit_Menu_kb") || Input.GetButtonDown("Cancel_Menu_kb") || Input.GetAxisRaw("Horizontal_Menu_kb") == 1 || Input.GetAxisRaw("Horizontal_Menu_kb") == -1 || Input.GetAxisRaw("Vertical_Menu_kb") == 1 || Input.GetAxisRaw("Vertical_Menu_kb") == -1);
    }

    /**---------------------------------------------------------------------------------
    * Checks for gamepad input.
    */
    private bool
    CheckForGamepadInput()
    {
        return (Input.GetButtonDown("Submit_Menu_gp") || Input.GetButtonDown("Cancel_Menu_gp") || Input.GetAxisRaw("Horizontal_Menu_gp") == 1 || Input.GetAxisRaw("Horizontal_Menu_gp") == -1 || Input.GetAxisRaw("Vertical_Menu_gp") == 1 || Input.GetAxisRaw("Vertical_Menu_gp") == -1);
    }

    /**---------------------------------------------------------------------------------
     * Executes when the mouse pointer is over a UI button.
     * Highlights the specific GameObject for EventSystem.
     */
    public void 
    HightlightItem(GameObject gameObj)
    {
        eventSys.SetSelectedGameObject(gameObj);
    }

    /**---------------------------------------------------------------------------------
     * Executes when the Reload Game button is pressed. 
     * Reloads the game by loading the scene called "Scene".
     */
    public void 
    ReloadGamePress()
    {
        Time.timeScale = 1;
        Application.LoadLevel("Scene");
        AudioManager.Instance.UnPauseVolume();
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Exit Game buttons i pressed.
     * Loads scene called "Main_Menu".
     */
    public void 
    ExitGamePress()
    {
        Time.timeScale = 1;
        Application.LoadLevel("Main_Menu");
        AudioManager.Instance.UnPauseVolume();
    }

    /**---------------------------------------------------------------------------------
     * Executed when Resume Game button is pressed. 
     * Disables the pause menu and sets timeScale to 1.
     */
    public void 
    ResumeGamePress()
    {
        DisablePauseMenu();
        gameIsPaused = false;
        Time.timeScale = 1;
        AudioManager.Instance.UnPauseVolume();
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Save Map button is pressed.
     * Saves the current map to current directory as a .png file.
     */
    public void 
    SaveMapPress()
    {
        //check all files MAPSAVE*.PNG in current directory
        string path = Directory.GetCurrentDirectory();
        //DirectoryInfo dir = new DirectoryInfo(path);
        string[] info = Directory.GetFiles(path, "MAPSAVE*.PNG");

        //save the current map as the last one in the list
        World.Instance.Save("MAPSAVE" +  info.Length + ".PNG");

        //Another way to do it that might be better in the long run
        //Save it to C:\Users\user\AppData\LocalLow\DefaultCompany instead

        //check all files MAPSAVE*.PNG in current directory
        //string path = Application.persistentDataPath;

        //DirectoryInfo dir = new DirectoryInfo(path);
        //string[] info = Directory.GetFiles(path, "MAPSAVE*.PNG");

        //save the current map as the last one in the list
        //World.Instance.Save("MAPSAVE" +  info.Length + ".PNG");
        //World.Instance.Save(path + "MAPSAVE" + info.Length + ".PNG");
  
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Audio Settings button is pressed. 
     * Disables pause menu and enables audio settings menu, then highlights the Back button.
     */
    public void 
    AudioSettingsPress()
    {
        DisablePauseMenu();
        EnableAudioSettings();
        eventSys.SetSelectedGameObject(backGameObject);
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
     * Executed when the AudioSettingsBack button is pressed. 
     * Disables the audio settings menu and enables the pause menu, then highlights the Resume Game button
     */
    public void 
    BackPress()
    {
        DisableAudioSettings();
        EnablePauseMenu();
        eventSys.SetSelectedGameObject(resumeGameObject);
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Pause menu needs to be enabled.
     * Enables the canvas and all the buttons in the canvas.
     */
    public void 
    EnablePauseMenu()
    {
        resumeGame.enabled = true;
        audioSettings.enabled = true;
        saveMapText.enabled = true;
        exitGame.enabled = true;
        pauseMenu.enabled = true;
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Audio Settings menu needs to be enabled.
     * Enables the canvas and all the buttons in the canvas.
     */
    public void 
    EnableAudioSettings()
    {
        masterVolSlider.enabled = true;
        effectsVolSlider.enabled = true;
        musicVolSlider.enabled = true;
        backText.enabled = true;
        audioMenu.enabled = true;
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Pause menu needs to be disabled.
     * Disables the canvas and all the buttons in the canvas.
     */
    public void 
    DisablePauseMenu()
    {
        resumeGame.enabled = false;
        audioSettings.enabled = false;
        saveMapText.enabled = false;
        exitGame.enabled = false;
        pauseMenu.enabled = false;
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Audio Settings menu needs to be disabled.
     * Disables the canvas and all the buttons in the canvas.
     */
    public void 
    DisableAudioSettings()
    {
        masterVolSlider.enabled = false;
        effectsVolSlider.enabled = false;
        musicVolSlider.enabled = false;
        backText.enabled = false;
        audioMenu.enabled = false;
    }
}
