using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject Player;
    //private Transform LookAtPoint;
    private float distance;
    private float height;
    private float heightDamping;
    private float rotationDamping;
    private float rotAngle;
    private float currentRotAngle;
    private Quaternion currentRot;
    private float wantedHeight;
    private float currentHeight;
    private Vector3 heightVec;
    private Vector3 cameraLook;
	// Use this for initialization
	void Start () {
        distance = 1.5f;
        height = 0.5f;
        heightDamping = 3.0f;
        rotationDamping = 5.0f;
        //LookAtPoint = Player.transform;
    }
	
	// Update is called once per frame
	void Update () {
        rotAngle = Player.transform.eulerAngles.y;
        wantedHeight = Player.transform.position.y + height;
        //smooth rot
        currentRotAngle = Mathf.LerpAngle(currentRotAngle, rotAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        currentRot = Quaternion.Euler(0, currentRotAngle, 0);

        transform.position = Player.transform.position;
        transform.position -= currentRot * Vector3.forward * distance;
        heightVec[1] = currentHeight;
        transform.position = transform.position + heightVec;
        //transform.position.y = currentHeight;
        //LookAtPoint.position = Player.transform.position;
        //LookAtPoint.position = Player.transform.position + heightVec;
        transform.LookAt(Player.transform);
    }
}
