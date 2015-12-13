using UnityEngine;
using System.Collections;

public class StartMenuAnimator : MonoBehaviour 
{  
    private Animator anim;
    private Rigidbody rb;
    private BoxCollider bc;

    private GameObject TimerGameObject;
    private TimerMenu TimerObj;

    private bool firstAnimation;
    private bool secondAnimation;

    /**---------------------------------------------------------------------------------
     * 
     */
	void 
    Start() 
    {
        TimerGameObject         = GameObject.Find("menuTimer");
        TimerObj                = TimerGameObject.GetComponent<TimerMenu>();

        anim                    = gameObject.GetComponentInChildren<Animator>();

        anim.Play("Idle");
	}

    /**---------------------------------------------------------------------------------
     * 
     */
	void 
    Update () 
    {
	    //Empty
	}
}
