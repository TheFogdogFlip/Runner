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
        sound.InitAudio();
        anim.Play("Idle");
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
                anim.SetTrigger("Run");
            }

            else
            {
                playerTimerObj.SetText();
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

    Vector2 startPosition = -Vector2.one;

    void KeyInputs()
    {
        int horizontal = 0;
        int vertical = 0;

        if(Input.touches.Length > 0)
        {
            Touch firstFinger = Input.touches[0];

            if(firstFinger.phase == TouchPhase.Began)
                startPosition = firstFinger.position;
            else if (firstFinger.phase == TouchPhase.Ended && startPosition != -Vector2.one)
            {
                Vector2 endPosition = firstFinger.position;

                float x = endPosition.x - startPosition.x;
                float y = endPosition.y - startPosition.y;

                startPosition = -Vector2.one;

                if (Mathf.Abs(x) > Mathf.Abs(y))
                    horizontal = x > 0 ? 1 : -1;
                else
                    vertical = y > 0 ? 1 : -1;
            }
        }

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
            if (Input.GetButtonDown("Right") || Input.GetAxisRaw("Horizontal") == 1 || horizontal == 1)
            {
                SetNextAction(PlayerState.TurnRight);
                coolingDown = true;
            }

            if (Input.GetButtonDown("Left") || Input.GetAxisRaw("Horizontal") == -1 || horizontal == -1)
            {
                SetNextAction(PlayerState.TurnLeft);
                coolingDown = true;
            }
            if (isJumpLocked)
            {
                if ((Input.GetButtonDown("Slide") || vertical == -1) && !isSliding && !isJumping)
                {
                    SetNextAction(PlayerState.Slide);
                }

                if ((Input.GetButtonDown("Jump") || vertical == 1) && !isJumping && !isSliding)
                {
                    SetNextAction(PlayerState.Jump);
                }
            }
        }

        if (!isJumpLocked)
        {
            if ((Input.GetButtonDown("Slide") || vertical == -1) && !isSliding && !isJumping)
            {
                float time = playerTimerObj.f_time;
                TimeStamp ts = new TimeStamp();
                ts.time = time;
                ts.input = PlayerState.Slide;
                inputs.Add(ts);
                Slide();
                sound.SlideSound();
            }

            if ((Input.GetButtonDown("Jump") || vertical == 1) && !isJumping && !isSliding)
            {
                float time = playerTimerObj.f_time;
                TimeStamp ts = new TimeStamp();
                ts.time = time;
                ts.input = PlayerState.Jump;
                inputs.Add(ts);
                Jump();
                sound.JumpSound();
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

    protected override void ActivateNextAction()
    {
        if (nextAction != PlayerState.Idle)
        {
            float time = playerTimerObj.f_time;
            TimeStamp ts = new TimeStamp();
            ts.time = time;
            ts.input = nextAction;
            inputs.Add(ts);

            if (nextAction == PlayerState.TurnLeft) TurnLeft();
            if (nextAction == PlayerState.TurnRight) TurnRight();
            if (nextAction == PlayerState.Slide) { Slide(); sound.SlideSound(); }
            if (nextAction == PlayerState.Jump) { Jump(); sound.JumpSound(); }

            nextAction = PlayerState.Idle;
        }
    }

    protected override void Death()
    {
        if (isSliding)
        {
            bc.size = new Vector3(2.0f * bc.size.x, 2.0f * bc.size.y, 2.0f * bc.size.z);
        }

        turnRotation = false;
        isJumping = false;
        isFalling = false;
        isSliding = false;
        turnPhase = 0;
        crntSpeed = runSpeed;
        crntJumpForce = jumpForce;
        transform.position = World.Instance.StartPosition;
        transform.rotation = Quaternion.Euler(World.Instance.StartDirection);
        rotationTarget = World.Instance.StartDirection.y;
        nextAction = PlayerState.Idle;
        anim.Play("Idle");
        sound.CollisionSound();

        foreach (GameObject go in ghosts)
        {
            Destroy(go);
        }

        SetupNextGame();
    }
    protected override void Falling()
    {
        sound.FallingSound();
    }
    protected override void GoalFunc()
    {
        sound.WinSound();
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

    public PlayerState GetNextAction()
    {
        return nextAction;
    }
}
