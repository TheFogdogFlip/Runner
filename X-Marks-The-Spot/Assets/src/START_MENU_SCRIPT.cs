using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class START_MENU_SCRIPT : MonoBehaviour
{
    public GameObject background;

    public Canvas quitMenu;
    public Canvas optionsMenu;
    public Canvas loadLevelMenu;
    public Canvas helpMenu;
    public Canvas startMenu;

    public Button startText;
    public Button exitText;
    public Button optionsText;
    public Button loadLevelText;
    public Button testGameText;

	void Start ()
    {

        background = GameObject.Find("Background");
        if(background.name =="Background")
        { 
            print("hej");
        }
        quitMenu = quitMenu.GetComponent<Canvas>();
        optionsMenu = optionsMenu.GetComponent<Canvas>();
        loadLevelMenu = loadLevelMenu.GetComponent<Canvas>();
        helpMenu = helpMenu.GetComponent<Canvas>();
        startMenu = startMenu.GetComponent<Canvas>();
        //globalMenu = globalMenu.GetComponent<Canvas>();
        
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        optionsText = optionsText.GetComponent<Button>();
        loadLevelText = loadLevelText.GetComponent<Button>();
        testGameText = testGameText.GetComponent<Button>();

        quitMenu.enabled = false;
        optionsMenu.enabled = false;
        loadLevelMenu.enabled = false;
        helpMenu.enabled = false;
        startMenu.enabled = true;
	}
	
	public void ExitPress()
    {
        quitMenu.enabled = true;
        startText.enabled = false;
        exitText.enabled = false;
        optionsText.enabled = false;
        loadLevelText.enabled = false;
        testGameText.enabled = false;
    }

    public void NoPress()
    {
        quitMenu.enabled = false;
        startText.enabled = true;
        exitText.enabled = true;
        optionsText.enabled = true;
        loadLevelText.enabled = true;
        testGameText.enabled = true;
    }

    public void TestGamePress()
    {
        Application.LoadLevel(4);
    }
    public void LoadLevelPress()
    {
        startMenu.enabled = false;
        loadLevelMenu.enabled = true;
    }
    public void OptionsPress()
    {
        startMenu.enabled = false;
        optionsMenu.enabled = true;
    }

    public void HelpMenuPress()
    {
        startMenu.enabled = false;
        helpMenu.enabled = true;
    }
    public void StartLevel()
    {
        Application.LoadLevel("Scene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
