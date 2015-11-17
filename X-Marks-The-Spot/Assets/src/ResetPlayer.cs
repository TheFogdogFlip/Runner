using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResetPlayer : MonoBehaviour
{
    private Timer_Ghost ghostTimerObj;
    private GameObject ghostTimerGameObj;

    private Player player;
    private List<List<TimeStamp>> ghostinputs;
    private List<GameObject> ghosts;

    private Vector3 StartPosition;
    private Quaternion StartRotation;
    private Transform PlayerTransform;
	
	void Start ()
    {
        PlayerTransform = GetComponent<Transform>();
        StartPosition = PlayerTransform.position;
        StartRotation = PlayerTransform.rotation;
        player = GetComponent<Player>();
        ghostinputs = new List<List<TimeStamp>>();
        ghosts = new List<GameObject>();
	}
	
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            
            Death();
            
            GameObject go = Instantiate(Resources.Load("Ghost", typeof(GameObject)), StartPosition, StartRotation) as GameObject;
            Ghost ghost = go.GetComponent<Ghost>();
            ghost.inputs = player.inputs;
            ghosts.Add(go);
            ghostinputs.Add(player.inputs);
            SetupGhostTimer();
        }
    }

    void Death()
    {
        Destroy(gameObject);
        Instantiate(Resources.Load("Player", typeof(GameObject)), StartPosition, StartRotation);
        foreach(GameObject go in ghosts)
        {
            Destroy(go);
        }
        for(int i = 0; i < ghostinputs.Count; ++i)
        {
            GameObject go = Instantiate(Resources.Load("Ghost", typeof(GameObject)), StartPosition, StartRotation) as GameObject;
            Ghost ghost = go.GetComponent<Ghost>();
            ghost.inputs = ghostinputs[i];
            ghosts.Add(go);
            
        }

    }
    void SetupGhostTimer()
    {
        ghostTimerGameObj = GameObject.Find("ghostTimer");
        ghostTimerObj = ghostTimerGameObj.GetComponent<Timer_Ghost>();
        ghostTimerObj.f_time = 0;
        ghostTimerObj.textObj.text = "0";
        ghostTimerObj.TimerRunning = true;
        

    }
}
