using UnityEngine;
using System.Collections;

public class ObstacleTile : Tile {

    public ObstacleTile(Vector3 position) : base(position)
    {
        this.Object = GameObject.FindGameObjectWithTag(ModelTypes.Obstacle);
    }
}
