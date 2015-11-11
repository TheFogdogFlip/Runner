using UnityEngine;
using System.Collections;

public class ResetPlayer : MonoBehaviour
{
    private Vector3 StartPosition;
    private Transform PlayerTransform;
	
	void Start ()
    {
        PlayerTransform = GetComponent<Transform>();
        StartPosition = PlayerTransform.position;
	}
	
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            PlayerTransform.position = StartPosition;
        }
    }
}
