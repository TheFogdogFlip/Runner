
using System.Collections.Generic;
using System.Xml.Serialization;

public struct ConnectionNode {

    public string TileName;
    public float Chance;

    [XmlArray("Rotations")]
    [XmlArrayItem("RotationChanceNode")]
    public List<RotationChanceNode> Rotations;
}
