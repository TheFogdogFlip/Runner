using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour
{
    //WORKABLE
    protected float deceleration = 0.002f;
    protected float acceleration = 0.003f;
    protected float runSpeed = 0.05f;
    protected float jumpSpeed = 0.05f;
    protected float jumpHeight = 1.0f;
    protected float turnSpeed = 300.0f;
    protected float slideSpeed = 1.0f;
    protected float maxSlideLength = 1.0f;

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



    // Use this for initialization
    void Awake ()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        crntSlideLength = maxSlideLength;
        rotationTarget = transform.rotation.y;
    }

    protected void UpdateRun()
    {
        //Run forward
        transform.Translate(transform.forward * crntSpeed * 100 * Time.deltaTime, Space.World);
    }

    protected void TurnLeft()
    {
        //ACTIVATION PHASE
        if (turnPhase == 0)
        {
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
            //Slide end
            if (crntSlideLength <= 0)
            {
                //bc.size += new Vector3(0, bc.size.y, 0);
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);
                crntSlideLength = maxSlideLength;
                isSliding = false;
            }
            else
            {
                crntSlideLength -= slideSpeed * Time.deltaTime;
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

        if (other.gameObject.CompareTag("Hole"))
        {
            isFalling = true;
        }
    }

    protected void Jump()
    {

        if (isSliding)
        {
            //bc.size += new Vector3(0, bc.size.y, 0);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);
            crntSlideLength = maxSlideLength;
            isSliding = false;
        }

        isJumping = true; 
    }

    private void UpdateJump()
    {
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
        }
    }

    private void UpdateFalling()
    {
        if (isFalling)
        {
            transform.Translate(new Vector3(0, -jumpSpeed, 0));
        }
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }

    protected void MovementUpdate()
    {
        UpdateJump();
        UpdateSlide();
        UpdateFalling();
        UpdateTurn();
        UpdateRun();
    }
}
