using UnityEngine;
using System.Collections;

public class Timer_Menu : MonoBehaviour 
{

    public float f_time;
    public int i_time;

    void Start()
    {

    }

    void Update()
    {
        f_time += Time.deltaTime;
    }
}
