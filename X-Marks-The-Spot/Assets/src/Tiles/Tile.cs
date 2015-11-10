using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public List<GameObject> Objects = new List<GameObject>();

    public Tile(Vector3 position)
    {
        this.Position = position;
    }
}
