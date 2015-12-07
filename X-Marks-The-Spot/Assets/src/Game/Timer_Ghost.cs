using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer_Ghost : MonoBehaviour {
    public float f_time;
    public Text textObj;
    public bool TimerRunning;

	// Use this for initialization
	void Start () 
    {
        print("start ghosttimer");
	}
	
	// Update is called once per frame
	void Update () 
    {
        print("update ghosttimer");
        f_time += Time.deltaTime;
	}
    public void SetText()
    {
        int i_time = (int)f_time;
        textObj.text = i_time.ToString();
    }
}
