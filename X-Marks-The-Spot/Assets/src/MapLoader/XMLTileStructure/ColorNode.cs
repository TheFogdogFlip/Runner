using UnityEngine;

public class ColorNode {

    public float R;
    public float G;
    public float B;
    public float A;

    public Color ToColor()
    {
        return new Color(R / 255.0f, G / 255.0f, B / 255.0f, A / 255.0f);
    }
}
