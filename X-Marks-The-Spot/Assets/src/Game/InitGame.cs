using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class InitGame : MonoBehaviour {

    public Canvas gameUI;
    public Canvas loadingScreen;
    public Camera loadingScreenCamera;
    public Text loadingText;
    void Awake()
    {
        loadingScreenCamera.enabled = true;
        loadingScreen.enabled = true;
        gameUI.enabled = false;

        loadingText.text = "Generating World...";
        World.Instance.Generate();

        loadingText.text = "Loading Player...";
        Instantiate(Resources.Load("Player", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection));

        loadingText.text = "Loading Camera...";
        Instantiate(Resources.Load("PlayerCamera", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection));

        loadingText.text = "Done!";
        loadingScreen.enabled = false;
        gameUI.enabled = true;
        loadingScreenCamera.enabled = false;
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {

	}
}
