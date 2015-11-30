using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Arrows : MonoBehaviour {

    RawImage riLeft;
    RawImage riRight;
    Player pl;
    bool isFirstFrame = true;

	void Start ()
    {
       
	}
	
	
	void Update ()
    {
        if (isFirstFrame)
        {
            riLeft = GameObject.Find("UI_Left").GetComponent<RawImage>();
            riRight = GameObject.Find("UI_Right").GetComponent<RawImage>();
            pl = GameObject.Find("Player(Clone)").GetComponent<Player>();
            isFirstFrame = false;
        }

	    if (pl.GetNextAction() == "TurnLeft")
        {
            riRight.enabled = false;
            riLeft.enabled = true;

        }

        if (pl.GetNextAction() == "TurnRight")
        {
            riLeft.enabled = false;
            riRight.enabled = true;
        }

        if (pl.GetNextAction() == "")
        {
            riRight.enabled = false;
            riLeft.enabled = false;
        }
    }
}
