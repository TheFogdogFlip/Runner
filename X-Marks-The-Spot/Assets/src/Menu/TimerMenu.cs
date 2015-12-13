using UnityEngine;
using System.Collections;

public class TimerMenu : MonoBehaviour 
{

    public float f_time;
    public int i_time;
    public bool isActive;

    /**---------------------------------------------------------------------------------
     * 
     */
    void 
    Start()
    {
        isActive = false;
    }

    /**---------------------------------------------------------------------------------
     * 
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
