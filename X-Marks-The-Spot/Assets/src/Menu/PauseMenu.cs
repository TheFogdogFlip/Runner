using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

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

	void Start () 
    {
        LoadComponents();
        DisablePauseMenu();
        DisableAudioSettings();
	}

    public void LoadComponents()
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
        
    }

	void Update () 
    {
	    if (Input.GetButtonDown("Cancel"))
        {
            Time.timeScale = 0;
            EnablePauseMenu();
            eventSys.SetSelectedGameObject(resumeGameObject);            
        }
	}

    public void ExitGamePress()
    {
        Application.LoadLevel("Main_Menu");
    }

    public void ResumeGamePress()
    {
        DisablePauseMenu();

        Time.timeScale = 1;
    }
    
    public void SaveMapPress()
    {
        World.Instance.Save("SavedMap.png");
    }

    public void AudioSettingsPress()
    {
        DisablePauseMenu();
        EnableAudioSettings();
        eventSys.SetSelectedGameObject(backGameObject);
    }

    public void BackPress()
    {
        DisableAudioSettings();
        EnablePauseMenu();
        eventSys.SetSelectedGameObject(resumeGameObject);
    }

    public void EnablePauseMenu()
    {
        resumeGame.enabled = true;
        audioSettings.enabled = true;
        saveMapText.enabled = true;
        exitGame.enabled = true;
        pauseMenu.enabled = true;
    }

    public void EnableAudioSettings()
    {
        masterVolSlider.enabled = true;
        effectsVolSlider.enabled = true;
        musicVolSlider.enabled = true;
        backText.enabled = true;
        audioMenu.enabled = true;
    }

    public void DisablePauseMenu()
    {
        resumeGame.enabled = false;
        audioSettings.enabled = false;
        saveMapText.enabled = false;
        exitGame.enabled = false;
        pauseMenu.enabled = false;
    }

    public void DisableAudioSettings()
    {
        masterVolSlider.enabled = false;
        effectsVolSlider.enabled = false;
        musicVolSlider.enabled = false;
        backText.enabled = false;
        audioMenu.enabled = false;
    }


}
