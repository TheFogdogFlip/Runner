using UnityEngine;
using System.Collections;

/**-----------------------------------------------------------------------------------
 * Enum for determining the current state of the player character.
 */
public enum PlayerState
{
    Idle,
    TurnRight,
    TurnLeft,
    Jump,
    Slide
}

/**
 * Playerbase is used as the base class for both the Player-character and the ghosts, consists of all the movement commands they both share. 
 */
public class PlayerBase : MonoBehaviour
{
    
    protected float runSpeed = 2f; //Tiles per Second.
    protected float turnSpeed = 225; //The speed of which the character turn.
    protected float jumpForce = 2.2f; //The inital vertical force of the jump.
    protected float gravity = 5f; //The negative force that pulls the character back towards the floor.
    protected float slideLenght = 0.75f; //The lenght of the slide.
    
    protected bool isJumpLocked = false; //Bool value which sets the current input mode of jump/slide, locked jumps only occur once a new tile is stepped on.
    protected bool isInvulnerableJ = false; //Remembers wheter or not the character is vulnerable by jump based obstacles.
    protected bool isInvulnerableS = false; //Remembers wheter or not the character is vulnerable by slide based obstacles.
    protected bool isJumping = false; //Remembers when the character is currently jumping.
    protected bool isFalling = false; //Remembers when the character is currently falling.
    protected bool isTurning = false; //Remembers when the character is activly turning.
    protected bool isSliding = false; //Remembers when the character is currently sliding.
    protected bool isActionActive = true; //Remembers wheter or not an non-specific action is active.

    protected float rotationTarget; //Remembers the targetrotation of a turn.
    protected float rotationLast; //Remembers the sourcerotation of a turn.
    protected float crntSlideLength; //Stores the current slide lenght.
    protected float crntJumpForce; //Stores the current jump force.
  
    protected PlayerState nextAction = PlayerState.Idle; //Remembers the current state of the character.
    
    //Necessities.
    protected Quaternion qTo = Quaternion.identity;
    protected Rigidbody rb;
    protected BoxCollider bc;
    protected Animator anim;

    /**---------------------------------------------------------------------------------
     * Occurs on creation of the character, necessary values are set.
     */
    void Awake ()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        crntSlideLength = slideLenght;
        crntJumpForce = jumpForce;
        rotationTarget = World.Instance.StartDirection.y;
        anim = gameObject.GetComponentInChildren<Animator>();
        anim.Play("Run");
    }

    /**---------------------------------------------------------------------------------
     * The keeper of the continual movement.
     */
    protected void UpdateRun()
    {
        //Run forward
        transform.Translate(transform.forward * 2.0f * runSpeed * Time.deltaTime, Space.World);
    }

    /**---------------------------------------------------------------------------------
     * The activation of a left turn, rotationtarget is set to the appropriate point. Open possibility to add a animation.
     */
    protected void TurnLeft()
    {
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
     * Activation of a right turn, with possibility to easily add some turning animations.
     */
    protected void TurnRight()
    {
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
     * Continual updating of the turning, only really applicable if a turn is initiated.
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
     * Activation method for the slide command, shrinks the hitbox and activates the animation.
     */
    protected void Slide()
    {

        isSliding = true;
        anim.SetTrigger("Slide");
        bc.center = new Vector3(bc.center.x, 0.14f, bc.center.z);
        bc.size = new Vector3(bc.size.x, 0.5f * bc.size.y,bc.size.z);
    }

    /**---------------------------------------------------------------------------------
     * updatemethod for the slide command, keeps the characters hitbox shrinked until the slidelenght has been reached.
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
     * The collider detector, pretty selfexplanatory.
     */
    void OnTriggerEnter(Collider other)
    {
        
        switch (other.gameObject.tag)
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
            case "Obstacle_Jump":
                if (!isInvulnerableJ) Death();
                break;
            case "Obstacle_Slide":
                if (!isInvulnerableS) Death();
                break;
            case "Hole":
                if (!isInvulnerableJ)
                {
                    Falling();
                    isFalling = true;
                }
                break;
        }
        
    }

    /**---------------------------------------------------------------------------------
     * Activation command of the jump command.
     */
    protected void Jump()
    {
        
        isJumping = true;
        anim.SetTrigger("Jump");
    }

    /**---------------------------------------------------------------------------------
     * Continual updatecheck wheter a not a jump has been initiated.
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
     * Continual updatecheck wheter or not the character shuold currently be falling.
     */
    private void UpdateFalling()
    {
        if (isFalling)
        {
            transform.Translate(transform.up * -gravity * Time.deltaTime, Space.World);
        }
    }

    /**---------------------------------------------------------------------------------
     * Set's a new value to the nextAction variable, this value will then be applied next time a tile is reached.
     */
    protected void SetNextAction(PlayerState input)
    {
        nextAction = input;
    }

    /**---------------------------------------------------------------------------------
     * Activation of the choosen queued up action, checks for when a new tile is reached.
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
     * Is activated every time a new tile is reached, activates an action if an action is queued up.
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
     * What actually happens when the death state is reached, overridden by player, applies only on ghosts.
     */
    protected virtual void Death()
    {
        Destroy(gameObject);
    }

    /**---------------------------------------------------------------------------------
     * Empty method, overridden by player.
     */
    protected virtual void GoalFunc()
    {
        //Empty
    }

    /**---------------------------------------------------------------------------------
     * Method activated when falling down a pit, falling down pits currently activates the slide animation.
     */
    protected virtual void Falling()
    {
        anim.SetTrigger("Slide");
    }

    /**---------------------------------------------------------------------------------
     * Updateloop, updates the movement of the character.
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
