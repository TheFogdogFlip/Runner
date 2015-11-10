using UnityEngine;
using System.Collections;

public class ResetPlayer : MonoBehaviour
{
    private Vector3 StartPosition;
    public Transform PlayerTransform;
	
	void Start ()
    {
        StartPosition = PlayerTransform.position;
	}
	
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            PlayerTransform.position.Set(StartPosition[0], StartPosition[1], StartPosition[2]);
        }
    }
}
