using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject Player;
    private Vector3 distance;
    private Vector3 holder;
    private Vector3 startpos;
    private Vector3 axis;
    private float angle;
	// Use this for initialization
	void Start () {
        angle = 1;
        startpos[1] = -3;
        startpos[2] = 10;
        axis[0] = 0;
        axis[1] = 1;
        axis[2] = 0;
        transform.position = Player.transform.position - startpos;
        distance = transform.position - Player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        holder.z = Player.transform.position.z;
        holder.x = Player.transform.position.x;
        transform.position = holder + distance;
        transform.RotateAround(Player.transform.position, axis, angle);
    }
}
