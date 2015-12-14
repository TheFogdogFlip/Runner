using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TwitterMenu : MonoBehaviour 
{
    /**---------------------------------------------------------------------------------
     * GameObjects associated with the script.
     */
    private GameObject twitterMenuGameObject;
    private GameObject startMenuGameObject;
    private GameObject backGameObject;

    /**---------------------------------------------------------------------------------
     * Class objects associated with the script.
     */
    private StartMenu startMenuObject;

    /**---------------------------------------------------------------------------------
     * Canvas associated with the script.
     */
    private Canvas twitterMenu;
    private Canvas startMenu;

    /**---------------------------------------------------------------------------------
     * Buttons associated with the script.
     */
    private Button backText;

    /**---------------------------------------------------------------------------------
     * EventSystem used by the script.
     */
    public EventSystem eventSys;

    /**---------------------------------------------------------------------------------
     * Executed when the script starts.
     */
	void 
    Start () 
    {
	
	}

    /**---------------------------------------------------------------------------------
     * Should only be executed once.
     * Loads necessary components for Twitter menu.
     * Changing the name of a GameObject in the scene will require changing the string in the respective GameObject.Find() call.
     */
    public void 
    LoadCompononents()
    {
        startMenuGameObject             = GameObject.Find("StartMenu_Canvas");
        twitterMenuGameObject           = GameObject.Find("TwitterMenu_Canvas");
        backGameObject                  = GameObject.Find("TwitterBack_TextBtn");

        startMenuObject                 = startMenuGameObject.GetComponent<StartMenu>(); 

        twitterMenu                     = twitterMenuGameObject.GetComponent<Canvas>();
        startMenu                       = startMenuGameObject.GetComponent<Canvas>();
        backText                        = backGameObject.GetComponent<Button>();    
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Twitter back button is pressed. 
     * Disables the twitter menu and enables the start menu.
     */
    public void 
    BackPress()
    {
        twitterMenu.enabled = false;
        startMenu.enabled = true;
        DisableTwitterMenu();
        startMenuObject.EnableStart();
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Options menu needs to be enabled.
     * Enables the canvas and all the components associated with the script.
     */
    public void
    EnableTwitterMenu()
    {
        backText.enabled = true;
        eventSys.SetSelectedGameObject(backGameObject);
    }

    /**---------------------------------------------------------------------------------
     * Executed when the Twitter menu needs to be disabled.
     * Disables the canvas and all the components associated with the script.
     */
    public void
    DisableTwitterMenu()
    {
        backText.enabled = false;
    }

    
}
