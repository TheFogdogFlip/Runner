using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class START_MENU_SCRIPT : MonoBehaviour
{
    private Sprite[] sprites;
    private GameObject Timer_GO;
    private GameObject panelTop;
    private GameObject panelBot;
    private Image imageTop;
    private Image imageBot;
    private Timer_Menu menuTimer;
    private int currentSprite;
    private int nextSprite;
    private int topPanelLayer;
    private int botPanelLayer;
    private string resourceName = "Backgrounds";
    private bool needAlphaChanged;
    private bool backFadedIn;
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
        backFadedIn = false;
        currentSprite = 0;
        nextSprite = 1;
        Timer_GO = GameObject.Find("menuTimer");
        //panelBot = GameObject.Find("PANEL_BACKGROUND_BOT");
        //panelBot.layer = 8;
        panelTop = GameObject.Find("PANEL_BACKGROUND_TOP");
        panelTop.layer = 9;
        menuTimer = Timer_GO.GetComponent<Timer_Menu>();
        //sprites = Resources.LoadAll<Sprite>(resourceName);
        //imageTop = panelTop.GetComponent<Image>();
        //imageBot = panelBot.GetComponent<Image>();

        //imageBot.sprite = sprites[currentSprite];
        //imageTop.sprite = sprites[nextSprite];
        
        
        //panelTop.GetComponent<Image>().sprite = sprites[nextSprite];
        //panelBot.GetComponent<Image>().sprite = sprites[currentSprite];

        //print("panelTop: " + " layer: " + panelTop.layer + " name: " + imageTop.sprite.name);
        //print("panelBot: " + " layer: " + panelBot.layer + " name: " + imageBot.sprite.name);

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
        

        //if (menuTimer.f_time > 3 && needAlphaChanged)
        //{
        //    needAlphaChanged = false;
        //    print("here: " + menuTimer.f_time);
        //    ReduceAlpha();
        //}
        //if(menuTimer.f_time > 5)
        //{
        //    ResetTimer();
        //    currentSprite++;
        //    if (currentSprite == sprites.Length)
        //    {
        //        currentSprite = 0;
        //    }
        //    nextSprite = currentSprite + 1;
        //    if (nextSprite == sprites.Length)
        //    {
        //        nextSprite = 0;
        //    }

        //    print("curr: " + currentSprite);
        //    print("next: " + nextSprite);
        //    SetBackground();
            
        //    needAlphaChanged = true;
        //    backFadedIn = false;
            
        //}
        //print("panelTop: " + " layer: " + panelTop.layer + " name: " + panelTop.GetComponent<Image>().sprite.name);
        //print("panelBot: " + " layer: " + panelBot.layer + " name: " + panelBot.GetComponent<Image>().sprite.name);
        
    }

    public void ReduceAlpha()
    {
        if (panelBot.layer == 8 && panelTop.layer == 9)
        {
            //Panel Bot is currently infront and needs to be faded
            print("fading bot");
            FadeImages(imageBot, imageTop);
        }

        else if (panelBot.layer == 9 && panelTop.layer == 8)
        {
            //Panel Top is currently infront and needs to be faded
            print("fading top");
            FadeImages(imageTop, imageBot);
            
        }
    }

    public void SwapLayers()
    {
        int temp = panelBot.layer;
        panelBot.layer = panelTop.layer;
        panelTop.layer = temp;
    }
    public void FadeImages(Image toBeShown, Image toBeFaded)
    {
        toBeShown.CrossFadeAlpha(1.0f, 1.0f, false);
        toBeFaded.CrossFadeAlpha(0f, 1.0f, false);
    }

    public void SetBackground()
    {

        if (panelBot.layer == 8 && panelTop.layer == 9)
        {
            //Panel Bot is currently infront and needs to be moved and set new background and then have it's alpha set to 1
            print("layer bot: " + panelBot.layer + " layer top: " + panelTop.layer + " ; setting bot bg, moving back bot, fading in bot behind top");
            imageBot.sprite = sprites[nextSprite];
/*
//             Color col = imageBot.color;
//             col.a = 1.0f;
//             imageBot.color = col;
//             col = imageBot.color;
//             float a = col.a;
//             print(a);
*/
            SwapLayers();
            //imageBot.CrossFadeAlpha(1.0f, 0f, true);
            print("after: layer bot: " + panelBot.layer + " layer top: " + panelTop.layer);
        }
        else if (panelBot.layer == 9 && panelTop.layer == 8)
        {
            //Panel Top is currently infront and needs to be moved and set new background and then have it's alpha set to 1
            print("layer bot: " + panelBot.layer + " layer top: " + panelTop.layer + " ; setting top bg, moving back top, fading in top behind bot");
            imageTop.sprite = sprites[nextSprite];
/*
//             Color col = imageTop.color;
//             col.a = 1.0f;
//             imageTop.color = col;
//             col = imageTop.color;
//             float a = col.a;
//             print(a);
*/
            SwapLayers();
            //imageTop.CrossFadeAlpha(1.0f, 0f, true);
            print("after: layer bot: " + panelBot.layer + " layer top: " + panelTop.layer);
        }

        print("panelTop: " + " layer: " + panelTop.layer + " name: " + imageTop.sprite.name);
        print("panelBot: " + " layer: " + panelBot.layer + " name: " + imageBot.sprite.name);
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
