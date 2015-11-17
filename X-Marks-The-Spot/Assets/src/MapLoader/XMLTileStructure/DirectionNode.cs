using System.Collections.Generic;
using System.Xml.Serialization;

public class DirectionNode
{
    public int Direction;
    [XmlArray("Connections")]
    [XmlArrayItem("ConnectionNode")]
    public List<ConnectionNode> Connections;

    public DirectionNode()
    {
        Direction = 0;
        Connections = new List<ConnectionNode>();
    }
}
