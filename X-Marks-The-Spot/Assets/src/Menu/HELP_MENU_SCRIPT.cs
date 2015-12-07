using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HELP_MENU_SCRIPT : MonoBehaviour {

    public Canvas startMenu;
    public Canvas helpMenu;

    public Button backText;

    public START_MENU_SCRIPT startMenuObj;
    public HELP_MENU_SCRIPT helpMenuObj;

	void Start() 
    {
        startMenuObj = GameObject.Find("START_MENU_CANVAS").GetComponent<START_MENU_SCRIPT>();
        helpMenuObj = GameObject.Find("HELP_MENU_CANVAS").GetComponent<HELP_MENU_SCRIPT>();

        startMenu = startMenu.GetComponent<Canvas>();
        helpMenu = helpMenu.GetComponent<Canvas>();
        backText = backText.GetComponent<Button>();
	}
	
    public void BackPress()
    {
        helpMenu.enabled = false;
        startMenu.enabled = true;
        DisableHelp();
        startMenuObj.EnableStart();
    }

    public void DisableHelp()
    {
        backText.enabled = false;
    }

    public void EnableHelp()
    {
        backText.enabled = true;
    }

}
