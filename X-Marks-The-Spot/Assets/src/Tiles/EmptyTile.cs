using UnityEngine;
using System.Collections;

public class EmptyTile {

    protected Vector3 position;

    public virtual Vector3 Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
        }
    }

    public EmptyTile(Vector3 position)
    {
        this.position = position;
    }

}
