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
    private Transform lookAtTrans;
    private Vector3 lookAtVec;
    // Use this for initialization
    void Start () {
        distance = 1.2f;
        height = 0.7f;
        heightDamping = 5.0f;
        rotationDamping = 5.0f;
        target = GameObject.Find("Player(Clone)").transform;
        tilt = 0.2f;
    }
	
	// Update is called once per frame
	void Update () {
        //wanted rotation and height
        rotAngleY = target.eulerAngles.y;
        wantedHeight = target.position.y*0f + height;

        //smooth rot
        currentRotAngleY = Mathf.LerpAngle(currentRotAngleY, rotAngleY, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        currentRot = Quaternion.Euler(0, currentRotAngleY, 0);

        //camera transform
        this.gameObject.transform.position = target.position;
        this.gameObject.transform.position -= (currentRot * Vector3.forward * distance);
        heightVec[1] = height;
        this.gameObject.transform.position = this.gameObject.transform.position + heightVec;
        tiltVec[1] = currentHeight - tilt;
        //LookAt
        //lookAtTrans = target;
        //lookAtVec[0] = target.position.x;
        //lookAtVec[2] = target.position.z;
        //lookAtTrans.position = lookAtVec;
        //lookAtTrans.position = new Vector3(target.position.x, 0, target.position.z);
        this.gameObject.transform.LookAt(target.position + tiltVec);
    }
}
