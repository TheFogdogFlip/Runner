using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class TileContainer {

    [XmlAttribute("node")]
    public List<TileNode> Nodes;
}
