using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private GameObject gameObject = null;
    private float rotation = 0.0f;
    private Vector3 position;
    private Color color;

    /**---------------------------------------------------------------------------------
    *   Used for Tile cache. So you don't have to read from disk for each tile.
    */
    private static Dictionary<string, GameObject> gameObjectsCache = new Dictionary<string,GameObject>();

    /**---------------------------------------------------------------------------------
    *   Used to get and set position of the tile.
    */
    public Vector3 
    Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
            if (gameObject != null)
                gameObject.transform.position = position;
        }
    }

    /**---------------------------------------------------------------------------------
    *   Holds the color value of the tile.
    */
    public Color 
    Color
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

    /**---------------------------------------------------------------------------------
    *   Used to get and set the rotation of the tile.
    */
    public float 
    Rotation
    {
        get { return rotation; }
        set
        {
            rotation = value;
            if (gameObject != null)
                gameObject.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), rotation);
        }
    }

    /**---------------------------------------------------------------------------------
    *   Used to get and set the Tile prefab for the tile.
    */
    public GameObject 
    GameObject
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

    /**---------------------------------------------------------------------------------
    *   Constructor. Adds unseen tile types to the game chache and then 
    *   instantiates the tile at given position and rotation.
    */
    public 
    Tile(Vector3 position, Color color, string gameObjectTag, float rotation = 0.0f)
    {
        GameObject gameObject = null;

        if (gameObjectsCache.ContainsKey(gameObjectTag))
            gameObject = gameObjectsCache[gameObjectTag];
        else
        {
            gameObject = Resources.Load<GameObject>(gameObjectTag);
            gameObjectsCache[gameObjectTag] = gameObject;
        }

        GameObject = Object.Instantiate<GameObject>(gameObject);
        Color = color;
        Position = position;
        Rotation = rotation;
    }
}