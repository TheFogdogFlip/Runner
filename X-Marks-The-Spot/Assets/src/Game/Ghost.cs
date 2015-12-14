using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghost : PlayerBase
{

    //For Animation

    public List<TimeStamp> inputs;
    private TimerGhost ghostTimerObj;
    private GameObject ghostTimerGameObj;
    private float invulnerabilityTime = 1f;
    int index;

    /**---------------------------------------------------------------------------------
     * 
     */
    void Start () 
    {
        index = 0;
        rotationTarget = World.Instance.StartDirection.y;
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        ghostTimerGameObj = GameObject.Find("ghostTimer");
        ghostTimerObj = ghostTimerGameObj.GetComponent<TimerGhost>();
    }

    /**---------------------------------------------------------------------------------
     * 
     */
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
                    if (index == inputs.Count -1) isInvulnerable = true;
                    SetNextAction(PlayerState.Jump);
                    break;
                case PlayerState.Slide:
                    if (index == inputs.Count -1) isInvulnerable = true;
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
                    if (index == inputs.Count-1) isInvulnerable = true;
                    Jump();
                    break;
                case PlayerState.Slide:
                    if (index == inputs.Count-1) isInvulnerable = true;
                    Slide();
                    break;
            }
        }

        if (isInvulnerable) invulnerabilityTime -= Time.deltaTime;
        if (invulnerabilityTime < 0) isInvulnerable = false; invulnerabilityTime = 1f;

        if (currEvent != PlayerState.Idle)
        {
            index++;
        }

        MovementUpdate();
    }
}
