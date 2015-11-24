using UnityEngine;
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


    void Start ()
    {
        SetupCtdTimer();
        //SetupGhostTimer();
        SetupPlayerTimer();
        crntSpeed = runSpeed;

        ghostinputs = new List<List<TimeStamp>>();
        ghosts = new List<GameObject>();
        inputs = new List<TimeStamp>();
    }

    void Update()
    {
        if (ghostTimerObj != null)
            ghostTimerObj.SetText();

        //Running forward block
        if (!ctdTimerObj.TimerFirstRunning)
        {
            MovementUpdate();
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

            Vector3 tempVec = transform.position;
            if (tempVec.z < 0) tempVec.z *= -1;
            if (tempVec.x < 0) tempVec.x *= -1;
            if (rotationTarget == 0)
            {
                if (tempVec.z % 2 <= 1)
                {
                    isActionActive = false;
                }
                else if (tempVec.z % 2 >= 1 && !isActionActive)
                {
                    ActivateNextAction();
                    isActionActive = true;
                }
            }
            else if (rotationTarget == 90 || rotationTarget == -270)
            {
                if (tempVec.x % 2 <= 1)
                {
                    isActionActive = false;
                }
                else if (tempVec.x % 2 >= 1 && !isActionActive)
                {
                    ActivateNextAction();
                    isActionActive = true;
                }
            }
            else if (rotationTarget == 180 || rotationTarget == -180)
            {
                if (tempVec.z % 2 >= 1)
                {
                    isActionActive = false;
                }
                else if (tempVec.z % 2 <= 1 && !isActionActive)
                {
                    ActivateNextAction();
                    isActionActive = true;
                }
            }
            else if (rotationTarget == 270 || rotationTarget == -90)
            {
                if (tempVec.x % 2 >= 1)
                {
                    isActionActive = false;
                }
                else if (tempVec.x % 2 <= 1 && !isActionActive)
                {
                    ActivateNextAction();
                    isActionActive = true;
                }
            }
            KeyInputs();
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
       
        if (Input.GetButtonDown("Right"))
        {
            SetNextAction("TurnRight");
        }

        if (Input.GetButtonDown("Left"))
        {
            SetNextAction("TurnLeft");
        }

        if (Input.GetButtonDown("Slide") && !isSliding && !isJumping)
        {
            SetNextAction("Slide");
        }

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            SetNextAction("Jump");
        }
    }

    void SetupCtdTimer()
    {
        ctdTimerGameObj = GameObject.Find("ctdTimer");
        ctdTimerObj = ctdTimerGameObj.GetComponent<Timer_Countdown>();
        ctdTimerObj.f_time = 3;
        ctdTimerObj.i_time = 3;
        ctdTimerObj.TimerFirstRunning = true;
        ctdTimerObj.TimerSecondRunning = true;
        ctdTimerObj.textObj.enabled = true;
    }

    void SetupPlayerTimer()
    {
        playerTimerGameObj = GameObject.Find("playerTimer");
        playerTimerObj = playerTimerGameObj.GetComponent<Timer_Player>();
        playerTimerObj.textObj.text = "0";
        playerTimerObj.TimerRunning = false;
    }

    void SetupGhostTimer()
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
            if (nextAction == "Jump") Jump();
            if (nextAction == "Slide") Slide();

            nextAction = "";
        }
    }

    protected override void Death()
    {
        if (isSliding)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);
        }

        isJumping = false;
        isFalling = false;
        isSliding = false;
        turnPhase = 0;
        crntSpeed = runSpeed;
        transform.position = World.Instance.StartPosition;
        transform.rotation = Quaternion.Euler(World.Instance.StartDirection);
        rotationTarget = transform.rotation.y;
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
    }
    public int GetTurn()
    {
        return turnPhase;
    }
}
