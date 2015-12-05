using UnityEngine;
using UnityEngine.UI;

public class UI_Arrows : MonoBehaviour {

    RawImage riLeft;
    RawImage riRight;
    Player pl;
    bool isFirstFrame;

	void Start ()
    {
        isFirstFrame = true;
        riLeft = GameObject.Find("UI_Left").GetComponent<RawImage>();
        riRight = GameObject.Find("UI_Right").GetComponent<RawImage>();
	}
	
	
	void Update ()
    {
        if (isFirstFrame)
        {
            pl = GameObject.Find("Player(Clone)").GetComponent<Player>();
            isFirstFrame = false;
        }

	    if (pl.GetNextAction() == PlayerState.TurnLeft)
        {
            riRight.enabled = false;
            riLeft.enabled = true;

        }

        if (pl.GetNextAction() == PlayerState.TurnRight)
        {
            riLeft.enabled = false;
            riRight.enabled = true;
        }

        if (pl.GetNextAction() == PlayerState.Idle)
        {
            riRight.enabled = false;
            riLeft.enabled = false;
        }
    }
}
