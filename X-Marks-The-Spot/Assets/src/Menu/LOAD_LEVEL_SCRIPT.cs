using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LOAD_LEVEL_SCRIPT : MonoBehaviour 
{
    public Canvas startMenu;
    public Canvas loadLevelMenu;

    public Button backText;

    public START_MENU_SCRIPT startMenuObj;
    public LOAD_LEVEL_SCRIPT loadMenuObj;

    void Start()
    {
        startMenuObj = GameObject.Find("START_MENU_CANVAS").GetComponent<START_MENU_SCRIPT>();
        loadMenuObj = GameObject.Find("LOAD_LEVEL_CANVAS").GetComponent<LOAD_LEVEL_SCRIPT>();

        startMenu = startMenu.GetComponent<Canvas>();
        loadLevelMenu = loadLevelMenu.GetComponent<Canvas>();
        backText = backText.GetComponent<Button>();
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
    }

    public void EnableLoadLevel()
    {
        backText.enabled = true;
    }
}
