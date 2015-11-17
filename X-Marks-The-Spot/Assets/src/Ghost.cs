﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghost : MonoBehaviour {

    //For moving forward
    public float runSpeed;
    public float crntSpeed;
    public float deceleration;
    public float acceleration;

    //Jumping
    private bool isJumping = false;
    private bool isFalling = false;
    public float jumpSpeed;
    public float jumpHeight;

    //Sliding
    private bool isSliding = false;
    public float slideSpeed;
    public float maxSlideLength;
    private float crntSlideLength;
    public BoxCollider bc;

    //For turning 90 degrees smoothly
    public float turnSpeed = 55.0f;
    private float rotationTarget;
    private Quaternion qTo = Quaternion.identity;
    private int turnPhase = 0;

    //For Animation
    public Animator anim;
    private bool isFirstFrame = true;

    public List<TimeStamp> inputs;
    private Timer_Ghost ghostTimerObj;
    private GameObject ghostTimerGameObj;
    int index;

    // Use this for initialization
    void Start () 
    {
        index = 0;
        rotationTarget = 0;
        crntSpeed = runSpeed;
        ghostTimerGameObj = GameObject.Find("ghostTimer");
        ghostTimerObj = ghostTimerGameObj.GetComponent<Timer_Ghost>();
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (isFirstFrame)
        {
            isFirstFrame = false;
            anim = gameObject.GetComponentInChildren<Animator>();
            //anim = GameObject.Find("Ghost(Clone)/NinjaBob").GetComponent<Animator>();
            anim.Play("Run");
        }
        ghostTimerObj.SetText();
        EventHandler();
        Run();
        Falling();
    }

    void EventHandler()
    {
        if(index < inputs.Count)
        {
            if (inputs[index].time <= ghostTimerObj.f_time)
            {
                if (inputs[index].input == "TurnRight")
                {
                    rotationTarget += 90.0f;
                    anim.Play("TurnRight90");
                    turnPhase = 1;
                    //TurnRight();
                }

                else if (inputs[index].input == "TurnLeft")
                {
                    rotationTarget -= 90.0f;
                    anim.Play("TurnLeft90");
                    turnPhase = 1;
                    //TurnLeft();
                }


                else if (inputs[index].input == "Jump")
                {
                    Jump();
                }

                else if (inputs[index].input == "Slide")
                {
                    Slide();
                }
                index++;
            }
            
        }
        if (rotationTarget == 360 || rotationTarget == -360)
        {
            rotationTarget = 0.0f;
        }

        qTo = Quaternion.Euler(0.0f, rotationTarget, 0.0f);
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

    void TurnRight()
    {
        rotationTarget += 90.0f;
        anim.Play("TurnRight90");
        turnPhase = 1;
        TurnPhase(turnPhase);
    }

    void TurnLeft()
    {
        rotationTarget -= 90.0f;
        anim.Play("TurnLeft90");
        turnPhase = 1;
        TurnPhase(turnPhase);
    }
       
    void TurnPhase(int turnPhase)
    {
        if (rotationTarget == 360 || rotationTarget == -360)
        {
            rotationTarget = 0.0f;
        }

        qTo = Quaternion.Euler(0.0f, rotationTarget, 0.0f);
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
        if (isSliding)
        {
            //bc.size += new Vector3(0, bc.size.y, 0);
            transform.localScale = new Vector3(transform.localScale.x, bc.size.y * 2, transform.localScale.z);
            crntSlideLength = maxSlideLength;
            isSliding = false;
        }
       
       //Going up
       if (!isFalling)
       {
           transform.Translate(new Vector3(0, jumpSpeed, 0));
           if (transform.position.y >= jumpHeight)
           {
               isFalling = true;
           }
       }
    }

    void Falling()
    {
        if (isFalling)
        {
            transform.Translate(new Vector3(0, -jumpSpeed, 0));
        }
    }

    void Slide()
    {
        //Slide start
        isSliding = true;
        //bc.size += new Vector3(0, -(bc.size.y * 0.5f), 0);
        transform.localScale = new Vector3(transform.localScale.x, bc.size.y * 0.5f, transform.localScale.z);
        
        //Slide end
        if (crntSlideLength <= 0)
        {
            //bc.size += new Vector3(0, bc.size.y, 0);
            transform.localScale = new Vector3(transform.localScale.x, bc.size.y * 2, transform.localScale.z);
            crntSlideLength = maxSlideLength;
            isSliding = false;
        }
        else
        {
            crntSlideLength -= slideSpeed * Time.deltaTime;
        }
        
    }

    void Run()
    {
        //Run forward
        transform.Translate(transform.forward * crntSpeed * 100 * Time.deltaTime, Space.World);
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
