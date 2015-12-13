using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class START_MENU_SCRIPT : MonoBehaviour
{
    private Sprite[] sprites;

    private GameObject Timer_GO;
    //private GameObject panelFront;
    //private GameObject panelBack;

    //private Image imageBack;
   //private Image imageFront;

    private Timer_Menu menuTimer;

    private int currentSprite;
    private int nextSprite;

    private string resourceName = "Backgrounds";

    private bool needAlphaReduced;
    private bool needPictureChanged;

    private GameObject playGameObj;
    private GameObject loadGameObj;
    private GameObject helpGameObj;
    private GameObject optionsGameObj;
    private GameObject exitGameObj;
    private GameObject yesGameObj;
    private GameObject noGameObj;
    private GameObject startMenuGameObj;
    private GameObject helpMenuGameObj;
    private GameObject loadMenuGameObj;
    private GameObject optionsMenuGameObj;

    private START_MENU_SCRIPT startMenuObj;
    private OPTIONS_MENU_SCRIPT optionsMenuObj;
    private LOAD_LEVEL_SCRIPT loadMenuObj;
    private HELP_MENU_SCRIPT helpMenuObj;

    public Canvas quitMenu;
    public Canvas optionsMenu;
    public Canvas loadLevelMenu;
    public Canvas helpMenu;
    public Canvas startMenu;

    private Button startText;
    private Button exitText;
    private Button optionsText;
    private Button loadLevelText;
    private Button helpText;
    private Button noText;
    private Button yesText;

    public EventSystem eventSys;

	void Start ()
    {
        LoadComponents();
        loadMenuObj.LoadComponents();
        helpMenuObj.LoadComponents();
        optionsMenuObj.LoadComponents();
       
        EnableStart();
        loadMenuObj.DisableLoadLevel();
        optionsMenuObj.DisableOptions();
        helpMenuObj.DisableHelp();

        eventSys.SetSelectedGameObject(playGameObj);


	}
    public void LoadComponents()
    {
        needAlphaReduced            = true;
        needPictureChanged          = true;

        currentSprite               = 0;
        Timer_GO                    = GameObject.Find("menuTimer");
        //panelBack                   = GameObject.Find("PANEL_BACKGROUND_BOT");
        //panelFront                  = GameObject.Find("PANEL_BACKGROUND_TOP");
        //panelFront.layer            = 8;
        menuTimer                   = Timer_GO.GetComponent<Timer_Menu>();
        sprites                     = Resources.LoadAll<Sprite>(resourceName);
        //imageFront                  = panelFront.GetComponent<Image>();
        //imageFront.sprite           = sprites[currentSprite];

        startMenuGameObj            = GameObject.Find("START_MENU_CANVAS");
        helpMenuGameObj             = GameObject.Find("HELP_MENU_CANVAS");
        loadMenuGameObj             = GameObject.Find("LOAD_LEVEL_CANVAS");
        optionsMenuGameObj          = GameObject.Find("OPTIONS_MENU_CANVAS");

        helpMenuObj                 = helpMenuGameObj.GetComponent<HELP_MENU_SCRIPT>();
        loadMenuObj                 = loadMenuGameObj.GetComponent<LOAD_LEVEL_SCRIPT>();
        optionsMenuObj              = optionsMenuGameObj.GetComponent<OPTIONS_MENU_SCRIPT>();

        quitMenu                    = quitMenu.GetComponent<Canvas>();
        optionsMenu                 = optionsMenu.GetComponent<Canvas>();
        loadLevelMenu               = loadLevelMenu.GetComponent<Canvas>();
        helpMenu                    = helpMenu.GetComponent<Canvas>();
        startMenu                   = startMenu.GetComponent<Canvas>();

        playGameObj                 = GameObject.Find("PLAY_TEXT");
        optionsGameObj              = GameObject.Find("OPTIONS_TEXT");
        helpGameObj                 = GameObject.Find("HELP_TEXT");
        loadGameObj                 = GameObject.Find("LOAD_LEVEL_TEXT");
        exitGameObj                 = GameObject.Find("QUIT_TEXT");
        yesGameObj                  = GameObject.Find("YES");
        noGameObj                   = GameObject.Find("NO");

        startText                   = playGameObj.GetComponent<Button>();
        exitText                    = exitGameObj.GetComponent<Button>();
        optionsText                 = optionsGameObj.GetComponent<Button>();
        loadLevelText               = loadGameObj.GetComponent<Button>();
        helpText                    = helpGameObj.GetComponent<Button>();
        yesText                     = yesGameObj.GetComponent<Button>();
        noText                      = noGameObj.GetComponent<Button>();

        quitMenu.enabled            = false;
        optionsMenu.enabled         = false;
        loadLevelMenu.enabled       = false;
        helpMenu.enabled            = false;
        startMenu.enabled           = true;
    }

    void Update()
    {
        //if (menuTimer.f_time > 4 && needAlphaReduced)
        //{
        //    needAlphaReduced = false;
        //    ReduceAlpha();
        //}
        //if (menuTimer.f_time > 4.5 && needPictureChanged)
        //{
        //    needPictureChanged = false;
        //    SetBackground();
        //}

        //if(menuTimer.f_time > 4.6)
        //{
        //    IncreaseAlpha();
        //    ResetTimer();

        //    needAlphaReduced = true;
        //    needPictureChanged = true;
        //}
    }

  
    public void HighlightItem(GameObject gameObj)
    {
        eventSys.SetSelectedGameObject(gameObj);
    }

    //public void ReduceAlpha()
    //{
    //    imageFront.CrossFadeAlpha(0, 0.5f, false);
    //}

    //public void IncreaseAlpha()
    //{
    //    imageFront.CrossFadeAlpha(1, 0.5f, false);
    //}

    //public void SetBackground()
    //{
    //    currentSprite++;

    //    if (currentSprite == sprites.Length)
    //    {
    //        currentSprite = 0;
    //    }
    //    imageFront.sprite = sprites[currentSprite];
    //}

	public void ExitPress()
    {
        quitMenu.enabled = true;
        yesText.enabled = true;
        noText.enabled = true;
        eventSys.SetSelectedGameObject(noGameObj);
        DisableStart();
    }

    public void NoPress()
    {
        quitMenu.enabled = false;
        yesText.enabled = false;
        noText.enabled = false;
        EnableStart();
    }

    public void ResetTimer()
    {
        menuTimer.f_time = 0;
    }

    public void LoadLevelPress()
    {
        startMenu.enabled = false;
        loadLevelMenu.enabled = true;
        loadMenuObj.EnableLoadLevel();
        DisableStart();
    }
    public void OptionsPress()
    {
        startMenu.enabled = false;
        optionsMenu.enabled = true;
        optionsMenuObj.EnableOptions();
        DisableStart();
    }

    public void HelpMenuPress()
    {
        startMenu.enabled = false;
        helpMenu.enabled = true;
        helpMenuObj.EnableHelp();
        DisableStart();
    }
    public void StartLevel()
    {
        Application.LoadLevel("Scene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void DisableStart()
    {
        exitText.enabled = false;
        startText.enabled = false;
        helpText.enabled = false;
        optionsText.enabled = false;
        loadLevelText.enabled = false;
    }

    public void EnableStart()
    {
        exitText.enabled = true;
        startText.enabled = true;
        helpText.enabled = true;
        optionsText.enabled = true;
        loadLevelText.enabled = true;
        yesText.enabled = false;
        noText.enabled = false;
        eventSys.SetSelectedGameObject(playGameObj);
    }
}
