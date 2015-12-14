using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghost : PlayerBase
{

    /**---------------------------------------------------------------------------------
     * Various variables for ghosts
     */
    public List<TimeStamp> inputs;
    private TimerGhost ghostTimerObj;
    private GameObject ghostTimerGameObj;
    private float invulnerabilityTime = 1f;
    int index; //current ghost action index to check with timer (action could for example be jump)

    /**---------------------------------------------------------------------------------
     * Exectues at start
     */
    void 
    Start () 
    {
        index = 0;
        rotationTarget = World.Instance.StartDirection.y;
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        ghostTimerGameObj = GameObject.Find("ghostTimer");
        ghostTimerObj = ghostTimerGameObj.GetComponent<TimerGhost>();
    }

    /**---------------------------------------------------------------------------------
     * Executed every frame. Handles ghost movement and actions
     */
    void 
    Update()
    {
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
                    if (index != inputs.Count-1) isInvulnerableJ = true;
                    SetNextAction(PlayerState.Jump);
                    break;
                case PlayerState.Slide:
                    if (index != inputs.Count-1) isInvulnerableS = true;
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
                    if (index != inputs.Count-1) isInvulnerableJ = true;
                    Jump();
                    break;
                case PlayerState.Slide:
                    if (index != inputs.Count-1) isInvulnerableS = true;
                    Slide();
                    break;
            }
        }

        if (isInvulnerableJ || isInvulnerableS) invulnerabilityTime -= Time.deltaTime;
        if (invulnerabilityTime < 0)
        {
            isInvulnerableS = false;
            isInvulnerableJ = false; 
            invulnerabilityTime = 1f;
        }

        if (currEvent != PlayerState.Idle)
        {
            index++;
        }

        MovementUpdate();
    }
}
