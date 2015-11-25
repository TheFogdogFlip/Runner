using System.Collections.Generic;
using System.Xml.Serialization;

public class TileNode
{
    public string TileName;

    [XmlArray("Rotations")]
    [XmlArrayItem("RotatationNode")]
    public List<RotationNode> Rotations;

    public TileNode()
    {
        Rotations = new List<RotationNode>();
    }
}
