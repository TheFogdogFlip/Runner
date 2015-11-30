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

    public Slider soundEffectsVolSlider;
    public Text soundEffectsVolText;

    public Slider musicVolSlider;
    public Text musicVolText;

    public Slider fieldOfViewSlider; 
    public Text fieldOfViewText;

	// Use this for initialization
	void Start() 
    {
        startMenu = startMenu.GetComponent<Canvas>();
        optionsMenu = optionsMenu.GetComponent<Canvas>();
        backText = backText.GetComponent<Button>();
        masterVolSlider = masterVolSlider.GetComponent<Slider>();
        soundEffectsVolSlider = soundEffectsVolSlider.GetComponent<Slider>();
        musicVolSlider = musicVolSlider.GetComponent<Slider>();
        fieldOfViewSlider = fieldOfViewSlider.GetComponent<Slider>();

        
        masterVolText.text = ((int)masterVolSlider.value).ToString();
        soundEffectsVolText.text = ((int)soundEffectsVolSlider.value).ToString();
        musicVolText.text = ((int)musicVolSlider.value).ToString();
        fieldOfViewText.text = ((int)fieldOfViewSlider.value).ToString();
	}

    public void BackPress()
    {
        optionsMenu.enabled = false;
        startMenu.enabled = true;
    }

    public void SetMasterVolume()
    {
        masterVolText.text = ((int)masterVolSlider.value).ToString();
    }

    public void SetSoundEffectsVolume()
    {
        soundEffectsVolText.text = ((int)soundEffectsVolSlider.value).ToString();
    }

    public void SetMusicVolume()
    {
        musicVolText.text = ((int)musicVolSlider.value).ToString();
    }

    public void SetFieldOfView()
    {
        fieldOfViewText.text = ((int)fieldOfViewSlider.value).ToString();
    }

}
