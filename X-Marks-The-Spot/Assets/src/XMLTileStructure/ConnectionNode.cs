using System.Collections.Generic;
using System.Xml.Serialization;

public class ConnectionNode
{
    public string TileName;
    public float Chance;

    [XmlArray("Rotations")]
    [XmlArrayItem("RotationChanceNode")]
    public List<RotationChanceNode> Rotations;

    public ConnectionNode()
    {
        TileName = "";
        Chance = 0.0f;
        Rotations = new List<RotationChanceNode>();
    }
}
