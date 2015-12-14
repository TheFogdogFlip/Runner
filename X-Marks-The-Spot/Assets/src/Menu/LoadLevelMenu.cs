﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

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
     * 
     */
    void 
    Start()
    {
        //Empty 
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
