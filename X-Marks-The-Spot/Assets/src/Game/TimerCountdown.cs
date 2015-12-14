using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerCountdown : MonoBehaviour
{
    /**---------------------------------------------------------------------------------
     * Variables used by the script.
     */
    public float f_time;
    public int i_time;
    public bool TimerFirstRunning;
    public bool TimerSecondRunning;
    
    /**---------------------------------------------------------------------------------
     * Text objects used by the script.
     */
    public Text countDownText;

    /**---------------------------------------------------------------------------------
     * Executed when the script starts.
     * Sets both bools to true to make the timer start running.
     */
    void 
    Start()
    {
        TimerFirstRunning           = true;
        TimerSecondRunning          = true;
    }

    /**---------------------------------------------------------------------------------
     * Executed on every frame.
     * Alters f_time by using Time.deltaTime.
     * Time.deltaTime is the time since the last time Update() was called (i.e. the time since the last frame).
     */
    void 
    Update()
    {
        f_time -= Time.deltaTime;
        i_time = (int)f_time;
    }

    /**---------------------------------------------------------------------------------
     * Sets the text to the countDownText depending on where the timer is at.
     * Hides the timer if enough time has passed.
     */
    public void 
    SetText()
    {
        if(f_time >= 1)
        {
            countDownText.text = i_time.ToString();
            return;
        }

        else if(f_time < 1 && f_time > 0)
        {
            countDownText.text = "GO!";
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

    /**---------------------------------------------------------------------------------
     * Hides the timer when "GO!" has been on the screen for a long enough time.
     */
    public void 
    HideTimer()
    {
        countDownText.enabled = false;
    }
}