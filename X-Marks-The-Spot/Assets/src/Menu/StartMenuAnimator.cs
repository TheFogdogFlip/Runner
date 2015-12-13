using UnityEngine;
using System.Collections;

public class StartMenuAnimator : MonoBehaviour 
{  
    private Animator anim;
    private Rigidbody rb;
    private BoxCollider bc;

    private GameObject TimerGameObject;
    private Timer_Menu TimerObj;

    private bool firstAnimation;
    private bool secondAnimation;

	// Use this for initialization
	void Start () 
    {
        TimerGameObject = GameObject.Find("menuTimer");
        TimerObj = TimerGameObject.GetComponent<Timer_Menu>();

        anim = gameObject.GetComponentInChildren<Animator>();
        anim.Play("Idle");
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}
}
