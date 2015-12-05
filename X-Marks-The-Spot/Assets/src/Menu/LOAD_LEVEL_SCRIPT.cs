using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LOAD_LEVEL_SCRIPT : MonoBehaviour 
{
    public Canvas startMenu;
    public Canvas loadLevelMenu;

    public Button backText;

    // Use this for initialization
    void Start()
    {
        startMenu = startMenu.GetComponent<Canvas>();
        loadLevelMenu = loadLevelMenu.GetComponent<Canvas>();
        backText = backText.GetComponent<Button>();
    }

    public void BackPress()
    {
        loadLevelMenu.enabled = false;
        startMenu.enabled = true;
    }
}
