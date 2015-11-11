using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class START_MENU_SCRIPT : MonoBehaviour
{
    public Canvas quitMenu;
    public Button startText;
    public Button exitText;
    public Button optionsText;
    public Button loadLevelText;
    public Button testGameText;

	void Start ()
    {
        quitMenu = quitMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        optionsText = optionsText.GetComponent<Button>();
        loadLevelText = loadLevelText.GetComponent<Button>();
        testGameText = testGameText.GetComponent<Button>();

        quitMenu.enabled = false;
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
        Application.LoadLevel(3);
    }
    public void OptionsPress()
    {
        Application.LoadLevel(2);
    }
    public void StartLevel()
    {
        Application.LoadLevel(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
