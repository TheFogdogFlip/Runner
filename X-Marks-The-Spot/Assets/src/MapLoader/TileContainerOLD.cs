using System.Xml.Serialization;
using System.Collections.Generic;

[XmlRoot("TileContainer")]
public class TileContainerOLD{

    [XmlArray("TypeList")]
    [XmlArrayItem("TileNode")]
    public List<TileNode> Nodes = new List<TileNode>();
}
