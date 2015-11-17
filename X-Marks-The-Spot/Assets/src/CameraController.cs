using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform target;
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
    private Vector3 tiltVec;

    // Use this for initialization
    void Start () {
        distance = 1.2f;
        height = 0.8f;
        heightDamping = 3.0f;
        rotationDamping = 5.0f;
        target = GameObject.Find("Player(Clone)").transform;
        tiltVec[1] = 0.4f;
    }
	
	// Update is called once per frame
	void Update () {
        //wanted rotation and height
        rotAngle = target.eulerAngles.y;
        wantedHeight = target.position.y + height;

        //smooth rot
        currentRotAngle = Mathf.LerpAngle(currentRotAngle, rotAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        currentRot = Quaternion.Euler(0, currentRotAngle, 0);

        //camera transform
        this.gameObject.transform.position = target.position;
        this.gameObject.transform.position -= currentRot * Vector3.forward * distance;
        heightVec[1] = currentHeight;
        this.gameObject.transform.position = this.gameObject.transform.position + heightVec;

        //LookAt
        this.gameObject.transform.LookAt(target.position + tiltVec);
    }
}
