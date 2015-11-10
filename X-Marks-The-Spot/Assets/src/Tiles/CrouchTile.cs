using UnityEngine;
using System.Collections;

public class CrouchTile : Tile {

	public CrouchTile(Vector3 position) : base(position)
    {
        this.Object = GameObject.FindGameObjectWithTag(ModelTypes.Crouch);
    }
}
