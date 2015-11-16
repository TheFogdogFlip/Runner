using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject Player;
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
	// Use this for initialization
	void Start () {
        distance = 3.0f;
        height = 0.1f;
        heightDamping = 3.0f;
        rotationDamping = 5.0f;
    }
	
	// Update is called once per frame
	void Update () {
        rotAngle = Player.transform.eulerAngles.y;
        wantedHeight = Player.transform.position.y + height;
        //smooth rot
        currentRotAngle = Mathf.LerpAngle(currentRotAngle, rotAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        currentRot = Quaternion.Euler(0, currentRotAngle, 0);

        heightVec[1] = currentHeight;
        transform.position = Player.transform.position + heightVec;
        transform.position -= currentRot * Vector3.forward * distance;

        //transform.position.y = currentHeight;

        transform.LookAt(Player.transform);
    }
}
