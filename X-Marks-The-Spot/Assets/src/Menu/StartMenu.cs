using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class StartMenu : MonoBehaviour
{
    /**---------------------------------------------------------------------------------
     * GameObjects associated with the script.
     */
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

    /**---------------------------------------------------------------------------------
     * Class objects associated with the script.
     */
    private StartMenu startMenuObj;
    private OptionsMenu optionsMenuObj;
    private LoadLevelMenu loadMenuObj;
    private HelpMenu helpMenuObj;
    private TimerMenu menuTimer;

    /**---------------------------------------------------------------------------------
     * Canvas associated with the script.
     */
    public Canvas quitMenu;
    public Canvas optionsMenu;
    public Canvas loadLevelMenu;
    public Canvas helpMenu;
    public Canvas startMenu;

    /**---------------------------------------------------------------------------------
     * Buttons associated with the script.
     */
    private Button startText;
    private Button exitText;
    private Button optionsText;
    private Button loadLevelText;
    private Button helpText;
    private Button noText;
    private Button yesText;

    /**---------------------------------------------------------------------------------
     * EventSystem used by the script.
     */
    public EventSystem eventSys;

    /**---------------------------------------------------------------------------------
     * Executes when the script starts.
     * Loads necessary components for start menu and all other menus in order to use them right away instead of waiting for each scripts Start() function.
     * LoadXML loads tile data.
     * Enables start menu and disables help, options and load level menu.
     * Highlights the Play button.
     */
    LoadingThreadHandler threadHandler;

    public Camera MainCamera;

    void
    Start()
    {
        World.Instance.LoadXML();
        threadHandler = new LoadingThreadHandler();

        LoadComponents();
        loadMenuObj.LoadComponents();
        helpMenuObj.LoadComponents();
        optionsMenuObj.LoadComponents();

        EnableStart();
        loadMenuObj.DisableLoadLevel();
        optionsMenuObj.DisableOptions();
        helpMenuObj.DisableHelp();

        eventSys.SetSelectedGameObject(playGameObj);
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
        startMenuGameObj = GameObject.Find("StartMenu_Canvas");
        helpMenuGameObj = GameObject.Find("HelpMenu_Canvas");
        loadMenuGameObj = GameObject.Find("LoadLevelMenu_Canvas");
        optionsMenuGameObj = GameObject.Find("OptionsMenu_Canvas");

        playGameObj = GameObject.Find("Play_TextBtn");
        optionsGameObj = GameObject.Find("Options_TextBtn");
        helpGameObj = GameObject.Find("Help_TextBtn");
        loadGameObj = GameObject.Find("LoadLevel_TextBtn");
        exitGameObj = GameObject.Find("Quit_TextBtn");
        yesGameObj = GameObject.Find("Yes_TextBtn");
        noGameObj = GameObject.Find("No_TextBtn");

        menuTimer = GameObject.Find("menuTimer").GetComponent<TimerMenu>();

        helpMenuObj = helpMenuGameObj.GetComponent<HelpMenu>();
        loadMenuObj = loadMenuGameObj.GetComponent<LoadLevelMenu>();
        optionsMenuObj = optionsMenuGameObj.GetComponent<OptionsMenu>();

        quitMenu = quitMenu.GetComponent<Canvas>();
        optionsMenu = optionsMenu.GetComponent<Canvas>();
        loadLevelMenu = loadLevelMenu.GetComponent<Canvas>();
        helpMenu = helpMenu.GetComponent<Canvas>();
        startMenu = startMenu.GetComponent<Canvas>();

        startText = playGameObj.GetComponent<Button>();
        exitText = exitGameObj.GetComponent<Button>();
        optionsText = optionsGameObj.GetComponent<Button>();
        loadLevelText = loadGameObj.GetComponent<Button>();
        helpText = helpGameObj.GetComponent<Button>();
        yesText = yesGameObj.GetComponent<Button>();
        noText = noGameObj.GetComponent<Button>();

        quitMenu.enabled = false;
        optionsMenu.enabled = false;
        loadLevelMenu.enabled = false;
        helpMenu.enabled = false;
        startMenu.enabled = true;
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
            StartCoroutine(Coroutine());
        }

    }

    IEnumerator Coroutine()
    {
        MainCamera.transform.position = new Vector3(256, 60, 256);
        MainCamera.transform.LookAt(World.Instance.StartPosition * 2);
        
        yield return new WaitForSeconds(0);

        if (threadHandler.Generated)
        {
            World.Instance.SetMapColor();
            World.Instance.loadFromMemory();
            threadHandler.UseGenerated();
            threadHandler.LoadAssets();
            Application.LoadLevel("Scene");
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
        eventSys.SetSelectedGameObject(noGameObj);
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
        loadMenuObj.EnableLoadLevel();
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
        optionsMenuObj.EnableOptions();
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
        helpMenuObj.EnableHelp();
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
        eventSys.SetSelectedGameObject(playGameObj);
    }
}
