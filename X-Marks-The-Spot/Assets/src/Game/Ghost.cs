using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghost : PlayerBase
{

    //For Animation

    public List<TimeStamp> inputs;
    private Timer_Ghost ghostTimerObj;
    private GameObject ghostTimerGameObj;
    int index;

    // Use this for initialization
    void Start () 
    {
        index = 0;
        rotationTarget = World.Instance.StartDirection.y;
        crntSpeed = runSpeed;
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        ghostTimerGameObj = GameObject.Find("ghostTimer");
        ghostTimerObj = ghostTimerGameObj.GetComponent<Timer_Ghost>();
    }

    // Update is called once per frame
    void Update()
    {
        ghostTimerObj.SetText();
        PlayerState currEvent = PlayerState.Idle;
        if (index < inputs.Count)
        {
            if (inputs[index].time <= ghostTimerObj.f_time)
            {
                currEvent = inputs[index].input;
            }
        }

        if (isJumpLocked)
        {
            switch (currEvent)
            {
                case PlayerState.TurnRight:
                    SetNextAction(PlayerState.TurnRight);
                    break;
                case PlayerState.TurnLeft:
                    SetNextAction(PlayerState.TurnLeft);
                    break;
                case PlayerState.Jump:
                    SetNextAction(PlayerState.Jump);
                    break;
                case PlayerState.Slide:
                    SetNextAction(PlayerState.Slide);
                    break;
            }
        }
        else
        {
            switch (currEvent)
            {
                case PlayerState.TurnRight:
                    SetNextAction(PlayerState.TurnRight);
                    break;
                case PlayerState.TurnLeft:
                    SetNextAction(PlayerState.TurnLeft);
                    break;
                case PlayerState.Jump:
                    Jump();
                    break;
                case PlayerState.Slide:
                    Slide();
                    break;
            }
        }

        if(currEvent != PlayerState.Idle)
            index++;

        MovementUpdate();
    }
}
