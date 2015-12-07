using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class LOAD_LEVEL_SCRIPT : MonoBehaviour 
{
    private Canvas startMenu;
    private Canvas loadLevelMenu;

    private Button backText;

    private START_MENU_SCRIPT startMenuObj;
    private LOAD_LEVEL_SCRIPT loadMenuObj;
    
    public EventSystem eventSys;

    private GameObject backGameObj;
    private GameObject startMenuGameObj;
    private GameObject loadMenuGameObj;
    void Start()
    {
        
    }

    public void LoadComponents()
    {
        startMenuGameObj = GameObject.Find("START_MENU_CANVAS");
        loadMenuGameObj = GameObject.Find("LOAD_LEVEL_CANVAS");
        backGameObj = GameObject.Find("LOAD_BACK_TEXT");

        startMenuObj = startMenuGameObj.GetComponent<START_MENU_SCRIPT>();
        loadMenuObj = loadMenuGameObj.GetComponent<LOAD_LEVEL_SCRIPT>();

        startMenu = startMenuGameObj.GetComponent<Canvas>();
        loadLevelMenu = loadMenuGameObj.GetComponent<Canvas>();
        backText = backGameObj.GetComponent<Button>();
    }

    public void BackPress()
    {
        loadLevelMenu.enabled = false;
        startMenu.enabled = true;
        DisableLoadLevel();
        startMenuObj.EnableStart();
    }

    public void DisableLoadLevel()
    {
        backText.enabled = false;
        print("here");
    }

    public void EnableLoadLevel()
    {
        backText.enabled = true;
        eventSys.SetSelectedGameObject(backGameObj);
    }
}
