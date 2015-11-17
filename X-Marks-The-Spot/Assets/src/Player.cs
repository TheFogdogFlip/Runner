using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    //For moving forward
    public float runSpeed;
    public float crntSpeed;
    public float deceleration;
    public float acceleration;

    //For jumping
    private bool isJumping = false;
    private bool isFalling = false;
    public float jumpSpeed;
    public float jumpHeight;
    public Rigidbody rb;

    //For turning 90 degrees smoothly
    public float turnSpeed = 55.0f;
    public float rotationTarget;
    private Quaternion qTo = Quaternion.identity;
    private int turnPhase = 0;

    //For sliding
    private bool isSliding = false;
    public float slideSpeed;
    public float maxSlideLength;
    private float crntSlideLength;
    public BoxCollider bc;

    //Ctd Timer
    private GameObject ctdTimerGameObj;
    private Timer_Countdown ctdTimerObj;
    
    //Player Timer
    private Timer_Player playerTimerObj;
    private GameObject playerTimerGameObj;

    //For Animation
    public Animator anim;
    private bool isFirstFrame = true;


    //Ghost Timer -- TEMPORARY, just needed somewhere to make it 
    private Timer_Ghost ghostTimerObj;
    private GameObject ghostTimerGameObj;

    void Awake ()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        crntSlideLength = maxSlideLength;
        rotationTarget = transform.rotation.y;

        SetupCtdTimer();
        SetupPlayerTimer();
        SetupGhostTimer();
        crntSpeed = runSpeed;
	}

    void Start ()
    {

    }

    void Update()
    {
        if (isFirstFrame)
        {
            isFirstFrame = false;
            anim = GameObject.Find("NinjaBob").GetComponent<Animator>();
        }
        if (!ghostTimerObj.TimerRunning)
        {
            ghostTimerObj.TimerRunning = true;
            ghostTimerObj.f_time = 0;
        }
        else
        {
            ghostTimerObj.SetText();
        }
        //Running forward block
        if (!ctdTimerObj.TimerFirstRunning)
        {
            if (!playerTimerObj.TimerRunning)
            {
                playerTimerObj.TimerRunning = true;
                playerTimerObj.f_time = 0;
            }

            else
            {
                playerTimerObj.SetText();
            }

            Turn();
            //Running forward block
            Run();
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

    void SetupCtdTimer()
    {
        ctdTimerGameObj = GameObject.Find("ctdTimer");
        ctdTimerObj = ctdTimerGameObj.GetComponent<Timer_Countdown>();
        ctdTimerObj.f_time = 4;
        ctdTimerObj.i_time = 4;
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
        ghostTimerObj.textObj.text = "0";
        ghostTimerObj.TimerRunning = false;
    }
    void LateUpdate()
    {
        //Do this late because we want to check collisions first
        Jump();
        Slide();
    }

    void FixedUpdate()
    {
        
    }

    void Run()
    {
        //Run forward
        transform.Translate(transform.forward * crntSpeed, Space.World);
    }

    void Turn()
    {
        
        //ACTIVATION PHASE
        if (turnPhase == 0)
        {

            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                rotationTarget += 90.0f;
                anim.Play("TurnRight90");
                turnPhase = 1;
            }
            if (Input.GetAxisRaw("Horizontal") == -1)
            {
                rotationTarget -= 90.0f;
                anim.Play("TurnLeft90");
                turnPhase = 1;
            }
            if (rotationTarget == 360 || rotationTarget == -360)
            {
                rotationTarget = 0.0f;
            }
            qTo = Quaternion.Euler(0.0f, rotationTarget, 0.0f);
        }
        //BRAKING PHASE
        if (turnPhase == 1)
        {
            crntSpeed -= deceleration;

            if (crntSpeed <= 0.01)
            {
                turnPhase = 2;
            }
        }

        //TURNING PHASE
        if (turnPhase == 2)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, turnSpeed * Time.deltaTime);

            if (transform.rotation == qTo)
            {
                turnPhase = 3;
            }
        }
        //AXELERATION PHASE
        if (turnPhase == 3)
        {
            crntSpeed += acceleration;
            if (crntSpeed >= runSpeed)
            {
                crntSpeed = runSpeed;
                turnPhase = 0;
            }
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            if (isSliding)
            {
                bc.size += new Vector3(0, bc.size.y, 0);
                crntSlideLength = maxSlideLength;
                isSliding = false;
            }

            isJumping = true;
        }
        if (isJumping)
        {
            //Going up
            if (!isFalling)
            {
                transform.Translate(new Vector3(0, jumpSpeed, 0));
                if (transform.position.y >= jumpHeight)
                {
                    isFalling = true;
                }
            }
            //Going down
            else if (isFalling)
            {
                transform.Translate(new Vector3(0, -jumpSpeed, 0));
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isJumping = false;
            isFalling = false;
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            Death();
        }
    }

    void Slide()
    {
        if (Input.GetButtonDown("Slide") && !isSliding && !isJumping)
        {
            //Slide start
            isSliding = true;
            bc.size += new Vector3(0, -(bc.size.y * 0.5f), 0);
            
        }

        if (isSliding)
        {
            //Slide end
            if (crntSlideLength <= 0)
            {
                bc.size += new Vector3(0, bc.size.y, 0);
                crntSlideLength = maxSlideLength;
                isSliding = false;
            }
            else
            {
                crntSlideLength -= slideSpeed * Time.deltaTime;
            }
        }
    }

    void Death()
    {
        isJumping = false;
        isFalling = false;
        isSliding = false;
        turnPhase = 0;
        crntSpeed = runSpeed;
        crntSlideLength = maxSlideLength;
        transform.position = World.Instance.StartPosition;
        transform.rotation = Quaternion.Euler(World.Instance.StartDirection);
        rotationTarget = transform.rotation.y;
    }
}
