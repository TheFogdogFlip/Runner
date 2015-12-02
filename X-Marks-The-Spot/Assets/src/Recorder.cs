using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

public class Recorder : MonoBehaviour {
    int imageNumber;
    public int fps = 30;
    public string imageFolder = "captured_images";
    public string audioFolder = "captured_audio";
    public float timer;

    GameObject target = null;

    public List<TimeStamp> inputs;
    public List<List<TimeStamp>> ghostinputs;
    public List<GameObject> ghosts;

    //For RenderTexuter screenshot
    public bool recording = false;
    private bool loading = false;
    private Camera replayCam;
    private int width;
    private int height;
    private Rect rectangle;

    //For Overlay/Loading
    private float loadValue;
    private Image loadBarImage;
    private Text loadText;
    private Timer_Ghost ghostTimerObj;
    public float finishedTime;
    private bool generating;
    Process process;
    StreamReader reader;

    // Use this for initialization
    void Start () {

        width = Screen.width;
        height = Screen.height;

        loadValue = 0.0f;
        rectangle = new Rect(0, 0, width, height);
        replayCam = gameObject.GetComponent<Camera>();

      

        GameObject playerObject = GameObject.Find("Player(Clone)");
        Destroy(playerObject);

        GameObject cam = GameObject.Find("PlayerCamera(Clone)");
        cam.GetComponent<CameraController>().enabled = false;
        Destroy(cam);

        
        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/" + imageFolder))
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/" + imageFolder);
        else
        {
            foreach(string path in System.IO.Directory.GetFiles(Application.persistentDataPath + "/" + imageFolder))
            {
                System.IO.File.Delete(path);
            }
        }

        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/" + audioFolder))
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/" + audioFolder);
        //else
        //{
        //    foreach (string path in System.IO.Directory.GetFiles(Application.persistentDataPath + "/" + audioFolder))
        //    {
        //        System.IO.File.Delete(path);
        //    }
        //}

        setupRun(false);
    }

    void setupRun(bool r)
    {
        recording = r;
        imageNumber = 1;
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
        
     
        if (recording)
            Time.captureFramerate = fps;

    }

    public void SetupGhostTimer()
    {
        GameObject ghostTimerGameObj = GameObject.Find("ghostTimer");
        ghostTimerObj = ghostTimerGameObj.GetComponent<Timer_Ghost>();
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
            loading = true;
            Instantiate(Resources.Load("RecordingOverlay", typeof(Canvas)));
            loadBarImage = GameObject.Find("LoadBar").GetComponent<Image>();
            loadText = GameObject.Find("LoadText").GetComponent<Text>();
        }
        if (loading)
        {
            if (ghostTimerObj.f_time <= finishedTime && recording)
            {
                loadText.text = "Recording Video";
                loadValue = 0.5f * ghostTimerObj.f_time / finishedTime;

            }
            else
            {
                recording = false;
            }
            if (!recording && !generating)
            {
                generating = true;
                loadText.text = "Generating Video";
                string filepath = Application.persistentDataPath;
                print(System.IO.File.Exists("ffmpeg.exe"));
                print(System.IO.File.Exists(filepath + "/captured_images/img00001.png"));
                print(System.IO.File.Exists(filepath + "/captured_audio/Kalimba.mp3"));
                print(fps);

                try
                {
                    process = new Process();

                    process.EnableRaisingEvents = true;
                    process.StartInfo.FileName = "ffmpeg.exe";
                    process.StartInfo.Arguments = "-framerate " + fps.ToString() + " -i \"" + filepath + "/captured_images/img%05d.png\" -i Kalimba.mp3 -c:v libx264 -vf fps=" + fps.ToString() + " -pix_fmt yuv420p -shortest \"" + filepath + "/out.mp4\" -y";

                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;


                    if (!process.Start())
                    {
                        print("Error trying to start");
                    }
                    reader = process.StandardError;

                }
                catch
                {
                    print("Error generating file");
                }
            }
            if (generating)
            {
                string line;
                try
                {
                    if ((line = reader.ReadLine()) != null)
                    {
                        Match match = Regex.Match(line, "frame=\\s+(\\d+)");
                        if (match.Captures.Count > 0)
                        {
                            //print(match.Groups[1]);
                            loadValue = 0.5f + 0.5f * int.Parse(match.Groups[1].Value) / imageNumber;
                            print(loadValue);
                        }
                    }
                    else
                    {
                        generating = false;
                        loading = false;
                        process.Close();
                        reader.Close();
                        loadText.text = "Done!";
                    }
                }
                catch
                {

                }

            }

            loadBarImage.fillAmount = loadValue;
        }
    }

    void LateUpdate()
    {  
        if (target != null)
            gameObject.transform.position = new Vector3(target.transform.position.x, 20, target.transform.position.z);

        if (recording)
        {
            screenShot();
        }

    }
   

    void screenShot()
    {
        
        int n = (int)System.Math.Floor(System.Math.Log10(imageNumber) + 1.0);
        string zeros = new string('0', 5 - n);
        string filename = Application.persistentDataPath + "/" + imageFolder + "/img" + zeros + imageNumber.ToString() + ".png";

        imageNumber++;

        RenderTexture rt = new RenderTexture(width, height, 24);
        Texture2D image = new Texture2D(width, height, TextureFormat.RGB24, false);
        replayCam.targetTexture = rt;
        replayCam.Render();
        RenderTexture.active = rt;
        image.ReadPixels(rectangle, 0, 0);
        replayCam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = image.EncodeToPNG();
        System.IO.File.WriteAllBytes(filename, bytes);

        //Application.CaptureScreenshot(filename);
        
    }
}
