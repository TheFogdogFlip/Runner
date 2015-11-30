using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Recorder : MonoBehaviour {
    int imageNumber;
    int fps;
    public float timer;

    GameObject target = null;

    public List<TimeStamp> inputs;
    public List<List<TimeStamp>> ghostinputs;
    public List<GameObject> ghosts;

    //For RenderTexuter screenshot
    private bool record;
    private Camera replayCam;
    

    // Use this for initialization
    void Start () {

        
        GameObject playerObject = GameObject.Find("Player(Clone)");
        Destroy(playerObject);

        GameObject cam = GameObject.Find("PlayerCamera(Clone)");
        cam.GetComponent<CameraController>().enabled = false;
        Destroy(cam);
        setupRun(true);
    }

    void setupRun(bool r)
    {
        record = r;
        imageNumber = 1;
        fps = 0;
        timer = 0;

        transform.position = World.Instance.StartPosition;
        transform.rotation = Quaternion.Euler(World.Instance.StartDirection);

        Ghost ghost;
        GameObject go;
        for (int i = 0; i < ghostinputs.Count - 1; ++i)
        {
            go = Instantiate(Resources.Load("Ghost", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection)) as GameObject;
            ghost = go.GetComponent<Ghost>();
            ghost.inputs = ghostinputs[i];
            ghosts.Add(go);
        }
        if (target == null)
            target = Instantiate(Resources.Load("ReplayPlayer", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection)) as GameObject;
        else
        {
            GameObject temp = target;
            target = Instantiate(Resources.Load("ReplayPlayer", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection)) as GameObject;
            Destroy(temp);
        }
        gameObject.transform.position = new Vector3(target.transform.position.x, 20, target.transform.position.z);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(70, 0, 0));
        ghost = target.GetComponent<Ghost>();
        ghost.inputs = inputs;

        SetupGhostTimer();
        
        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/captured_images"))
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/captured_images");
        if (record)
            Time.captureFramerate = 60;

    }

    public void SetupGhostTimer()
    {
        GameObject ghostTimerGameObj = GameObject.Find("ghostTimer");
        Timer_Ghost ghostTimerObj = ghostTimerGameObj.GetComponent<Timer_Ghost>();
        ghostTimerObj.f_time = 0;
        ghostTimerObj.textObj.text = "0";
        ghostTimerObj.TimerRunning = true;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach(GameObject go in ghosts)
            {
                Destroy(go);
            }
            setupRun(true);
        }
            
    }

    void LateUpdate()
    {  
        if (target != null)
            gameObject.transform.position = new Vector3(target.transform.position.x, 20, target.transform.position.z);


        

        if (record)
            screenShot();
        timer += Time.deltaTime;

        if (timer >= 1.0f)
        {
            print(fps);
            fps = 0;
            timer = 0;
        }
    }
    
    void screenShot()
    {
        
        int n = (int)System.Math.Floor(System.Math.Log10(imageNumber) + 1.0);
        string zeros = new string('0', 5 - n);
        string filename = Application.persistentDataPath + "/captured_images/img" + zeros + imageNumber.ToString() + ".png";

        imageNumber++;
        fps++;

        Application.CaptureScreenshot(filename);
        
    }
}
