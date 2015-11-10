using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class START_MENU_SCRIPT : MonoBehaviour
{
    public Canvas quitMenu;
    public Button startText;
    public Button loadLevelText;
    public Button exitText;
    public Button optionsText;

	void Start ()
    {
        quitMenu = quitMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        optionsText = optionsText.GetComponent<Button>();
        loadLevelText = loadLevelText.GetComponent<Button>();
        quitMenu.enabled = false;
	}
	
	public void ExitPress()
    {
        quitMenu.enabled = true;
        loadLevelText.enabled = false;
        startText.enabled = false;
        exitText.enabled = false;
        optionsText.enabled = false;
    }

    public void NoPress()
    {
        quitMenu.enabled = false;
        loadLevelText.enabled = true;
        startText.enabled = true;
        exitText.enabled = true;
        optionsText.enabled = true;
    }

    public void StartLevel()
    {
        Application.LoadLevel(3);
    }

    public void LoadLevelPress()
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
