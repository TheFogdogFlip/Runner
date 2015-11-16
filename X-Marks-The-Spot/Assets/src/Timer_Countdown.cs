using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer_Countdown : MonoBehaviour
{
    public float f_timeRemaining = 3;
    public int i_timeRemaining;
    public bool TimerRunning = true;
    public bool TimerRunning_Second = true;
    
    public void SetText(Text textObj)
    {
        if(f_timeRemaining > 0.5)
        {
            textObj.text = i_timeRemaining.ToString();
        }
        if(i_timeRemaining == 0)
        {
            textObj.text = "GO!";
            TimerRunning = false;
            
        }
        else if(i_timeRemaining <= 0)
        {
            TimerRunning_Second = false;            
        }
    }
}
