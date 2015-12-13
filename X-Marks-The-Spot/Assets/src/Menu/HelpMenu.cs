using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class HelpMenu : MonoBehaviour 
{
    private Canvas startMenu;
    private Canvas helpMenu;

    private Button backText;

    private StartMenu startMenuObj;
    private HelpMenu helpMenuObj;

    public EventSystem eventSys;
    private GameObject backGameObj;
    private GameObject startMenuGameObj;
    private GameObject helpMenuGameObj;

    /**---------------------------------------------------------------------------------
     * 
     */
	void 
    Start() 
    {
        //Empty
	}

    /**---------------------------------------------------------------------------------
     * 
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
     * 
     */
    public void 
    BackPress()
    {
        helpMenu.enabled = false;
        startMenu.enabled = true;
        DisableHelp();
        startMenuObj.EnableStart();
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    DisableHelp()
    {
        backText.enabled = false;
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    EnableHelp()
    {
        backText.enabled = true;
        eventSys.SetSelectedGameObject(backGameObj);
    }

}
