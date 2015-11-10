using UnityEngine;
using System.Collections;

public class WallTile : Tile {

    public WallTile(Vector3 position) : base(position)
    {
        this.Object = GameObject.FindGameObjectWithTag(ModelTypes.Wall);
    }
}
