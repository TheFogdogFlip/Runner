using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class TileNode {

    public string Name = "";

    public NColor Color = new NColor();

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
            this.a = 0.0f;
        }

        //public NColor(float r = 0, float g = 0, float b = 0, float a = 0)
        //{
        //    this.r = r;
        //    this.g = g;
        //    this.b = b;
        //    this.a = a;
        //}
    }
}
