using UnityEngine;
using System.Collections;

public class InitWorld : MonoBehaviour {

    void Awake()
    {
        World.Instance.Generate();

        Instantiate(Resources.Load("Player", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection));
        Instantiate(Resources.Load("PlayerCamera", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection));
    }

	// Use this for initialization
	void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
