using UnityEngine;

public class EmptyTile : Component
{

    protected Vector3 position;
    protected Color color;

    public virtual Color Color
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

    public EmptyTile()
    {
    }

    public EmptyTile(Vector3 position, Color color)
    {
        this.position = position;
        this.color = color;
    }

}
