using UnityEngine;
using System.Collections;

public enum PlayerState
{
    Idle,
    TurnRight,
    TurnLeft,
    Jump,
    Slide
}


public class PlayerBase : MonoBehaviour
{
    //WORKABLE
    protected float runSpeed = 2f; //tiles per second.
    protected float turnSpeed = 225;
    protected float jumpForce = 2.2f;
    protected float gravity = 5f;
    protected float slideLenght = 0.75f;
    protected bool isJumpLocked = false;
    

    //DONT TOUCH
    protected bool isJumping = false;
    protected bool isFalling = false;
    protected Rigidbody rb;
    protected float rotationTarget;
    protected float rotationLast;
    protected Quaternion qTo = Quaternion.identity;
    protected bool isTurning;
    protected bool isSliding = false;
    protected float crntSlideLength;
    protected BoxCollider bc;
    protected Animator anim;
    protected bool isFirstFrame = true;
    protected PlayerState nextAction = PlayerState.Idle;
    protected bool isActionActive = true;
    protected float crntJumpForce;


    /**---------------------------------------------------------------------------------
     * 
     */
    void Awake ()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        crntSlideLength = slideLenght;
        crntJumpForce = jumpForce;
        rotationTarget = World.Instance.StartDirection.y;
        isFirstFrame = false;
        anim = gameObject.GetComponentInChildren<Animator>();
        anim.Play("Run");
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    protected void UpdateRun()
    {
        //Run forward
        transform.Translate(transform.forward * 2.0f * runSpeed * Time.deltaTime, Space.World);
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    protected void TurnLeft()
    {
        //ACTIVATION PHASE

        //anim.SetTrigger("TurnLeft90");

        rotationLast = rotationTarget;
        rotationTarget -= 90.0f;
        isTurning = true;

        if (rotationTarget == 360 || rotationTarget == -360) rotationTarget = 0.0f;
        if (rotationTarget == -270) rotationTarget = 90;
        if (rotationTarget == -180) rotationTarget = 180;
        if (rotationTarget == -90) rotationTarget = 270;
        qTo = Quaternion.Euler(0.0f, rotationTarget, 0.0f);

    }

    /**---------------------------------------------------------------------------------
     * 
     */
    protected void TurnRight()
    {
        //ACTIVATION PHASE

        //anim.SetTrigger("TurnRight90");

        rotationLast = rotationTarget;
        rotationTarget += 90.0f;
        isTurning = true;

        if (rotationTarget == 360 || rotationTarget == -360) rotationTarget = 0.0f;
        if (rotationTarget == -270) rotationTarget = 90;
        if (rotationTarget == -180) rotationTarget = 180;
        if (rotationTarget == -90) rotationTarget = 270;
        qTo = Quaternion.Euler(0.0f, rotationTarget, 0.0f);

    }

    /**---------------------------------------------------------------------------------
     * 
     */
    private void UpdateTurn()
    {
        if (isTurning)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, turnSpeed * Time.deltaTime);
            if (qTo == transform.rotation) isTurning = false;

            Vector3 tempVec = transform.position;
            if (tempVec.z < 0) tempVec.z *= -1;
            if (tempVec.x < 0) tempVec.x *= -1;
            if (rotationLast == 0 && tempVec.z % 2 >= 0 && tempVec.z % 2 < 1) { isTurning = false; transform.rotation = qTo; }
            else if (rotationLast == 90 && tempVec.x % 2 >= 0 && tempVec.x % 2 < 1)  { isTurning = false; transform.rotation = qTo; }
            else if (rotationLast == 180 && tempVec.z % 2 <= 2 && tempVec.z % 2 > 1) { isTurning = false; transform.rotation = qTo; }
            else if (rotationLast == 270 && tempVec.x % 2 <= 2 && tempVec.x % 2 > 1) { isTurning = false; transform.rotation = qTo; }
        }
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    protected void Slide()
    {

        isSliding = true;
        anim.SetTrigger("Slide");
        bc.center = new Vector3(bc.center.x, 0.14f, bc.center.z);
        bc.size = new Vector3(bc.size.x, 0.5f * bc.size.y,bc.size.z);
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    private void UpdateSlide()
    {
        if (isSliding)
        {
            if (crntSlideLength <= 0.1)
            {
                bc.size = new Vector3(bc.size.x, 2.0f * bc.size.y, bc.size.z);
                bc.center = new Vector3(bc.center.x, 0.2765842f, bc.center.z);
                crntSlideLength = slideLenght;
                isSliding = false;
            }
            else
            {
                crntSlideLength -= 1.0f * Time.deltaTime;
            }
        }
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    void OnTriggerEnter(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case "Floor":
                isJumping = false;
                isFalling = false;
                transform.position = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
                break;
            case "Pole":
            case "Wall":
                Death();
                break;
            case "Goal":
                GoalFunc();
                break;
            case "Hole":
                Falling();
                isFalling = true;
                break;
        }
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    protected void Jump()
    {
        
        isJumping = true;
        anim.SetTrigger("Jump");
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    private void UpdateJump()
    {
        if (isJumping)
        {
            if (crntJumpForce <= 0)
            {
                crntJumpForce = jumpForce;
                isFalling = true;
            }
            //Going up
            else if (!isFalling)
            {
                crntJumpForce -= gravity * Time.deltaTime;
                transform.Translate(transform.up * crntJumpForce * Time.deltaTime, Space.World);
            }
        }
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    private void UpdateFalling()
    {
        if (isFalling)
        {
            transform.Translate(transform.up * -gravity * Time.deltaTime, Space.World);
        }
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    protected void SetNextAction(PlayerState input)
    {
        nextAction = input;
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    private void nextActionActivation()
    {
        if (!isSliding && !isFalling && !isJumping)
        {   
            Vector3 tempVec = transform.position;
            if (tempVec.z < 0) tempVec.z *= -1;
            if (tempVec.x < 0) tempVec.x *= -1;

            if (rotationTarget <= 1 && rotationTarget >= -1)
            {
                rotationTarget = 0;
            }

            if (rotationTarget == 0)
            {

                if (tempVec.z % 2 <= 1)
                {
                    isActionActive = false;
                }

                if (tempVec.z % 2 >= 1 && !isActionActive)
                {
                    ActivateNextAction();
                    isActionActive = true;
                }
            }
            if (rotationTarget == 90)
            {
                if (tempVec.x % 2 <= 1)
                {
                    isActionActive = false;
                }

                if (tempVec.x % 2 >= 1 && !isActionActive)
                {
                    ActivateNextAction();
                    isActionActive = true;
                }
            }
            if (rotationTarget == 180)
            {

                if (tempVec.z % 2 >= 1)
                {
                    isActionActive = false;
                }

                if (tempVec.z % 2 < 1 && !isActionActive)
                {
                    ActivateNextAction();
                    isActionActive = true;
                }
            }
            if (rotationTarget == 270)
            {

                if (tempVec.x % 2 >= 1)
                {
                    isActionActive = false;
                }
                if (tempVec.x % 2 <= 1 && !isActionActive)
                {
                    ActivateNextAction();
                    isActionActive = true;
                }
            }
        }
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    virtual protected void ActivateNextAction()
    {
        if (nextAction != PlayerState.Idle)
        {
            if (nextAction == PlayerState.TurnLeft) TurnLeft();
            if (nextAction == PlayerState.TurnRight) TurnRight();
            if (nextAction == PlayerState.Jump) Jump();
            if (nextAction == PlayerState.Slide) Slide();

            nextAction = PlayerState.Idle;
        }
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    protected virtual void Death()
    {
        Destroy(gameObject);
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    protected virtual void GoalFunc()
    {
        //Empty
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    protected virtual void Falling()
    {
        anim.SetTrigger("Slide");
    }

    /**---------------------------------------------------------------------------------
     * 
     */
    protected void MovementUpdate()
    {
        nextActionActivation();
        UpdateJump();
        UpdateSlide();
        UpdateFalling();
        UpdateTurn();
        UpdateRun();
    }
}
