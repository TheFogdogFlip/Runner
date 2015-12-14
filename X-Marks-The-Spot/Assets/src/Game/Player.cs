using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/**
 * Child of 'PlayerBase' which contains all movement methods.
 */
public class Player : PlayerBase
{
    //Ctd Timer
    private GameObject ctdTimerGameObj;
    private TimerCountdown ctdTimerObj;
    //Player Timer
    private TimerPlayer playerTimerObj;
    private GameObject playerTimerGameObj;
    //Ghost Timer
    private TimerGhost ghostTimerObj;
    private GameObject ghostTimerGameObj;
    //Joystick cooldown
    private int cooldownCount = 25;
    private bool coolingDown = false;

    private List<List<TimeStamp>> ghostinputs;
    private List<GameObject> ghosts;
    Vector2 startPosition = -Vector2.one;
    public List<TimeStamp> inputs;



    /**---------------------------------------------------------------------------------
     * Activated when the player is activated, initiates all the necessities.
     */
    void 
    Start()
    {
        //reset captureframerate so game runs at normal speed afer recordign
        Time.captureFramerate = 0;

        SetupCtdTimer();
        //SetupGhostTimer();
        SetupPlayerTimer();

        ghostinputs = new List<List<TimeStamp>>();
        ghosts = new List<GameObject>();
        inputs = new List<TimeStamp>();
        anim.Play("Idle");
        AudioManager.Instance.startLoop();
    }

    /**---------------------------------------------------------------------------------
     * Metod activated every turn.
     */
    void 
    Update()
    {
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
        }
        if (!ctdTimerObj.TimerSecondRunning)
        {
            ctdTimerObj.HideTimer();
        }
        else
        {
            ctdTimerObj.SetText();
        }
    }

    /**---------------------------------------------------------------------------------
     * Method which does things when stuff is clicked, checked every frame.
     */
    void 
    KeyInputs()
    {
        int horizontal = 0;
        int vertical = 0;

        /**
         * Used on touchpad devices.
         */
        if (Input.touches.Length > 0)
        {
            Touch firstFinger = Input.touches[0];

            if (firstFinger.phase == TouchPhase.Began)
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

        //Temp stuff for easier testing.
        if (Input.GetKeyDown(KeyCode.R))
            GoalFunc();

        /**
         * Cooldown to avoid queuing up unwanted commands.
         */
        if (coolingDown)
        {
            Input.ResetInputAxes();
            cooldownCount -= 1;
        }

        if (cooldownCount == 0)
        {
            coolingDown = false;
            cooldownCount = 15;
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
                AudioManager.Instance.SlideSound();
            }

            if ((Input.GetButtonDown("Jump") || vertical == 1) && !isJumping && !isSliding)
            {
                float time = playerTimerObj.f_time;
                TimeStamp ts = new TimeStamp();
                ts.time = time;
                ts.input = PlayerState.Jump;
                inputs.Add(ts);
                Jump();
                AudioManager.Instance.JumpSound();
            }
        }
    }

    /**---------------------------------------------------------------------------------
     * Sets up the countdown timer.
     */
    public void 
    SetupCtdTimer()
    {
        ctdTimerGameObj = GameObject.Find("ctdTimer");
        ctdTimerObj = ctdTimerGameObj.GetComponent<TimerCountdown>();
        ctdTimerObj.f_time = 2;
        ctdTimerObj.i_time = 2;
        ctdTimerObj.TimerFirstRunning = true;
        ctdTimerObj.TimerSecondRunning = true;
        ctdTimerObj.countDownText.enabled = true;
    }

    /**---------------------------------------------------------------------------------
     * Sets up the playertimer
     */
    public void 
    SetupPlayerTimer()
    {
        playerTimerGameObj = GameObject.Find("playerTimer");
        playerTimerObj = playerTimerGameObj.GetComponent<TimerPlayer>();
        playerTimerObj.TimerRunning = false;
    }

    /**---------------------------------------------------------------------------------
     * Sets up the GHOST.
     */
    public void 
    SetupGhostTimer()
    {
        ghostTimerGameObj = GameObject.Find("ghostTimer");
        ghostTimerObj = ghostTimerGameObj.GetComponent<TimerGhost>();
        ghostTimerObj.f_time = 0;
        ghostTimerObj.TimerRunning = true;

    }

    /**---------------------------------------------------------------------------------
     * Metod for setting up the next game, occurs on death-
     */
    void 
    SetupNextGame()
    {
        GameObject go;
        Ghost ghost;
        ghostinputs.Add(inputs);
        inputs = new List<TimeStamp>();

        /**
         * Activates all saved away ghosts.
         */
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

    /**---------------------------------------------------------------------------------
     * Method overridden to save away the action for the next ghost.
     */
    protected override void 
    ActivateNextAction()
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
            if (nextAction == PlayerState.Slide) { Slide(); AudioManager.Instance.SlideSound(); }
            if (nextAction == PlayerState.Jump) { Jump(); AudioManager.Instance.JumpSound(); }

            nextAction = PlayerState.Idle;
        }
    }

    /**---------------------------------------------------------------------------------
     * Method activeted on death, resetts all necessary values to their correct state.
     */
    protected override void 
    Death()
    {
        if (isSliding)
        {
            bc.center = new Vector3(bc.center.x, 0.2765842f, bc.center.z);
            bc.size = new Vector3(bc.size.x, 2.0f * bc.size.y, bc.size.z);
        }

        isTurning = false;
        isJumping = false;
        isFalling = false;
        isSliding = false;
        crntJumpForce = jumpForce;
        transform.position = World.Instance.StartPosition;
        transform.rotation = Quaternion.Euler(World.Instance.StartDirection);
        rotationTarget = World.Instance.StartDirection.y;
        nextAction = PlayerState.Idle;
        anim.Play("Idle");
        AudioManager.Instance.CollisionSound();

        //Kills off all current ghosts.
        foreach (GameObject go in ghosts)
        {
            Destroy(go);
        }

        SetupNextGame();
    }

    /**---------------------------------------------------------------------------------
     * Pitfalling ovverridden, just for the sound :)
     */
    protected override void 
    Falling()
    {
        AudioManager.Instance.FallingSound();
        anim.SetTrigger("Slide");
    }

    /**---------------------------------------------------------------------------------
     * Method activated when the goal is reached, set's up the finishing camera.
     */
    protected override void 
    GoalFunc()
    {
        AudioManager.Instance.WinSound();
        ctdTimerObj.countDownText.enabled = true;
        ctdTimerObj.countDownText.text = "Victory!";

        Camera cam = Instantiate(Resources.Load("ReplayCamera", typeof(Camera)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection)) as Camera;
        Recorder rec = cam.GetComponent<Recorder>();
        rec.inputs = inputs;
        rec.ghostinputs = ghostinputs;
        rec.finishedTime = playerTimerObj.f_time;

    }

    /**---------------------------------------------------------------------------------
     * Returns wheter or not the character is turning or not.
     */
    public bool 
    GetTurn()
    {
        return isTurning;
    }

    /**---------------------------------------------------------------------------------
     * Returns the players state.
     */
    public PlayerState 
    GetNextAction()
    {
        return nextAction;
    }
}
