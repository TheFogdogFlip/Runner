using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject target;
    private float distance;
    private float height;
    private float heightDamping;
    private float rotationDamping;
    private float distanceDamping;

    private float rotAngleY;
    private float currentRotAngleY;
    private Quaternion currentRot;

    private float wantedHeight;
    private float currentHeight;
    private Vector3 heightVec;
    private Vector3 tiltVec;
    private float tilt;
    private Transform lookAtTrans;
    private bool isTurning;
    private GameObject timer;
    private Timer_Countdown timerObj;


    // Use this for initialization
    void Start () {
        //distance = 1.5f;
        height = 0.8f;
        heightDamping = 5.0f;
        rotationDamping = 5.0f;
        distanceDamping = 3.0f;
        target = GameObject.Find("Player(Clone)");
        timer = GameObject.Find("ctdTimer");
        timerObj = timer.GetComponent<Timer_Countdown>();
        tilt = 0.3f;
    }

	// Update is called once per frame
	void Update () {
        //wanted rotation and height
        rotAngleY = target.transform.eulerAngles.y;
        wantedHeight = target.transform.position.y + height;
        isTurning = target.GetComponent<Player>().GetTurn();
        //smooth rot
        currentRotAngleY = Mathf.LerpAngle(currentRotAngleY, rotAngleY, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        currentRot = Quaternion.Euler(0, currentRotAngleY, 0);

        //camera transform
        this.gameObject.transform.position = target.transform.position;
        this.gameObject.transform.position -= (currentRot * Vector3.forward * distance);
        heightVec[1] = height;
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, height, this.gameObject.transform.position.z);
        tiltVec[1] = currentHeight - tilt;

        if(timerObj.TimerFirstRunning)
        {
            distance = 1.0f;
        }
        else if (isTurning)
        {
            distance = Mathf.Lerp(distance, 1.0f, distanceDamping * Time.deltaTime);
        }
        else
        {
            distance = Mathf.Lerp(distance, 1.5f, distanceDamping * Time.deltaTime);
        }
        this.gameObject.transform.LookAt(new Vector3(target.transform.position.x, 0, target.transform.position.z) + tiltVec);
       
    }
}
