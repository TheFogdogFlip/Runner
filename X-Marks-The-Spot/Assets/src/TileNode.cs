using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class TileNode {

    [XmlAttribute("name")]
    public string Name;

    [XmlAttribute("color")]
    public Color Color;
}
