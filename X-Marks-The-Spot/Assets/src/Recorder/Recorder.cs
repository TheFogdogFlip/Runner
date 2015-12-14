using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

public class Recorder : MonoBehaviour 
{
    /**---------------------------------------------------------------------------------
     * Variabels to adjust replay camera.
     */
    public int cameraHeight;
    public int cameraAngle;
    public int cameraZoffset;

    /**---------------------------------------------------------------------------------
     * Adjustable capture variables.
     */
    public string imageFolder = "captured_images";
    public int fps = 30;

    /**---------------------------------------------------------------------------------
     * Various variables for replay player, ghost and camera follow target.
     */
    public float timer;
    GameObject target = null;
    public List<TimeStamp> inputs;
    public List<List<TimeStamp>> ghostinputs;
    public List<GameObject> ghosts;
    public float finishedTime;
    private TimerGhost ghostTimerObj;

    /**---------------------------------------------------------------------------------
     * Variables associated with screenshots.
     */
    int imageNumber; //counter for current recorded image.
    private Camera replayCam; //camera following replay player.
    private int width;
    private int height;
    private Rect rectangle; //rectangle used for capturing area.

    /**---------------------------------------------------------------------------------
     * variables for recording/video generating overlay and generation process.
     */
    private float loadValue;
    private Image loadBarImage;
    private Text loadText;
    private Text loadPercentageText;

    private bool loading = false;
    public bool recording = false;
    private bool generating = false;
    
    Process process;
    StreamReader reader;


    /**---------------------------------------------------------------------------------
     * Exectutes at the start of replay, setup som valuesm destroys some player related objects
     * makes sure record folder exists and starts audio 
     */
    private void 
    Start() 
    {
        loadValue = 0.0f;
        replayCam = gameObject.GetComponent<Camera>();

        GameObject playerObject = GameObject.Find("Player(Clone)");
        Destroy(playerObject);

        GameObject cam = GameObject.Find("PlayerCamera(Clone)");
        cam.GetComponent<CameraController>().enabled = false;
        Destroy(cam);


        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/" + imageFolder))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/" + imageFolder);
        }

        else
        {
            foreach (string path in System.IO.Directory.GetFiles(Application.persistentDataPath + "/" + imageFolder))
            {
                System.IO.File.Delete(path);
            }
        }

        AudioManager.Instance.startLoop();
        setupRun(false);
    }

    /**---------------------------------------------------------------------------------
     * Sets up necessary variables, resets ghost timer and destroys not yet destroyed ghosts.
     */
    private void 
    setupRun(bool r)
    {
        width = Screen.width;
        height = Screen.height;
        rectangle = new Rect(0, 0, width, height);
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
        {
            target = Instantiate(Resources.Load("ReplayPlayer", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection)) as GameObject;
        }
        else
        {
            GameObject temp = target;
            target = Instantiate(Resources.Load("ReplayPlayer", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection)) as GameObject;
            Destroy(temp);
        }

        gameObject.transform.position = new Vector3(target.transform.position.x, cameraHeight, target.transform.position.z + cameraZoffset);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(cameraAngle, 0, 0));
        ghost = target.GetComponent<Ghost>();
        ghost.inputs = inputs;

        SetupGhostTimer();

        //If recording capture framerate needs to be set to get consistent video
        if (recording)
        {
            Time.captureFramerate = fps;
        }

    }

    /**---------------------------------------------------------------------------------
     * Setting up ghost timer, same as in playerscript.
     */
    public void 
    SetupGhostTimer()
    {
        GameObject ghostTimerGameObj = GameObject.Find("ghostTimer");
        ghostTimerObj = ghostTimerGameObj.GetComponent<TimerGhost>();
        ghostTimerObj.f_time = 0;
        ghostTimerObj.TimerRunning = true;

    }

    /**---------------------------------------------------------------------------------
    * Setting up for recording, Instantiating overlay objects.
    */
    private void 
    SetupRecording()
    {
        foreach (GameObject go in ghosts)
        {
            Destroy(go);
        }
        setupRun(true);
        loading = true;
        Instantiate(Resources.Load("RecordingOverlay", typeof(Canvas)));
        loadBarImage = GameObject.Find("LoadBar").GetComponent<Image>();
        loadText = GameObject.Find("LoadText").GetComponent<Text>();
        loadPercentageText = GameObject.Find("PercentageText").GetComponent<Text>();
    }

    /**---------------------------------------------------------------------------------
     * Executes every frame, Video recording and generation is done here.
     */
    void 
    Update()
    {
        //Can be removed for a option in some replay menu
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetupRecording();
        }

        //Whole recording and generation process
        if (loading)
        {
            //Updating loading value while recording
            if (ghostTimerObj.f_time <= finishedTime && recording)
            {
                loadText.text = "Recording Video";
                loadValue = 0.5f * ghostTimerObj.f_time / finishedTime;
            }
            else
            {
                recording = false;
            }

            //Generating setup
            if (!recording && !generating)
            {
                generating = true;
                loadText.text = "Generating Video";
                string filepath = Application.persistentDataPath;

                try
                {
                    process = new Process();

                    process.EnableRaisingEvents = true;
                    process.StartInfo.FileName = "ffmpeg.exe"; //executable to use
                    process.StartInfo.Arguments = 
                    "-framerate " + fps.ToString()                              //Sets number of images per second to use for input
                    + " -i \"" + filepath + "/" + imageFolder +"/img%05d.png\" "   //image input path declaration 
                    //(%05d works as it does with printf in c++, could be for emaple img00001.png or img99999.png or anything inbetween)
                    + " -i Dreamworld.mp3"                                      //audio input path declaration
                    + " -c:v libx264"                                           //sets videocodec to h.264
                    + " -vf fps=" + fps.ToString()                              //sets video output framerate
                    + " -pix_fmt yuv420p"                                       //sets pixelformat(not exactly sure what yuv420p is, seems to be needed for this to work).
                    + " -shortest \""                                           //cuts whatever is the longer of audio/video to the lenght of the shortest input 
                    + filepath + "/out.mp4\" -y";                               //video output path -y makes ffmpeg skip asking to start

                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardError = true; // used to get values for loadingbar
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                    //Does not allways seem to work, could be unity tho
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

            //Geration process, reading messages from ffmpeg for use on loadingbar progression
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
                            loadValue = 0.5f + 0.5f * int.Parse(match.Groups[1].Value) / imageNumber;
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
            loadPercentageText.text = System.Math.Ceiling(loadValue*100) + "%";
            
        }
       
    }

    /**---------------------------------------------------------------------------------
     * Executes when last every frame, makes replay camera follow replayplayer.
     * Screenshot is put here so everything is rendered and moved for this frame.
     */
    private void 
    LateUpdate()
    {
        if (target != null)
        {
            gameObject.transform.position = new Vector3(target.transform.position.x, cameraHeight, target.transform.position.z + cameraZoffset);
        }

        if (recording)
        {
            screenShot();
        }
    }

    /**---------------------------------------------------------------------------------
     * Executes every frame while recording(Default: 30 per second).
     * Takes image from a specific camera(in this case replay camera.
     */
    private void 
    screenShot()
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
    }
}
