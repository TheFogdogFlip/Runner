using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerPlayer : MonoBehaviour {
    public float f_time;
    public Text textObj;
    public bool TimerRunning;

    /**---------------------------------------------------------------------------------
     * 
     */
	void 
    Start () 
    {
        //Empty
	}

    /**---------------------------------------------------------------------------------
     * 
     */
	void 
    Update () 
    {
        f_time += Time.deltaTime;
	}

    /**---------------------------------------------------------------------------------
     * 
     */
    public void 
    SetText()
    {
        int i_time = (int)f_time;
        textObj.text = i_time.ToString();
    }
}
