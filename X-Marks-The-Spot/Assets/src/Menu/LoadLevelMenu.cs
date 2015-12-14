using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class LoadLevelMenu : MonoBehaviour 
{
    /**---------------------------------------------------------------------------------
     * 
     */
    private GameObject backGameObj;
    private GameObject startMenuGameObj;
    private GameObject loadMenuGameObj;

    /**---------------------------------------------------------------------------------
     * 
     */
    private Canvas startMenu;
    private Canvas loadLevelMenu;

    /**---------------------------------------------------------------------------------
     * 
     */
    private Button backText;

    /**---------------------------------------------------------------------------------
     * 
     */
    private StartMenu startMenuObj;
    private LoadLevelMenu loadMenuObj;

    /**---------------------------------------------------------------------------------
     * 
     */
    public EventSystem eventSys;

    

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
        loadMenuGameObj             = GameObject.Find("LoadLevelMenu_Canvas");
        backGameObj                 = GameObject.Find("LoadLevelBack_TextBtn");

        startMenuObj                = startMenuGameObj.GetComponent<StartMenu>();
        loadMenuObj                 = loadMenuGameObj.GetComponent<LoadLevelMenu>();

        startMenu                   = startMenuGameObj.GetComponent<Canvas>();
        loadLevelMenu               = loadMenuGameObj.GetComponent<Canvas>();
        backText                    = backGameObj.GetComponent<Button>();
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    BackPress()
    {
        loadLevelMenu.enabled = false;
        startMenu.enabled = true;
        DisableLoadLevel();
        startMenuObj.EnableStart();
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    DisableLoadLevel()
    {
        backText.enabled = false;
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    EnableLoadLevel()
    {
        backText.enabled = true;
        eventSys.SetSelectedGameObject(backGameObj);
    }
}
