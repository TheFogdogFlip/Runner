using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class InitGame : MonoBehaviour 
{
    public Canvas gameUI;
    public Canvas loadingScreen;
    public Text loadingText;

    /**---------------------------------------------------------------------------------
     * 
     */
	void Start () 
    {
        loadingText.text = "Loading Player...";
        Instantiate(Resources.Load("Player", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection));

        loadingText.text = "Loading Camera...";
        Instantiate(Resources.Load("PlayerCamera", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection));
	}

    /**---------------------------------------------------------------------------------
     * 
     */
	void Update () 
    {
        //Empty
	}
}
