using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    public Canvas pauseMenu;
    public Button exitGame;
    public Button resumeGame;
    public Button audioSettings;

	// Use this for initialization
	void Start () 
    {
        pauseMenu.enabled = false;
        
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (Input.GetButtonDown("Cancel"))
        {
            pauseMenu.enabled = true;
        }
	}

    public void ExitGamePress()
    {
        
        Application.LoadLevel("Main_Menu");
    }

    public void ResumeGamePress()
    {
        pauseMenu.enabled = false;
    }

    public void AudioSettingsPress()
    {

    }


}
