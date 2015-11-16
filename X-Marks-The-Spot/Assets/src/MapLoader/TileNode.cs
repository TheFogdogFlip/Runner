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

    public class NColor
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public NColor()
        {
            this.r = 0.0f;
            this.g = 0.0f;
            this.b = 0.0f;
            this.a = 1.0f;
        }

        public NColor(float r, float g = 0.0f, float b = 0.0f, float a = 1.0f)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
    }
}
