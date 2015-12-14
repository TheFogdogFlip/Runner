using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class HelpMenu : MonoBehaviour 
{
    /**---------------------------------------------------------------------------------
     * GameObjects associated with the script.
     */
    private GameObject backGameObj;
    private GameObject startMenuGameObj;
    private GameObject helpMenuGameObj;

    /**---------------------------------------------------------------------------------
     * Canvas associated with the script.
     */
    private Canvas startMenu;
    private Canvas helpMenu;

    /**---------------------------------------------------------------------------------
     * Buttons associated with the script.
     */
    private Button backText;

    /**---------------------------------------------------------------------------------
     * Class objects used by the script.
     */
    private StartMenu startMenuObj;
    private HelpMenu helpMenuObj;

    /**---------------------------------------------------------------------------------
     * EventSystem used by the script.
     */
    public EventSystem eventSys;
    

    /**---------------------------------------------------------------------------------
     * Executed when the script starts.
     */
	void 
    Start() 
    {
        //Empty
	}

    /**---------------------------------------------------------------------------------
     * Should only be executed once.
     * Loads all components necessary for the script.
     * Changing the name of a GameObject in the scene will require changing the string in the respective GameObject.Find() call.
     */
    public void 
    LoadComponents()
    {
        startMenuGameObj            = GameObject.Find("StartMenu_Canvas");
        helpMenuGameObj             = GameObject.Find("HelpMenu_Canvas");
        backGameObj                 = GameObject.Find("HelpBack_TextBtn");

        startMenuObj                = startMenuGameObj.GetComponent<StartMenu>();
        helpMenuObj                 = helpMenuGameObj.GetComponent<HelpMenu>();

        startMenu                   = startMenuGameObj.GetComponent<Canvas>();
        helpMenu                    = helpMenuGameObj.GetComponent<Canvas>();
        backText                    = backGameObj.GetComponent<Button>();
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Back button is pressed.
     * Disables help menu and enables start menu.
     */
    public void 
    BackPress()
    {
        DisableHelp();
        startMenuObj.EnableStart();
        startMenu.enabled = true;
    }

    /**---------------------------------------------------------------------------------
     * Executed when the help menu needs to be hidden.
     * Hides the help menu and disables it's components.
     */
    public void 
    DisableHelp()
    {
        helpMenu.enabled = false;
        backText.enabled = false;
    }

    /**---------------------------------------------------------------------------------
     * Executed when the help menu needs to be shown.
     * Shows the help menu and enables it's components.
     */
    public void 
    EnableHelp()
    {
        backText.enabled = true;
        eventSys.SetSelectedGameObject(backGameObj);
    }

}
