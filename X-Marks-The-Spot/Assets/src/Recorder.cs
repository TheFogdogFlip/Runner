using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Recorder : MonoBehaviour {
    int imageNumber;
    int fps;
    public float timer;
    public float timer2;
    int width;
    int height;
    Rect rectangle;

    GameObject target;

    public List<TimeStamp> inputs;
    public List<List<TimeStamp>> ghostinputs;

    // Use this for initialization
    void Start () {
        imageNumber = 0;
        fps = 0;
        timer = 0;
        rectangle = new Rect(0, 0, width, height);
        GameObject playerObject = GameObject.Find("Player(Clone)");

        //target = playerObject.transform;

        //Player player = playerObject.GetComponent<Player>();
        //player.enabled = false;
        //playerObject.transform.position = World.Instance.StartPosition;
        //playerObject.transform.rotation = Quaternion.Euler(World.Instance.StartDirection);
        //Ghost ghost = playerObject.AddComponent<Ghost>();
        //ghost.inputs = inputs;

        transform.position = World.Instance.StartPosition;
        transform.rotation = Quaternion.Euler(World.Instance.StartDirection);

        Ghost ghost;
        GameObject go;
        for (int i = 0; i < ghostinputs.Count - 1; ++i)
        {
            go = Instantiate(Resources.Load("Ghost", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection)) as GameObject;
            ghost = go.GetComponent<Ghost>();
            ghost.inputs = ghostinputs[i];

        }
        target = Instantiate(Resources.Load("ReplayPlayer", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection)) as GameObject;
        ghost = target.GetComponent<Ghost>();
        ghost.inputs = inputs;
        playerObject.GetComponent<Player>().SetupGhostTimer();
        Destroy(playerObject);
        GameObject cam = GameObject.Find("PlayerCamera(Clone)");
        cam.GetComponent<CameraController>().enabled = false;
        Destroy(cam);
        
     
    }
   

    void Update()
    {
        if (target != null)
            gameObject.transform.position = new Vector3(target.transform.position.x, 20, target.transform.position.z);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(70, 0, 0));
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;
        if (timer >= 0.033f)
        {
            //string filename = System.IO.Directory.GetCurrentDirectory() + "/captured images/img" + imageNumber.ToString() + ".png";

            imageNumber++;
            fps++;

            //Application.CaptureScreenshot(filename);
            timer = 0;
        }
        if(timer2 >= 1.0f)
        {
            print(fps);
            fps = 0;
            timer2 = 0;
        }
        
    }


}
