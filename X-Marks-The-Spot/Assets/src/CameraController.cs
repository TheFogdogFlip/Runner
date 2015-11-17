using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform target;
    private float distance;
    private float height;
    private float heightDamping;
    private float rotationDamping;

    private float rotAngleY;
    private float rotAngleX;
    private float currentRotAngleY;
    private float currentRotAngleX;
    private Quaternion currentRot;

    private float wantedHeight;
    private float currentHeight;
    private Vector3 heightVec;
    private Vector3 tiltVec;
    private float tilt;

    // Use this for initialization
    void Start () {
        distance = 1.2f;
        height = 0.7f;
        heightDamping = 3.0f;
        rotationDamping = 5.0f;
        target = GameObject.Find("Player(Clone)").transform;
        //tiltVec[1] = 0.4f;
        tilt = 0.3f;
    }
	
	// Update is called once per frame
	void Update () {
        //wanted rotation and height
        rotAngleY = target.eulerAngles.y;
        rotAngleX = target.eulerAngles.x;
        wantedHeight = target.position.y*0f + height;

        //smooth rot
        currentRotAngleY = Mathf.LerpAngle(currentRotAngleY, rotAngleY, rotationDamping * Time.deltaTime);
        currentRotAngleX = Mathf.LerpAngle(currentRotAngleX, rotAngleX, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        currentRot = Quaternion.Euler(currentRotAngleX, currentRotAngleY, 0);

        //camera transform
        this.gameObject.transform.position = target.position;
        this.gameObject.transform.position -= currentRot * Vector3.forward * distance;
        heightVec[1] = currentHeight;
        this.gameObject.transform.position = this.gameObject.transform.position + heightVec;
        tiltVec[1] = currentHeight - tilt;
        //LookAt
        this.gameObject.transform.LookAt(target.position + tiltVec);
    }
}
