using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    
    public float ForwardForce;
    public Rigidbody rb;

    public float turnSpeed = 55.0f;
    private float rotation = 0.0f;
    private Quaternion qTo = Quaternion.identity;


   

    void Awake ()
    {
        rb = GetComponent<Rigidbody>();
	}

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            rotation += 90.0f;
            qTo = Quaternion.Euler(0.0f, rotation, 0.0f);

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rotation -= 90f;
            qTo = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, turnSpeed * Time.deltaTime);

    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        float turn;

        rb.AddForce(transform.forward * ForwardForce);

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Debug.Log("Moving right!");
        //    transform.Rotate(Vector3.up * 90);
        //}
        //else if (Input.GetKeyDown(KeyCode.D))
        //{
        //    Debug.Log("Moving left!");
        //    transform.Rotate(Vector3.down * 90);
        //}
    }
}
