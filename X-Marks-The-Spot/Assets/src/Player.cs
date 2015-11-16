using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    //For moving forward
    public float runSpeed;

    //For jumping
    private bool isJumping = false;
    private bool isFalling = false;
    public float jumpSpeed;
    public float jumpHeight;
    public Rigidbody rb;

    //For turning 90 degrees smoothly
    public float turnSpeed = 55.0f;
    private float rotationTarget = 0.0f;
    private Quaternion qTo = Quaternion.identity;
    private float lastY = 0f;

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

    //Ghost Timer -- TEMPORARY, just needed somewhere to make it 
    private Timer_Ghost ghostTimerObj;
    private GameObject ghostTimerGameObj;

    void Awake ()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        crntSlideLength = maxSlideLength;

        SetupCtdTimer();
        SetupPlayerTimer();
        SetupGhostTimer();
	}

    void Update()
    {
        if(!ghostTimerObj.TimerRunning)
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

            Run();

            //Turn block
            if (lastY == transform.rotation.eulerAngles.y)
            {
                Turn();
            }
            lastY = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, turnSpeed * Time.deltaTime);
        }
        else if(!ctdTimerObj.TimerSecondRunning)
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
        transform.Translate(transform.forward * runSpeed, Space.World);
    }

    void Turn()
    {
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            rotationTarget += 90.0f;
        }
        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            rotationTarget -= 90.0f;
        }
        if (rotationTarget == 360 || rotationTarget == -360)
        {
            rotationTarget = 0.0f;
        }
        qTo = Quaternion.Euler(0.0f, rotationTarget, 0.0f);
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
}
