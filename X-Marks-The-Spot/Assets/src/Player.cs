using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    //For moving forward
    public float ForwardForce;
    public Rigidbody rb;

    //For turning 90 degrees smoothly
    public float turnSpeed = 55.0f;
    private float rotationTarget = 0.0f;
    private Quaternion qTo = Quaternion.identity;
    private float lastY = 0f;


   

    void Awake ()
    {
        rb = GetComponent<Rigidbody>();
	}

    void Update()
    {
        if (lastY == transform.rotation.eulerAngles.y)
        {
            Turn();
        }
        lastY = transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, turnSpeed * Time.deltaTime);
        
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        rb.AddForce(transform.forward * ForwardForce);
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
}
