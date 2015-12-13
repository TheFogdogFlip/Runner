using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.IO;

public class PauseMenu : MonoBehaviour 
{
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

    private Canvas pauseMenu;
    private Canvas audioMenu;

    private Button exitGame;
    private Button resumeGame;
    private Button audioSettings;
    private Button saveMapText;

    private Slider masterVolSlider;
    public Text masterVolText;

    private Slider effectsVolSlider;
    public Text effectsVolText;

    private Slider musicVolSlider;
    public Text musicVolText;

    private Button backText;
    
    public EventSystem eventSys;


    /**---------------------------------------------------------------------------------
     * 
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
     * 
     */
    public void 
    LoadComponents()
    {
        pauseMenuGameObject = GameObject.Find("PauseMenu");
        audioMenuGameObject = GameObject.Find("AudioSettings");
        exitGameObject = GameObject.Find("ExitGame_Text");
        resumeGameObject = GameObject.Find("ResumeGame_Text");
        audioGameObject = GameObject.Find("AudioSettings_Text");
        saveMapGameObject = GameObject.Find("SaveMap_Text");

        backGameObject = GameObject.Find("SETTINGS_BACK_TEXT");
        masterVolGameObject = GameObject.Find("SOUND_MASTER_SLIDER");
        musicVolGameObject = GameObject.Find("SOUND_MUSIC_SLIDER");
        effectsVolGameObject = GameObject.Find("SOUND_EFFECTS_SLIDER");
        

        pauseMenu = pauseMenuGameObject.GetComponent<Canvas>();
        audioMenu = audioMenuGameObject.GetComponent<Canvas>();

        exitGame = exitGameObject.GetComponent<Button>();
        resumeGame = resumeGameObject.GetComponent<Button>();
        audioSettings = audioGameObject.GetComponent<Button>();
        saveMapText = saveMapGameObject.GetComponent<Button>();

        backText = backGameObject.GetComponent<Button>();
        masterVolSlider = masterVolGameObject.GetComponent<Slider>();
        effectsVolSlider = effectsVolGameObject.GetComponent<Slider>();
        musicVolSlider = musicVolGameObject.GetComponent<Slider>();

        int masterVol = GlobalGameSettings.GetMasterVolume();
        int effectsVol = GlobalGameSettings.GetEffectsVolume();
        int musicVol = GlobalGameSettings.GetMusicVolume();

        masterVolSlider.value = masterVol;
        effectsVolSlider.value = effectsVol;
        musicVolSlider.value = musicVol;

        masterVolText.text = masterVol.ToString();
        effectsVolText.text = effectsVol.ToString();
        musicVolText.text = musicVol.ToString();        

    }

    /**---------------------------------------------------------------------------------
     * 
     */
	void 
    Update () 
    {
	    if (Input.GetButtonDown("Cancel"))
        {
            Time.timeScale = 0;
            EnablePauseMenu();
            eventSys.SetSelectedGameObject(resumeGameObject);
            //AudioManager.Instance.pauseVolume();
        }
	}

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    HightlightItem(GameObject gameObj)
    {
        eventSys.SetSelectedGameObject(gameObj);
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    ReloadGamePress()
    {
        Time.timeScale = 1;
        Application.LoadLevel("Scene");
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    ExitGamePress()
    {
        Time.timeScale = 1;
        Application.LoadLevel("Main_Menu");
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    ResumeGamePress()
    {
        DisablePauseMenu();

        Time.timeScale = 1;
        //AudioManager.Instance.UnPauseVolume();
    }

    /**---------------------------------------------------------------------------------
     * 
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
     * 
     */
    private void 
    GetCurrentDirectory()
    {
        throw new System.NotImplementedException();
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    AudioSettingsPress()
    {
        DisablePauseMenu();
        EnableAudioSettings();
        eventSys.SetSelectedGameObject(backGameObject);
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
    BackPress()
    {
        DisableAudioSettings();
        EnablePauseMenu();
        eventSys.SetSelectedGameObject(resumeGameObject);
    }

    /**---------------------------------------------------------------------------------
     * 
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
     * 
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
     * 
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
     * 
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
