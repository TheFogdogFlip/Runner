﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player : PlayerBase
{

    //Ctd Timer
    private GameObject ctdTimerGameObj;
    private Timer_Countdown ctdTimerObj;
    
    //Player Timer
    private Timer_Player playerTimerObj;
    private GameObject playerTimerGameObj;

    public List<TimeStamp> inputs;

    //Ghost Timer -- TEMPORARY, just needed somewhere to make it 
    private Timer_Ghost ghostTimerObj;
    private GameObject ghostTimerGameObj;

    private List<List<TimeStamp>> ghostinputs;
    private List<GameObject> ghosts;

    private string nextAction = "";
    private bool isActionActive = false;

    //Turn finetuning
    private float turnDelay = 0.1f;

    //Joystick cooldown
    private int cooldownCount = 25;
    private bool coolingDown = false;

    //Audio
    private AudioManager sound;

    void Start ()
    {
        SetupCtdTimer();
        //SetupGhostTimer();
        SetupPlayerTimer();

        ghostinputs = new List<List<TimeStamp>>();
        ghosts = new List<GameObject>();
        inputs = new List<TimeStamp>();
        sound = new AudioManager();
    }

    void Update()
    {
        if (ghostTimerObj != null)
            ghostTimerObj.SetText();

        //Running forward block
        if (!ctdTimerObj.TimerFirstRunning)
        {
            MovementUpdate();
            KeyInputs();
            if (!playerTimerObj.TimerRunning)
            {
                playerTimerObj.TimerRunning = true;
                playerTimerObj.f_time = 0;
                anim.Play("Run");
            }

            else
            {
                playerTimerObj.SetText();
            }

            if (turnPhase == 0 || turnPhase == 2)
            {
                Vector3 tempVec = transform.position;
                if (tempVec.z < 0) tempVec.z *= -1;
                if (tempVec.x < 0) tempVec.x *= -1;

                if (rotationTarget <= 1 && rotationTarget >= -1)
                {
                    rotationTarget = 0;
                }
                    
                if (rotationTarget == 0)
                {

                    if (tempVec.z % 2 <= 1 && !isJumping && !isSliding)
                    {
                        isActionActive = false;
                    }

                    if (tempVec.z % 2 >= 1 && !isActionActive)
                    {
                        ActivateNextAction();
                        isActionActive = true;
                    }
                }
                if (rotationTarget == 90 || rotationTarget == -270)
                {
                    if (tempVec.x % 2 <= 1 && !isJumping && !isSliding)
                    {
                        isActionActive = false;
                    }

                    if (tempVec.x % 2 >= 1 && !isActionActive)
                    {
                        ActivateNextAction();
                        isActionActive = true;
                    }
                }
                if (rotationTarget == 180 || rotationTarget == -180)
                {

                    if (tempVec.z % 2 >= 1 && !isJumping && !isSliding)
                    {
                        isActionActive = false;
                    }

                    if (tempVec.z % 2 < 1 && !isActionActive)
                    {
                        ActivateNextAction();
                        isActionActive = true;
                    }
                }
                if (rotationTarget == 270 || rotationTarget == -90)
                {

                    if (tempVec.x % 2 >= 1 && !isJumping && !isSliding)
                    {
                        isActionActive = false;
                    }
                    if (tempVec.x % 2 <= 1 && !isActionActive)
                    {
                        ActivateNextAction();
                        isActionActive = true;
                    }
                }
            }      
        }
        if(!ctdTimerObj.TimerSecondRunning)
        {
            ctdTimerObj.HideTimer();
        }
        else
        {
            ctdTimerObj.SetText();
        }
    }

    void KeyInputs()
    {

        if (Input.GetKeyDown(KeyCode.R))
            GoalFunc();

        if (coolingDown)
        {
            Input.ResetInputAxes();
            cooldownCount -= 1;
        }

        if (cooldownCount == 0)
        {
            coolingDown = false;
            cooldownCount = 25;
        }
        else
        {
            if (Input.GetButtonDown("Right") || Input.GetAxisRaw("Horizontal") == 1)
            {
                SetNextAction("TurnRight");
                coolingDown = true;

            }

            if (Input.GetButtonDown("Left") || Input.GetAxisRaw("Horizontal") == -1)
            {
                SetNextAction("TurnLeft");
                coolingDown = true;

            }
        }

        if (turnPhase == 0 || turnPhase == 2)
        {
            if (Input.GetButtonDown("Slide") && !isSliding && !isJumping)
            {
                Slide();
                float time = playerTimerObj.f_time;
                TimeStamp ts = new TimeStamp();
                ts.time = time;
                ts.input = "Slide";
                inputs.Add(ts);
                sound.SlideSound();
            }

            if (Input.GetButtonDown("Jump") && !isJumping && !isSliding)
            {
                Jump();
                float time = playerTimerObj.f_time;
                TimeStamp ts = new TimeStamp();
                ts.time = time;
                ts.input = "Jump";
                inputs.Add(ts);
                sound.JumpSound();
            }
            if (Input.GetButtonUp("Jump") && isJumping && !isSliding)
            {
                isFalling = true;
                float time = playerTimerObj.f_time;
                TimeStamp ts = new TimeStamp();
                ts.time = time;
                ts.input = "Fall";
                inputs.Add(ts);
            }
        }
    }

    public void SetupCtdTimer()
    {
        ctdTimerGameObj = GameObject.Find("ctdTimer");
        ctdTimerObj = ctdTimerGameObj.GetComponent<Timer_Countdown>();
        ctdTimerObj.f_time = 2;
        ctdTimerObj.i_time = 2;
        ctdTimerObj.TimerFirstRunning = true;
        ctdTimerObj.TimerSecondRunning = true;
        ctdTimerObj.textObj.enabled = true;
    }

    public void SetupPlayerTimer()
    {
        playerTimerGameObj = GameObject.Find("playerTimer");
        playerTimerObj = playerTimerGameObj.GetComponent<Timer_Player>();
        playerTimerObj.textObj.text = "0";
        playerTimerObj.TimerRunning = false;
    }

    public void SetupGhostTimer()
    {
        ghostTimerGameObj = GameObject.Find("ghostTimer");
        ghostTimerObj = ghostTimerGameObj.GetComponent<Timer_Ghost>();
        ghostTimerObj.f_time = 0;
        ghostTimerObj.textObj.text = "0";
        ghostTimerObj.TimerRunning = true;

    }

    void SetupNextGame()
    {
        GameObject go;
        Ghost ghost;
        ghostinputs.Add(inputs);
        inputs = new List<TimeStamp>();

        for (int i = 0; i < ghostinputs.Count; ++i)
        {
            go = Instantiate(Resources.Load("Ghost", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection)) as GameObject;
            ghost = go.GetComponent<Ghost>();
            ghost.inputs = ghostinputs[i];
            ghosts.Add(go);

        }

        SetupGhostTimer();
        SetupCtdTimer();
        SetupPlayerTimer();
    }

    void SetNextAction(string input)
    {
        nextAction = input;
    }

    void ActivateNextAction()
    {
        if (nextAction != "")
        {
            float time = playerTimerObj.f_time;
            TimeStamp ts = new TimeStamp();
            ts.time = time;
            ts.input = nextAction;
            inputs.Add(ts);

            if (nextAction == "TurnLeft") TurnLeft();
            if (nextAction == "TurnRight") TurnRight();

            nextAction = "";
        }
    }

    protected override void Death()
    {
        if (isSliding)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);
        }

        turnRotation = false;
        isJumping = false;
        isFalling = false;
        isSliding = false;
        turnPhase = 0;
        crntSpeed = runSpeed;
        transform.position = World.Instance.StartPosition;
        transform.rotation = Quaternion.Euler(World.Instance.StartDirection);
        rotationTarget = World.Instance.StartDirection.y;
        nextAction = "";

        foreach (GameObject go in ghosts)
        {
            Destroy(go);
        }

        SetupNextGame();
    }
    protected override void GoalFunc()
    {
        ctdTimerObj.textObj.enabled = true;
        ctdTimerObj.textObj.text = "Victory!";

        Camera cam = Instantiate(Resources.Load("ReplayCamera", typeof(Camera)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection)) as Camera;
        Recorder rec = cam.GetComponent<Recorder>();
        rec.inputs = inputs;
        rec.ghostinputs = ghostinputs;
        rec.finishedTime = playerTimerObj.f_time;

    }
    public int GetTurn()
    {
        return turnPhase;
    }

    public string GetNextAction()
    {
        return nextAction;
    }
}
