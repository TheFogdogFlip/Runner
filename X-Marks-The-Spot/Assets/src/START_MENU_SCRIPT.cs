using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class START_MENU_SCRIPT : MonoBehaviour
{
    private Sprite[] sprites;
    private GameObject Timer_GO;
    private GameObject panelTop;
    private GameObject panelBot;

    private Timer_Menu menuTimer;
    private int currentSprite;
    private int nextSprite;
    private string resourceName = "Backgrounds";
    private bool needAlphaChanged;
    private bool firstAlpha;
    private bool secondAlpha;

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
        needAlphaChanged = false;
        firstAlpha = false;
        secondAlpha = false;
        currentSprite = 0;
        nextSprite = currentSprite + 1;
        Timer_GO = GameObject.Find("menuTimer");
        panelBot = GameObject.Find("PANEL_BACKGROUND_BOT");
        panelTop = GameObject.Find("PANEL_BACKGROUND_TOP");
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
        
        if (menuTimer.f_time > 10)
        {
            if(currentSprite >= sprites.Length - 1)
            {
                currentSprite = 0;
            }
            else
            {
                currentSprite++;
                nextSprite = currentSprite + 1;
                if (nextSprite >= sprites.Length - 1)
                {
                    nextSprite = 0;
                }
            }
            
            
            SetBackground();
            ResetTimer();            
        }
        
        if(menuTimer.f_time > 4 && !firstAlpha)
        {
            needAlphaChanged = true;
            firstAlpha = true;
        }
        if (menuTimer.f_time > 9 && !secondAlpha)
        {
            needAlphaChanged = true;
            secondAlpha = true;
        }
        if (needAlphaChanged)
        {
            ReduceAlpha();
            needAlphaChanged = false;
        }
    }

    public void ReduceAlpha()
    {
        int layerBot = panelBot.layer;
        int layerTop = panelTop.layer;
        print("layer bot: " + layerBot + " layer top: " + layerTop);

        if (layerBot < layerTop)
        {
            //Panel Bot is currently being shown
            panelTop.GetComponent<Image>().CrossFadeAlpha(0, 1.0f, false);
            panelBot.GetComponent<Image>().CrossFadeAlpha(1, 0f, false);

            panelBot.layer = layerTop;
            panelTop.layer = layerBot;
        }
        else if (layerTop < layerBot)
        {
            //Panel Top is currently being shown
            panelBot.GetComponent<Image>().CrossFadeAlpha(0, 1.0f, false);
            panelTop.GetComponent<Image>().CrossFadeAlpha(1, 0f, false);
            panelBot.layer = layerTop;
            panelTop.layer = layerBot;

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

        int layerBot = panelBot.layer;
        int layerTop = panelTop.layer;
        print("layer bot: " + layerBot + " layer top: " + layerTop);

        if (layerBot < layerTop)
        {
            panelTop.GetComponent<Image>().sprite = sprites[nextSprite];
            panelBot.GetComponent<Image>().sprite = sprites[currentSprite];
        }
        else if (layerTop < layerBot)
        {
            panelTop.GetComponent<Image>().sprite = sprites[currentSprite];
            panelBot.GetComponent<Image>().sprite = sprites[nextSprite];
        }
    }

    

    public void TestGamePress()
    {
        Application.LoadLevel("Test_game");
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
