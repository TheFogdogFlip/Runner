using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    //For moving forward
    public float runSpeed;

    //For jumping
    public float jumpForce;
    public Rigidbody rb;

    //For turning 90 degrees smoothly
    public float turnSpeed = 55.0f;
    private float rotationTarget = 0.0f;
    private Quaternion qTo = Quaternion.identity;
    private float lastY = 0f;

    //


   

    void Awake ()
    {
        rb = GetComponent<Rigidbody>();
	}

    void Update()
    {
        //Running forward block
        Run();

        //Turn block
        if (lastY == transform.rotation.eulerAngles.y)
        {
            Turn();
        }
        lastY = transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, turnSpeed * Time.deltaTime);
        
    }

    void FixedUpdate()
    {
        Jump();
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
        if (Input.GetButtonDown("Jump") && rb.velocity.y < 0.01f)
        {
            rb.AddForce(new Vector3(0f, 1f, 0f) * jumpForce);
        }
    }

    void Slide()
    {
        if (Input.GetButtonDown("Slide"))
        {

        }
    }
}
