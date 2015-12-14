using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class InitGame : MonoBehaviour {

    public Canvas gameUI;
    public Canvas loadingScreen;
    public Text loadingText;

    /**---------------------------------------------------------------------------------
     * 
     */
    void Awake()
    {
        loadingScreen.enabled = true;
        gameUI.enabled = false;

        loadingText.text = "Generating World...";
        World.Instance.Generate();

        loadingText.text = "Loading Audio...";
        AudioManager.Instance.InitAudio();

        loadingText.text = "Loading Player...";
        Instantiate(Resources.Load("Player", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection));

        loadingText.text = "Loading Camera...";
        Instantiate(Resources.Load("PlayerCamera", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection));

        

        loadingText.text = "Done!";
        loadingScreen.enabled = false;
        gameUI.enabled = true;
    }

    /**---------------------------------------------------------------------------------
     * 
     */
	void Start () 
    {
	    //Empty
	}

    /**---------------------------------------------------------------------------------
     * 
     */
	void Update () 
    {
        //Empty
	}
}
