using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Arrows : MonoBehaviour {

    RawImage riLeft;
    RawImage riRight;
    Player pl;

	void Start ()
    {
        riLeft = GameObject.Find("UI_Left").GetComponent<RawImage>();
        riRight = GameObject.Find("UI_Right").GetComponent<RawImage>();
        pl = GameObject.Find("Player(Clone)").GetComponent<Player>();
	}
	
	
	void Update ()
    {
	    if (pl.GetNextAction() == "TurnLeft")
        {
            riLeft.enabled = true;
        }

        if (pl.GetNextAction() == "TurnRight")
        {
            riRight.enabled = true;
        }

        if (pl.GetNextAction() == "")
        {
            riRight.enabled = false;
            riLeft.enabled = false;
        }
    }
}
