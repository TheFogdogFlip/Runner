using System.Collections.Generic;
using System.Xml.Serialization;

public class TileNode
{
    public string TileName;

    [XmlArray("Rotations")]
    [XmlArrayItem("RotatationNode")]
    public List<RotationNode> Rotations;

    [XmlArray("Directions")]
    [XmlArrayItem("DirectionNode")]
    public List<DirectionNode> Directions;

    public TileNode()
    {
        Rotations = new List<RotationNode>();
        Directions = new List<DirectionNode>();
    }
}
