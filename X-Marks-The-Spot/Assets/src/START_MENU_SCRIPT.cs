using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class START_MENU_SCRIPT : MonoBehaviour
{
    private Sprite[] sprites;
    private GameObject Timer_GO;
    private Timer_Menu menuTimer;
    private int currentSprite;
    private string resourceName = "Backgrounds";

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
        currentSprite = 0;
        Timer_GO = GameObject.Find("menuTimer");
        menuTimer = Timer_GO.GetComponent<Timer_Menu>();
        sprites = Resources.LoadAll<Sprite>(resourceName);
        if (sprites != null)
            print("great");

        for (int i = 0; i < sprites.Length; i++ )
        {
            print(i);
        }
        print(sprites.Length);

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

    void Update()
    {
        if (menuTimer.f_time > 5)
        {
            if(currentSprite >= sprites.Length-1)
            {
                currentSprite = 0;
            }
            else
            {
                currentSprite++;
            }
            SetBackground();
            ResetTimer();            
        }
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

    public void ResetTimer()
    {
        menuTimer.f_time = 0;
    }

    public void SetBackground()
    {
        GameObject.Find("Panel").GetComponent<Image>().sprite = sprites[currentSprite];
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
