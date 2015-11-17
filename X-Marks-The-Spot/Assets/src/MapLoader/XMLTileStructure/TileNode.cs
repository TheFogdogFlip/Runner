using System.Collections.Generic;
using System.Xml.Serialization;

public struct TileNode {
    public string TileName;

    [XmlArray("Rotations")]
    [XmlArrayItem("RotatationNode")]
    public List<RotationNode> Rotations;

    [XmlArray("Directions")]
    [XmlArrayItem("DirectionNode")]
    public List<DirectionNode> Directions;
}
