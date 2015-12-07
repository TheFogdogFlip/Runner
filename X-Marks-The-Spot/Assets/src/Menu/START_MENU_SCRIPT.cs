using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class START_MENU_SCRIPT : MonoBehaviour
{
    private Sprite[] sprites;

    private GameObject Timer_GO;
    private GameObject panelFront;
    private GameObject panelBack;

    private Image imageBack;
    private Image imageFront;

    private Timer_Menu menuTimer;

    private int currentSprite;
    private int nextSprite;
    private int topPanelLayer;
    private int botPanelLayer;

    private string resourceName = "Backgrounds";

    private bool needAlphaReduced;
    private bool needPictureChanged;

    public Canvas quitMenu;
    public Canvas optionsMenu;
    public Canvas loadLevelMenu;
    public Canvas helpMenu;
    public Canvas startMenu;

    public Button startText;
    public Button exitText;
    public Button optionsText;
    public Button loadLevelText;


	void Start ()
    {
        needAlphaReduced = true;
        needPictureChanged = true;

        currentSprite = 0;
        Timer_GO = GameObject.Find("menuTimer");
        panelBack = GameObject.Find("PANEL_BACKGROUND_BOT");
        panelFront = GameObject.Find("PANEL_BACKGROUND_TOP");

        //panelBack.layer = 9;
        panelFront.layer = 8;
        menuTimer = Timer_GO.GetComponent<Timer_Menu>();
        sprites = Resources.LoadAll<Sprite>(resourceName);
        //imageBack = panelFront.GetComponent<Image>();
        imageFront = panelFront.GetComponent<Image>();

        imageFront.sprite = sprites[currentSprite];

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

        quitMenu.enabled = false;
        optionsMenu.enabled = false;
        loadLevelMenu.enabled = false;
        helpMenu.enabled = false;
        startMenu.enabled = true;
	}

    void Update()
    {
        if (menuTimer.f_time > 4 && needAlphaReduced)
        {
            needAlphaReduced = false;
            ReduceAlpha();
        }
        if (menuTimer.f_time > 4.5 && needPictureChanged)
        {
            needPictureChanged = false;
            SetBackground();
        }

        if(menuTimer.f_time > 4.6)
        {
            IncreaseAlpha();
            ResetTimer();

            needAlphaReduced = true;
            needPictureChanged = true;
        }
    }

    public void ReduceAlpha()
    {
        imageFront.CrossFadeAlpha(0, 0.5f, false);
    }

    public void IncreaseAlpha()
    {
        imageFront.CrossFadeAlpha(1, 0.5f, false);
    }

    public void SetBackground()
    {
        currentSprite++;

        if (currentSprite == sprites.Length)
        {
            currentSprite = 0;
        }
        imageFront.sprite = sprites[currentSprite];
    }

	public void ExitPress()
    {
        quitMenu.enabled = true;
        startText.enabled = false;
        exitText.enabled = false;
        optionsText.enabled = false;
        loadLevelText.enabled = false;
    }

    public void NoPress()
    {
        quitMenu.enabled = false;
        startText.enabled = true;
        exitText.enabled = true;
        optionsText.enabled = true;
        loadLevelText.enabled = true;
    }

    public void ResetTimer()
    {
        menuTimer.f_time = 0;
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
