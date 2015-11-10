using UnityEngine;
using System.Collections;

public class PitTile : Tile {

	public PitTile(Vector3 position) : base(position)
    {
        this.Object = GameObject.FindGameObjectWithTag(ModelTypes.Pit);
    } 
}
