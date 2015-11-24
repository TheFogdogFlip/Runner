using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour
{
    //WORKABLE
    public float deceleration;
    protected float acceleration;
    protected float runSpeed = 2f; //tiles per second.
    protected float jumpSpeed;
    protected float jumpHeight = 0.8f;
    protected float turnSpeed = 300.0f;

    //DONT TOUCH
    protected float crntSpeed;
    protected bool isJumping = false;
    protected bool isFalling = false;
    protected Rigidbody rb;
    protected float rotationTarget;
    protected Quaternion qTo = Quaternion.identity;
    protected int turnPhase = 0;
    protected bool isSliding = false;
    protected float crntSlideLength;
    protected BoxCollider bc;
    protected Animator anim;
    protected bool isFirstFrame = true;


    // Use this for initialization
    void Awake ()
    {
        deceleration = runSpeed * runSpeed;
        acceleration = runSpeed * runSpeed;
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        crntSlideLength = 1 / runSpeed;
        jumpSpeed = jumpHeight / (1 / (2 * runSpeed));
        rotationTarget = transform.rotation.y;
    }

    protected void UpdateRun()
    {
        //Run forward
        transform.Translate(transform.forward * 2.0f *crntSpeed * Time.deltaTime, Space.World);
    }

    protected void TurnLeft()
    {
        //ACTIVATION PHASE
        if (turnPhase == 0)
        {
            anim.Play("TurnLeft90");

            rotationTarget -= 90.0f;
            turnPhase = 1;

            if (rotationTarget == 360 || rotationTarget == -360)
            {
                rotationTarget = 0.0f;
            }
            qTo = Quaternion.Euler(0.0f, rotationTarget, 0.0f);

        }
    }

    protected void TurnRight()
    {
        //ACTIVATION PHASE
        if (turnPhase == 0)
        {
            anim.Play("TurnRight90");

            rotationTarget += 90.0f;
            turnPhase = 1;

            if (rotationTarget == 360 || rotationTarget == -360)
            {
                rotationTarget = 0.0f;
            }
            qTo = Quaternion.Euler(0.0f, rotationTarget, 0.0f);
        }
    }

    private void UpdateTurn()
    {
        //BRAKING PHASE
        if (turnPhase == 1)
        {
            crntSpeed -= deceleration * Time.deltaTime;

            if (crntSpeed <= 0)
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
            crntSpeed += acceleration * Time.deltaTime;
            if (crntSpeed >= runSpeed)
            {
                crntSpeed = runSpeed;
                turnPhase = 0;
            }
        }
    }

    protected void Slide()
    {
        //Slide start
        isSliding = true;
        //bc.size += new Vector3(0, -(bc.size.y * 0.5f), 0);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 0.5f, transform.localScale.z);
    }
    private void UpdateSlide()
    {
        if (isSliding)
        {
            crntSlideLength -= 1.0f * Time.deltaTime;
            //Slide end
            if (crntSlideLength <= 0.1)
            {
                //bc.size += new Vector3(0, bc.size.y, 0);
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);
                crntSlideLength = 1  / runSpeed;
                isSliding = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isJumping = false;
            isFalling = false;
            transform.position = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            Death();
        }
		
		if (other.gameObject.CompareTag("Goal"))
        {
            GoalFunc();
        }
		
        if (other.gameObject.CompareTag("Hole"))
        {
            isFalling = true;
        }
    }

    protected void Jump()
    {
        isJumping = true; 
    }

    private void UpdateJump()
    {
        if (isJumping)
        {
            //Going up
            if (!isFalling)
            {
                transform.Translate(new Vector3(0, jumpSpeed * Time.deltaTime, 0));
                if (transform.position.y >= jumpHeight-0.1)
                {
                    isFalling = true;
                }
            }
        }
    }

    private void UpdateFalling()
    {
        if (isFalling)
        {
            transform.Translate(new Vector3(0, 1.5f * -jumpSpeed * Time.deltaTime, 0));
        }
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }

	 protected virtual void GoalFunc()
    {

    }
	
    protected void MovementUpdate()
    {
        if (isFirstFrame)
        {
            isFirstFrame = false;
            anim = gameObject.GetComponentInChildren<Animator>();
            anim.Play("Run");
        }
        UpdateJump();
        UpdateSlide();
        UpdateFalling();
        UpdateTurn();
        UpdateRun();
        if (transform.position.y <= -5) Death();
    }
}
