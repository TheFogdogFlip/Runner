using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer_Countdown : MonoBehaviour
{
    public float f_time;
    public int i_time;
    public bool TimerFirstRunning = true;
    public bool TimerSecondRunning = true;
    public Text textObj;

    void Start()
    {

    }

    void Update()
    {
        f_time -= Time.deltaTime;
        i_time = (int)f_time;
    }

    public void SetText()
    {
        if(f_time >= 1)
        {
            textObj.text = i_time.ToString();
            return;
        }
        else if(f_time < 1 && f_time > 0)
        {
            textObj.text = "GO!";
            TimerFirstRunning = false;
            return;
        }
        else
        {
            TimerSecondRunning = false;
            HideTimer();
            return;
        }
        
    }
    public void HideTimer()
    {
        textObj.enabled = false;
    }
}