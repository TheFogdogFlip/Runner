using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathTile : EmptyTile {

    private GameObject gameObject = null;
    private float rotation = 0.0f;

    public override Vector3 Position {
        get
        {
            return position;
        }
        set
        {
            position = value;
            if(gameObject != null)
                gameObject.transform.position = position;
        }
    }

    public float Rotation
    {
        get { return rotation; }
        set
        {
            rotation = value;
            if (gameObject != null)
                gameObject.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), rotation);
        }
    }

    public GameObject GameObject
    {
        set
        {
            gameObject = value;
            if (gameObject != null)
            {
                gameObject.transform.position = position;
                gameObject.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), rotation);
            }
        }
        get { return gameObject; }
    }

    public PathTile(Vector3 position, string gameObjectTag, float rotation = 0.0f)
    {
        GameObject = (GameObject)Instantiate(Resources.Load(gameObjectTag)); ;
        Position = position;
        Rotation = rotation;
    }
}
