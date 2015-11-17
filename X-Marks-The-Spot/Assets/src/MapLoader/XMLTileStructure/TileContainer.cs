using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("TileContainer")]
public class TileContainer {

    [XmlArray("TileList")]
    [XmlArrayItem("TileNode")]
    public List<TileNode> Tiles;
}
