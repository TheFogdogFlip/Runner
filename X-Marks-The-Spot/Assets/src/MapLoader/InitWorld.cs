using UnityEngine;
using System.Collections;

public class InitWorld : MonoBehaviour {

    void Awake()
    {
        World.Init("TestMap");

        Instantiate(Resources.Load("Player", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(Resources.Load("PlayerCamera", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(new Vector3(0, 90, 0)));
    }

	// Use this for initialization
	void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
