using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class HELP_MENU_SCRIPT : MonoBehaviour 
{
    private Canvas startMenu;
    private Canvas helpMenu;

    private Button backText;

    private START_MENU_SCRIPT startMenuObj;
    private HELP_MENU_SCRIPT helpMenuObj;

    public EventSystem eventSys;
    private GameObject backGameObj;
    private GameObject startMenuGameObj;
    private GameObject helpMenuGameObj;

	void Start() 
    {
        
	}

	public void LoadComponents()
    {
        startMenuGameObj = GameObject.Find("START_MENU_CANVAS");
        helpMenuGameObj = GameObject.Find("HELP_MENU_CANVAS");
        backGameObj = GameObject.Find("HELP_BACK_TEXT");

        startMenuObj = startMenuGameObj.GetComponent<START_MENU_SCRIPT>();
        helpMenuObj = helpMenuGameObj.GetComponent<HELP_MENU_SCRIPT>();

        startMenu = startMenuGameObj.GetComponent<Canvas>();
        helpMenu = helpMenuGameObj.GetComponent<Canvas>();
        backText = backGameObj.GetComponent<Button>();
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
        eventSys.SetSelectedGameObject(backGameObj);
    }

}
