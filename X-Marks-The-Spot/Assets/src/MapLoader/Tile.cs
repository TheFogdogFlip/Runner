using UnityEngine;

public class Tile : Object
{
    private GameObject gameObject = null;
    private float rotation = 0.0f;
    private Vector3 position;
    private Color color;

    public Vector3 Position {
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

    public Color Color
    {
        get
        {
            return color;
        }
        set
        {
            color = value;
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

    public Tile(Vector3 position, Color color, string gameObjectTag, float rotation = 0.0f)
    {
        GameObject = (GameObject)Instantiate(Resources.Load(gameObjectTag));
        Color = color;
        Position = position;
        Rotation = rotation;
    }
}
