using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghost : PlayerBase
{

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
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        ghostTimerGameObj = GameObject.Find("ghostTimer");
        ghostTimerObj = ghostTimerGameObj.GetComponent<Timer_Ghost>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFirstFrame)
        {
            isFirstFrame = false;
            anim = gameObject.GetComponentInChildren<Animator>();
            anim.Play("Run");
        }
        ghostTimerObj.SetText();
        string currEvent = null;
        if (index < inputs.Count)
        {
            if (inputs[index].time <= ghostTimerObj.f_time)
            {
                currEvent = inputs[index].input;
            }
        }
        if (currEvent == "TurnLeft") TurnLeft();
        if (currEvent == "TurnRight") TurnRight();
        if (currEvent == "Jump") Jump();
        if (currEvent == "Slide") Slide();
        if (currEvent != null) index++;
        MovementUpdate();
    }
}
