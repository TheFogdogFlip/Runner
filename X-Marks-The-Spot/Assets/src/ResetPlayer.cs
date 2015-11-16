using UnityEngine;
using System.Collections;

public class ResetPlayer : MonoBehaviour
{
    private Vector3 StartPosition;
    private Quaternion StartRotation;
    private Transform PlayerTransform;
	
	void Start ()
    {
        PlayerTransform = GetComponent<Transform>();
        StartPosition = PlayerTransform.position;
        StartRotation = PlayerTransform.rotation;
	}
	
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
        Instantiate(Resources.Load("Player", typeof(GameObject)), StartPosition, StartRotation);
    }
}
