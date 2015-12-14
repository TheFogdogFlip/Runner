using UnityEngine;
using System.Collections;

public class TimerMenu : MonoBehaviour 
{
    /**---------------------------------------------------------------------------------
     * Variables used by the script.
     */
    public float f_time;
    public int i_time;
    public bool isActive;

    /**---------------------------------------------------------------------------------
     * Executed when the script starts.
     * Sets isActive to false because the timer should not start right away.
     */
    void 
    Start()
    {
        isActive = false;
    }

    /**---------------------------------------------------------------------------------
     * Executed on every frame.
     * Checks if isActive is true. If it is, the timer has been started.
     * Alters f_time by using Time.deltaTime.
     * Time.deltaTime is the time since the last time Update() was called (i.e. the time since the last frame).
     */
    void 
    Update()
    {
        if (isActive)
        {
            f_time += Time.deltaTime;
            i_time = (int)f_time;
        }
    }
}
