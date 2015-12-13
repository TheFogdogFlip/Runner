using UnityEngine;
using System.Collections;

public class StartMenuAnimator : MonoBehaviour {

    private Animator anim;
    private Rigidbody rb;
    private BoxCollider bc;

	// Use this for initialization
	void Start () 
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        anim.Play("Idle");
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
