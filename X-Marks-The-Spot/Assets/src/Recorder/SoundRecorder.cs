using UnityEngine;
using System.Collections;
using System.IO;

public class SoundRecorder : MonoBehaviour {

    private int bufferSize;
    private int numBuffers;
    private int outputRate;
    private string fileName;
    private int headerSize;
    private bool checkheader;
    private FileStream fileStream;

	// Use this for initialization
	void Start () {
        outputRate = 44100;
        fileName = "recordTest.wav";
        headerSize = 44;
        AudioSettings.outputSampleRate = outputRate;
        AudioSettings.GetDSPBufferSize(out bufferSize, out numBuffers);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("r")){
            print("rec");
            if (checkheader == true){

            }
        }
	}
    void StartWriting(string fileName){
        fileStream = new FileStream(fileName, FileMode.Create);
        byte emptyByte = new byte();
        for (int i = 0; i < headerSize; i++){
            fileStream.WriteByte(emptyByte);
        }
    }

}
