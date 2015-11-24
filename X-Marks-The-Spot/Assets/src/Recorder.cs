using UnityEngine;
using System.Collections;

public class Recorder : MonoBehaviour {
    int imageNumber;
    int fps;
    public float timer;
    public float timer2;
    int width;
    int height;
    Rect rectangle;
    public Transform target;
    // Use this for initialization
    void Start () {
        imageNumber = 0;
        fps = 0;
        timer = 0;
        width = Screen.width;
        height = Screen.height;
        rectangle = new Rect(0, 0, width, height);
        target = GameObject.Find("Player(Clone)").transform;
        
    }
    
    void Update()
    {
        gameObject.transform.position = new Vector3(target.position.x, 20, target.position.z);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(70, 0, 0));
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;
        if (timer >= 0.033f)
        {
            string filename = "C:/Users/eriset-4-local.A6284/Desktop/captured images/img" + imageNumber.ToString() + ".png";

            imageNumber++;
            fps++;

            Application.CaptureScreenshot(filename);
            timer = 0;

            //RenderTexture rt = new RenderTexture(width, height, 24);
            //cam.targetTexture = rt;
            //cam.Render();
            //RenderTexture.active = rt;

            //Texture2D image = new Texture2D(width, height);
            //image.ReadPixels(rectangle, 0, 0);
            //RenderTexture.active = null;
            //cam.targetTexture = null;
            //byte[] bytes;
            //bytes = image.EncodeToPNG();
            //System.IO.File.WriteAllBytes(filename, bytes);
            //timer = 0;
        }
        if(timer2 >= 1.0f)
        {
            print(fps);
            fps = 0;
            timer2 = 0;
        }
        
    }


}
