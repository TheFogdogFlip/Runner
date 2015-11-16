
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
        this.a = 255.0f;
    }

    public NColor(float r, float g = 0.0f, float b = 0.0f, float a = 255.0f)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }
}
