using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OPTIONS_MENU_SCRIPT : MonoBehaviour 
{
    public Canvas startMenu;
    public Canvas optionsMenu;

    public Button backText;

	// Use this for initialization
	void Start() 
    {
        startMenu = startMenu.GetComponent<Canvas>();
        optionsMenu = optionsMenu.GetComponent<Canvas>();
        backText = backText.GetComponent<Button>();
	}

    public void BackPress()
    {
        optionsMenu.enabled = false;
        startMenu.enabled = true;
    }
}
