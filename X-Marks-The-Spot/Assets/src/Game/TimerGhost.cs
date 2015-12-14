using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerGhost : MonoBehaviour 
{
    /**---------------------------------------------------------------------------------
     * Variables used by the script.
     */
    public float f_time;
    public bool TimerRunning;
    
    /**---------------------------------------------------------------------------------
     * Executed when the script starts.
     */
	void 
    Start () 
    {
        //Empty
	}

    /**---------------------------------------------------------------------------------
     * Executed on every frame.
     * Alters f_time by using Time.deltaTime.
     * Time.deltaTime is the time since the last time Update() was called (i.e. the time since the last frame).
     */
    void 
    Update () 
    {
        f_time += Time.deltaTime;
	}
}
