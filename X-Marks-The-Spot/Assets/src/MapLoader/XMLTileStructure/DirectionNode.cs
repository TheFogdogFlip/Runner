using System.Collections.Generic;
using System.Xml.Serialization;

public struct DirectionNode {

    public int Direction;
    [XmlArray("Connections")]
    [XmlArrayItem("ConnectionNode")]
    public List<ConnectionNode> Connections;
}
