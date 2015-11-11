using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathTile : EmptyTile {

    public override Vector3 Position {
        get
        {
            return position;
        }
        set
        {
            position = value;
            if(floor != null)
                floor.transform.position = position;
            if (leftwall != null)
                leftwall.transform.position = position;
            if (bottomwall != null)
                bottomwall.transform.position = new Vector3(position.x + World.GridDimentions.x, position.y, position.z);
        }
    }

    private GameObject leftwall = null;

    public GameObject LeftWall
    {
        set
        {
            leftwall = value;
            leftwall.transform.position = position;
        }
        get { return leftwall; }
    }

    private GameObject bottomwall = null;

    public GameObject BottomWall
    {
        set
        {
            bottomwall = value;
            bottomwall.transform.position = new Vector3(position.x + World.GridDimentions.x, position.y, position.z);
        }
        get { return bottomwall; }
    }

    private GameObject floor = null;

    public GameObject Floor
    {
        set
        {
            floor = value;
            floor.transform.position = position;
        }
        get { return floor; }
    }

    public PathTile(Vector3 position) : base(position)
    {
        leftwall = null;
        bottomwall = null;
        floor = null;
    }
}
