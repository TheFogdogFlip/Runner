using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class StartMenu : MonoBehaviour
{
    /**---------------------------------------------------------------------------------
     * GameObjects associated with the script.
     */
    private GameObject playGameObject;
    private GameObject loadGameObject;
    private GameObject helpGameObject;
    private GameObject optionsGameObject;
    private GameObject exitGameObject;
    private GameObject twitterGameObject;
    private GameObject yesGameObject;
    private GameObject noGameObject;
    private GameObject startMenuGameObject;
    private GameObject helpMenuGameObject;
    private GameObject loadMenuGameObject;
    private GameObject optionsMenuGameObject;
    private GameObject twitterMenuGameObject;
    private GameObject quitMenuGameObject;

    /**---------------------------------------------------------------------------------
     * Class objects associated with the script.
     */
    private StartMenu startMenuObject;
    private OptionsMenu optionsMenuObject;
    private LoadLevelMenu loadMenuObject;
    private HelpMenu helpMenuObject;
    private TwitterMenu twitterMenuObject;
    private TimerMenu menuTimer;

    /**---------------------------------------------------------------------------------
     * Canvas associated with the script.
     */
    private Canvas quitMenu;
    private Canvas optionsMenu;
    private Canvas loadLevelMenu;
    private Canvas helpMenu;
    private Canvas startMenu;
    private Canvas twitterMenu;

    /**---------------------------------------------------------------------------------
     * Buttons associated with the script.
     */
    private Button startText;
    private Button exitText;
    private Button optionsText;
    private Button loadLevelText;
    private Button helpText;
    private Button twitterImg;
    private Button noText;
    private Button yesText;

    /**---------------------------------------------------------------------------------
     * EventSystem used by the script.
     */
    public EventSystem eventSys;

    /**---------------------------------------------------------------------------------
     * LoadingThreadHandler required for loading during start menu.
     */
    private LoadingThreadHandler threadHandler;

    /**---------------------------------------------------------------------------------
     * Executes when the script starts.
     * Loads necessary components for start menu and all other menus in order to use them right away instead of waiting for each scripts Start() function.
     * LoadXML loads tile data.
     * Enables start menu and disables help, options load level and twitter menu.
     * Highlights the Play button.
     */
    void
    Start()
    {
        World.Instance.LoadXML();
        threadHandler = new LoadingThreadHandler();

        LoadComponents();
        loadMenuObject.LoadComponents();
        helpMenuObject.LoadComponents();
        optionsMenuObject.LoadComponents();
        twitterMenuObject.LoadCompononents();

        EnableStart();
        loadMenuObject.DisableLoadLevel();
        optionsMenuObject.DisableOptions();
        helpMenuObject.DisableHelp();
        twitterMenuObject.DisableTwitterMenu();

        eventSys.SetSelectedGameObject(playGameObject);
        threadHandler.GenerateWorld();
    }

    /**---------------------------------------------------------------------------------
     * Should only be executed once.
     * Loads necessary components for Start menu.
     * Changing the name of a GameObject in the scene will require changing the string in the respective GameObject.Find() call.
     */
    public void
    LoadComponents()
    {
        startMenuGameObject             = GameObject.Find("StartMenu_Canvas");
        helpMenuGameObject              = GameObject.Find("HelpMenu_Canvas");
        loadMenuGameObject              = GameObject.Find("LoadLevelMenu_Canvas");
        optionsMenuGameObject           = GameObject.Find("OptionsMenu_Canvas");
        quitMenuGameObject              = GameObject.Find("QuitMenu_Canvas");
        twitterMenuGameObject           = GameObject.Find("TwitterMenu_Canvas");

        playGameObject                  = GameObject.Find("Play_TextBtn");
        optionsGameObject               = GameObject.Find("Options_TextBtn");
        helpGameObject                  = GameObject.Find("Help_TextBtn");
        loadGameObject                  = GameObject.Find("LoadLevel_TextBtn");
        exitGameObject                  = GameObject.Find("Quit_TextBtn");
        twitterGameObject               = GameObject.Find("Twitter_ImgBtn");
        yesGameObject                   = GameObject.Find("Yes_TextBtn");
        noGameObject                    = GameObject.Find("No_TextBtn");

        menuTimer                       = GameObject.Find("menuTimer").GetComponent<TimerMenu>();

        helpMenuObject                  = helpMenuGameObject.GetComponent<HelpMenu>();
        loadMenuObject                  = loadMenuGameObject.GetComponent<LoadLevelMenu>();
        optionsMenuObject               = optionsMenuGameObject.GetComponent<OptionsMenu>();
        twitterMenuObject               = twitterMenuGameObject.GetComponent<TwitterMenu>();

        quitMenu                        = quitMenuGameObject.GetComponent<Canvas>();
        twitterMenu                     = twitterMenuGameObject.GetComponent<Canvas>();
        optionsMenu                     = optionsMenuGameObject.GetComponent<Canvas>();
        loadLevelMenu                   = loadMenuGameObject.GetComponent<Canvas>();
        helpMenu                        = helpMenuGameObject.GetComponent<Canvas>();
        startMenu                       = startMenuGameObject.GetComponent<Canvas>();

        startText                       = playGameObject.GetComponent<Button>();
        exitText                        = exitGameObject.GetComponent<Button>();
        optionsText                     = optionsGameObject.GetComponent<Button>();
        loadLevelText                   = loadGameObject.GetComponent<Button>();
        helpText                        = helpGameObject.GetComponent<Button>();
        twitterImg                      = twitterGameObject.GetComponent<Button>();
        yesText                         = yesGameObject.GetComponent<Button>();
        noText                          = noGameObject.GetComponent<Button>();

        quitMenu.enabled                = false;
        optionsMenu.enabled             = false;
        loadLevelMenu.enabled           = false;
        helpMenu.enabled                = false;
        twitterMenu.enabled             = false;
        startMenu.enabled               = true;
    }

    /**---------------------------------------------------------------------------------
     * Executed every frame. 
     * Simply checks if the player has pressed play, in which case UnityChans jump animation has started and after 0.77 sec the scene "Scene" is loaded.
     */
    void
    Update()
    {
        if (menuTimer.f_time > 0.77 && menuTimer.isActive)
        {
            if (threadHandler.Generated)
            {
                World.Instance.SetMapColor();
                World.Instance.loadFromMemory();
                threadHandler.UseGenerated();
                var test = World.Instance.GetTile(256, 256);
                threadHandler.LoadAssets();
                Application.LoadLevel("Scene");
            }
        }

    }

    /**---------------------------------------------------------------------------------
     * Executes when the mouse pointer is over a UI button
     * Highlights the specific GameObject for EventSystem.
     */
    public void
    HighlightItem(GameObject gameObj)
    {
        eventSys.SetSelectedGameObject(gameObj);
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Exit Game button is pressed.
     * Enables the Quit menu and disables the start menu without hiding it.
     * Sets the selected GameObject to the No button.
     */
    public void
    ExitPress()
    {
        quitMenu.enabled = true;
        yesText.enabled = true;
        noText.enabled = true;
        eventSys.SetSelectedGameObject(noGameObject);
        DisableStart();
    }

    /**---------------------------------------------------------------------------------
     * Executes when the No button is pressed.
     * Disables and hides the Quit menu and enables the Start menu.
     */
    public void
    NoPress()
    {
        quitMenu.enabled = false;
        yesText.enabled = false;
        noText.enabled = false;
        EnableStart();
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Load Level button is pressed. 
     * Disables the start menu and hides it and enables the Load Level menu. 
     */
    public void
    LoadLevelPress()
    {
        startMenu.enabled = false;
        loadLevelMenu.enabled = true;
        loadMenuObject.EnableLoadLevel();
        DisableStart();
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Options button is pressed.
     * Disables and hides the Start menu and enables the Options menu.
     */
    public void
    OptionsPress()
    {
        startMenu.enabled = false;
        optionsMenu.enabled = true;
        optionsMenuObject.EnableOptions();
        DisableStart();
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Help button is pressed.
     * Disables and hides the Start menu and enables the Help menu.
     */
    public void
    HelpMenuPress()
    {
        startMenu.enabled = false;
        helpMenu.enabled = true;
        helpMenuObject.EnableHelp();
        DisableStart();
    }

    /**---------------------------------------------------------------------------------
    * Executed when the Twitter button is pressed.
    * Disables and hides the Start menu and enables the Twitter menu.
    */
    public void
    TwitterPress()
    {
        startMenu.enabled = false;
        twitterMenu.enabled = true;
        twitterMenuObject.EnableTwitterMenu();
        DisableStart();
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Play button is pressed. 
     * Finds the GameObject called Player and gets it's Animator component.
     * Initiates the menu timer and plays UnityChan's "Jump" animation
     * Disables the Start menu without hiding it. 
     */
    public void
    PlayPress()
    {
        GameObject playerGameObject = GameObject.Find("Player");
        Animator anim;
        anim = playerGameObject.GetComponentInChildren<Animator>();

        menuTimer.f_time = 0;
        menuTimer.isActive = true;
        anim.Play("Jump");
        DisableStart();
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Yes button in the quit menu is pressed. 
     * Terminates the game.
     */
    public void
    ExitGame()
    {
        Application.Quit();
    }

    /**---------------------------------------------------------------------------------
     * Disables the start menu and it's components but does not hide it.
     */
    public void
    DisableStart()
    {
        exitText.enabled = false;
        startText.enabled = false;
        helpText.enabled = false;
        optionsText.enabled = false;
        loadLevelText.enabled = false;
    }

    /**---------------------------------------------------------------------------------
     * Enables the start menu and disables the Quit menu.
     * Sets the selected GameObject to the Play button.
     */
    public void
    EnableStart()
    {
        exitText.enabled = true;
        startText.enabled = true;
        helpText.enabled = true;
        optionsText.enabled = true;
        loadLevelText.enabled = true;
        yesText.enabled = false;
        noText.enabled = false;
        eventSys.SetSelectedGameObject(playGameObject);
    }
}
