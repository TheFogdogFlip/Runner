﻿using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private GameObject gameObject = null;
    private float rotation = 0.0f;
    private Vector3 position;
    private Color color;

    private static Dictionary<string, GameObject> gameObjects = new Dictionary<string,GameObject>();

    public Vector3 Position
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
        GameObject gameObject = null;

        if(gameObjects.ContainsKey(gameObjectTag))
            gameObject = gameObjects[gameObjectTag];
        else
        {
            gameObject = Resources.Load<GameObject>(gameObjectTag);
            gameObjects[gameObjectTag] = gameObject;
        }

        GameObject = Object.Instantiate<GameObject>(gameObject);
        Color = color;
        Position = position;
        Rotation = rotation;
    }
}