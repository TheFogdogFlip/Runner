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
    private Vector3 lookAtVec;
    private Transform lookAtPos;

    // Use this for initialization
    void Start () {
        distance = 1.5f;
        height = 0.5f;
        heightDamping = 3.0f;
        rotationDamping = 5.0f;
        target = GameObject.Find("Player(Clone)").transform;
        lookAtVec[1] = height;
    }
	
	// Update is called once per frame
	void Update () {
        //target = GameObject.Find("Player(Clone)").transform;
        rotAngle = target.eulerAngles.y;
        wantedHeight = target.position.y + height;

        //smooth rot
        currentRotAngle = Mathf.LerpAngle(currentRotAngle, rotAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        currentRot = Quaternion.Euler(0, currentRotAngle, 0);


        this.gameObject.transform.position = target.position;
        this.gameObject.transform.position -= currentRot * Vector3.forward * distance;
        heightVec[1] = currentHeight;
        this.gameObject.transform.position = this.gameObject.transform.position + heightVec;
        //LookAt
        //lookAtPos.position = target.position + lookAtVec;
        this.gameObject.transform.LookAt(target);
    }
}
