using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HELP_MENU_SCRIPT : MonoBehaviour {

    public Canvas startMenu;
    public Canvas helpMenu;

    public Button backText;

	// Use this for initialization
	void Start() 
    {
        startMenu = startMenu.GetComponent<Canvas>();
        helpMenu = helpMenu.GetComponent<Canvas>();
        backText = backText.GetComponent<Button>();
	}
	
    public void BackPress()
    {
        helpMenu.enabled = false;
        startMenu.enabled = true;
    }
}
