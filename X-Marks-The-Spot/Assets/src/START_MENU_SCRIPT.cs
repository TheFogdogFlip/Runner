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
    private int topPanelLayer;
    private int botPanelLayer;
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
        needAlphaChanged = true;
        firstAlpha = false;
        secondAlpha = false;
        currentSprite = 0;
        nextSprite = currentSprite + 1;
        Timer_GO = GameObject.Find("menuTimer");
        panelBot = GameObject.Find("PANEL_BACKGROUND_BOT");
        panelTop = GameObject.Find("PANEL_BACKGROUND_TOP");
        menuTimer = Timer_GO.GetComponent<Timer_Menu>();
        sprites = Resources.LoadAll<Sprite>(resourceName);

        panelTop.GetComponent<Image>().sprite = sprites[currentSprite];
        panelBot.GetComponent<Image>().sprite = sprites[nextSprite];

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
        

        if (menuTimer.f_time > 3 && needAlphaChanged)
        {
            topPanelLayer = panelTop.layer;
            botPanelLayer = panelBot.layer;
            needAlphaChanged = false;
            ReduceAlpha();
        }
        if(menuTimer.f_time > 4)
        {
            if (currentSprite >= sprites.Length - 1)
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
            needAlphaChanged = true;
        }
        
        
    }

    public void ReduceAlpha()
    {
        //print("reduce alpha; layer bot: " + layerBot + " layer top: " + layerTop);

        if (botPanelLayer < topPanelLayer)
        {
            //Panel Bot is currently infront and needs to be faded
            print("fading bot; layer bot: " + botPanelLayer + " layer top: " + topPanelLayer);
            panelBot.GetComponent<Image>().CrossFadeAlpha(0, 1.0f, false);
        }

        else if (topPanelLayer < botPanelLayer)
        {
            //Panel Top is currently infront and needs to be faded
            print("fading top; layer bot: " + botPanelLayer + " layer top: " + topPanelLayer);
            panelTop.GetComponent<Image>().CrossFadeAlpha(0, 1.0f, false);

        }
    }

    public void SetBackground()
    {

        
        //print("set background; layer bot: " + botPanelLayer + " layer top: " + topPanelLayer);

        if (botPanelLayer < topPanelLayer)
        {
            //Panel Bot is currently infront and needs to be moved and set new background and then have it's alpha set to 1
            //print("swapping ");
            print("layer bot: " + botPanelLayer + " layer top: " + topPanelLayer + " ; setting bot bg, moving back bot, fading in bot behind top");
            panelBot.GetComponent<Image>().sprite = sprites[nextSprite];
            panelBot.layer = topPanelLayer;
            panelTop.layer = botPanelLayer;
            panelBot.GetComponent<Image>().CrossFadeAlpha(1, 0, false);
            print("after: layer bot: " + panelBot.layer + " layer top: " + panelTop.layer);
        }
        else if (topPanelLayer < botPanelLayer)
        {
            //Panel Top is currently infront and needs to be moved and set new background and then have it's alpha set to 1
            print("layer bot: " + botPanelLayer + " layer top: " + topPanelLayer + " ; setting top bg, moving back top, fading in top behind bot");
            panelTop.GetComponent<Image>().sprite = sprites[nextSprite];
            panelTop.layer = botPanelLayer;
            panelBot.layer = topPanelLayer;
            panelTop.GetComponent<Image>().CrossFadeAlpha(1, 0, false);
            print("after: layer bot: " + panelBot.layer + " layer top: " + panelTop.layer);
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
