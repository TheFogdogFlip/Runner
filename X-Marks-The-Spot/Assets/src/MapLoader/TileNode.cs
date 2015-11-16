using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class TileNode {

    public string Name = "";

    public NColor Color = new NColor();

    public float Rotation = 0.0f;

    public TileNode()
    {

    }

    public TileNode(string name, NColor color = null, float rotation = 0.0f)
    {
        Name = name;
        if (color != null) Color = color;
        else Color = new NColor();
        Rotation = rotation;
    }
}
