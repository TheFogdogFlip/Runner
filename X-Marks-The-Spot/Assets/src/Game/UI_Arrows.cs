using UnityEngine;
using UnityEngine.UI;

public class UI_Arrows : MonoBehaviour {

    RawImage riLeft;
    RawImage riRight;
    RawImage riUp;
    RawImage riDown;
    Player pl;
    bool isFirstFrame;

	void Start ()
    {
        isFirstFrame = true;
        riLeft = GameObject.Find("UI_LeftArrow").GetComponent<RawImage>();
        riRight = GameObject.Find("UI_RightArrow").GetComponent<RawImage>();
        riUp = GameObject.Find("UI_UpArrow").GetComponent<RawImage>();
        riDown = GameObject.Find("UI_DownArrow").GetComponent<RawImage>();
    }
	
	
	void Update ()
    {
        if (isFirstFrame)
        {
            pl = GameObject.Find("Player(Clone)").GetComponent<Player>();
            isFirstFrame = false;
        }
        switch(pl.GetNextAction())
        {
            case PlayerState.TurnLeft:
                riLeft.enabled = true;
                riRight.enabled = false;
                riDown.enabled = false;
                riUp.enabled = false;
                break;
            case PlayerState.TurnRight:
                riRight.enabled = true;
                riLeft.enabled = false;
                riDown.enabled = false;
                riUp.enabled = false;
                break;
            case PlayerState.Jump:
                riUp.enabled = true;
                riRight.enabled = false;
                riDown.enabled = false;
                riLeft.enabled = false;
                break;
            case PlayerState.Slide:
                riDown.enabled = true;
                riRight.enabled = false;
                riLeft.enabled = false;
                riUp.enabled = false;
                break;

            case PlayerState.Idle:
                riRight.enabled = false;
                riLeft.enabled = false;
                riDown.enabled = false;
                riUp.enabled = false;
                break;
        }
    }
}
