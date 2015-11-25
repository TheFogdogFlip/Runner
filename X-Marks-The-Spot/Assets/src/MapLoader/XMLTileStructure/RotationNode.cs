using System.Collections.Generic;
using System.Xml.Serialization;

public class RotationNode
{
    public float Rotation;
    public ColorNode Color;

    [XmlArray("Directions")]
    [XmlArrayItem("DirectionNode")]
    public List<DirectionNode> Directions;

    public RotationNode()
    {
        Rotation = 0.0f;
        Color = new ColorNode();
        Directions = new List<DirectionNode>();
    }
}
