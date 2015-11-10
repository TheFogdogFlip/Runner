using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class START_MENU_SCRIPT : MonoBehaviour
{

    public Canvas quitMenu;
    public Button startText;
    public Button exitText;
    public Button optionsText;
	// Use this for initialization
	void Start ()
    {
        quitMenu = quitMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        optionsText = optionsText.GetComponent<Button>();
        quitMenu.enabled = false;
	}
	
	public void ExitPress()
    {
        quitMenu.enabled = true;
        startText.enabled = false;
        exitText.enabled = false;
        optionsText.enabled = false;
    }

    

    public void NoPress()
    {
        quitMenu.enabled = false;
        startText.enabled = true;
        exitText.enabled = true;
        optionsText.enabled = true;
    }

    public void StartLevel()
    {
        Application.LoadLevel(2);

    }

    public void OptionsPress()
    {
        Application.LoadLevel(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
