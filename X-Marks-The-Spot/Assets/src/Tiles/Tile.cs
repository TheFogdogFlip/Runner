using UnityEngine;
using System.Collections;

public class Tile {

    public Vector3 Position {
        get
        {
            return Position;
        }
        set
        {
            Position = value;
        }
    } 
    public GameObject Object
    {
        get
        {
            return Object;
        }
        set
        {
            Object = value;
        }
    }

    public Tile(Vector3 position)
    {
        this.Position = position;
    }
}
