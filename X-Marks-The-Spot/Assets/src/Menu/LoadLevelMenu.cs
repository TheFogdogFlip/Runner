using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.IO;

public class LoadLevelMenu : MonoBehaviour 
{
    /**---------------------------------------------------------------------------------
     * GameObjects associated with the script.
     */
    private GameObject backGameObject;
    private GameObject startMenuGameObject;
    private GameObject loadMenuGameObject;

    /**---------------------------------------------------------------------------------
     * Canvas associated with the script.
     */
    private Canvas startMenu;
    private Canvas loadLevelMenu;

    /**---------------------------------------------------------------------------------
     * Buttons associated with the script.
     */
    private Button backText;

    /**---------------------------------------------------------------------------------
     * Class objects associated with the script.
     */
    private StartMenu startMenuObject;

    /**---------------------------------------------------------------------------------
     * EventSystem used by the script.
     */
    public EventSystem eventSys;
    /**---------------------------------------------------------------------------------
    * When no saved file is selected the mapName is "default"
    */
    public static string mapName = "default";
    
    /**---------------------------------------------------------------------------------
     * 
     */
    void 
    Start()
    {
        GetMaps();
    }

    /**---------------------------------------------------------------------------------
     * Should only be executed once.
     * Loads necessary components for Load Level menu.
     * Changing the name of a GameObject in the scene will require changing the string in the respective GameObject.Find() call.
     */
    public void 
    LoadComponents()
    {
        startMenuGameObject             = GameObject.Find("StartMenu_Canvas");
        loadMenuGameObject              = GameObject.Find("LoadLevelMenu_Canvas");
        backGameObject                  = GameObject.Find("LoadLevelBack_TextBtn");

        startMenuObject                 = startMenuGameObject.GetComponent<StartMenu>();

        startMenu                       = startMenuGameObject.GetComponent<Canvas>();
        loadLevelMenu                   = loadMenuGameObject.GetComponent<Canvas>();
        backText                        = backGameObject.GetComponent<Button>();
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Load Level back button is pressed. 
     * Disables the load level menu and enables the start menu.
     */
    public void 
    BackPress()
    {
        loadLevelMenu.enabled = false;
        startMenu.enabled = true;
        DisableLoadLevel();
        startMenuObject.EnableStart();
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Load Level menu needs to be disabled.
     * Disables the canvas and all the components associated with the script.
     */
    public void 
    DisableLoadLevel()
    {
        backText.enabled = false;
    }

    /**---------------------------------------------

------------------------------------
    * 
    */
    public string[]
    GetMaps()
    {

        //check all files MAPSAVE*.PNG in current directory 

        string path = Directory.GetCurrentDirectory();
        //DirectoryInfo dir = new DirectoryInfo (path);
        string[] info = Directory.GetFiles(path, "MAPSAVE*.PNG");
        int i = 1;

        //update buttons in load screen so that a saved map can be loaded
        int numberOfButtons = 5; //fixed number of buttons, five right now
        print(info.Length);
        int lenghtMax = info.Length;

        if (lenghtMax > numberOfButtons)
            lenghtMax = numberOfButtons;
        for (; i <= lenghtMax; ++i)
        {
            GameObject.Find("LoadGame" + i).GetComponent<Text>().text = System.IO.Path.GetFileName(info[i - 1]);
        }

        //disable all buttons where no save is present
        for (; i <= numberOfButtons; ++i)
        {
            GameObject.Find("LoadGame" + i).GetComponent<Button>().enabled = false;
        }

        return info;
    }

    /**---------------------------------------------------------------------------------
    * Executed when a savegame is selected in the Load map menu
    */
    public void
    LoadLevelSelection(string theSave)
    {
        //set the selected mapName to the selected saved map
        print("Selected save: " + theSave);
        mapName = theSave;
    }


    /**---------------------------------------------------------------------------------
     * Executed when the Load Level menu needs to be enabled.
     * Enables the canvas and all the components associated with the script.
     */
    public void 
    EnableLoadLevel()
    {
        backText.enabled = true;
        eventSys.SetSelectedGameObject(backGameObject);
    }
}
