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

    //For Animation
    public Animator anim;
    private bool isFirstFrame = true;

    void Awake ()
    {
        SetupCtdTimer();
        //SetupGhostTimer();
        SetupPlayerTimer();
        crntSpeed = runSpeed;
	}

    void Start ()
    {
        ghostinputs = new List<List<TimeStamp>>();
        ghosts = new List<GameObject>();
        inputs = new List<TimeStamp>();
    }

    void Update()
    {
        if (isFirstFrame)
        {
            isFirstFrame = false;
            anim = gameObject.GetComponentInChildren<Animator>();
        }
        if (ghostTimerObj != null)
            ghostTimerObj.SetText();
        
        //Running forward block
        if (!ctdTimerObj.TimerFirstRunning)
        {
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

            KeyInputs();
            MovementUpdate();
            

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
        {
            Instantiate(Resources.Load("ReplayCamera", typeof(Camera)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection));
        }


        float time = playerTimerObj.f_time;
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            TimeStamp ts = new TimeStamp();
            ts.time = time;
            ts.input = "TurnRight";
            inputs.Add(ts);
            anim.Play("TurnRight90");
            TurnRight();
        }

        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            TimeStamp ts = new TimeStamp();
            ts.time = time;
            ts.input = "TurnLeft";
            inputs.Add(ts);
            anim.Play("TurnLeft90");
            TurnLeft();
        }

        if (Input.GetButtonDown("Slide") && !isSliding && !isJumping)
        {
            TimeStamp ts = new TimeStamp();
            ts.time = playerTimerObj.f_time;
            ts.input = "Slide";
            inputs.Add(ts);
            Slide();
        }

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            TimeStamp ts = new TimeStamp();
            ts.time = playerTimerObj.f_time;
            ts.input = "Jump";
            inputs.Add(ts);
            Jump();
        }
    }

    void SetupCtdTimer()
    {
        ctdTimerGameObj = GameObject.Find("ctdTimer");
        ctdTimerObj = ctdTimerGameObj.GetComponent<Timer_Countdown>();
        ctdTimerObj.f_time = 2;
        ctdTimerObj.i_time = 2;
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
        crntSlideLength = maxSlideLength;
        transform.position = World.Instance.StartPosition;
        transform.rotation = Quaternion.Euler(World.Instance.StartDirection);
        rotationTarget = transform.rotation.y;
        

        foreach (GameObject go in ghosts)
        {
            Destroy(go);
        }

        SetupNextGame();
    }
}
