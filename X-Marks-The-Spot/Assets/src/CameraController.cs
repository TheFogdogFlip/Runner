using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject Player;
    private Vector3 distance;
    private Vector3 holder;
	// Use this for initialization
	void Start () {
        distance = transform.position - Player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        holder.z = Player.transform.position.z;
        holder.x = Player.transform.position.x;
        transform.position = holder + distance;
        transform.Rotate(Player.transform.up * Time.deltaTime);
    }
}
