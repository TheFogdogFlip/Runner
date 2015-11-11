using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

public class World : Component{

    private static Vector3 gridDimentions = new Vector3(2, 2, 2);

    public static Vector3 GridDimentions
    {
        get
        {
            return gridDimentions;
        }
    }

    private EmptyTile[,] grid;

    private int width;
    private int depth;

    public static World Instance;

    private World()
    {

    }

    public static void Init(string filename)
    {
        Instance = new World();
        Instance.load(filename);
    }

    public static void Init()
    {
        Instance = new World();
        Instance.generate();
    }

    public EmptyTile GetTile(int x, int y)
    {
        return grid[Mathf.FloorToInt(y / gridDimentions.y), Mathf.FloorToInt(x / gridDimentions.x)];
    }

    public void SetTile(int x, int y, EmptyTile tile)
    {
        grid[Mathf.FloorToInt(y / gridDimentions.y), Mathf.FloorToInt(x / gridDimentions.x)] = tile;
       
    }

    private void generate()
    {

    }

    public void Save(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(TileContainer));
        FileStream stream = new FileStream("TileNodes.xml", FileMode.OpenOrCreate);

        TileContainer container = new TileContainer();
        container.Nodes.Add(new TileNode() { Name = "Empty", Color = new TileNode.NColor() { r = 1.0f, g = 1.0f, b = 1.0f, a = 1.0f } });
        container.Nodes.Add(new TileNode() { Name = "Path", Color = new TileNode.NColor() { r = 0.0f, g = 0.0f, b = 0.0f, a = 1.0f } });

        serializer.Serialize(stream, container);
        stream.Close();
    }

    private void load(string filename)
    {

        XmlSerializer serializer = new XmlSerializer(typeof(TileContainer));
        FileStream stream = new FileStream("TileNodes.xml", FileMode.Open);

        var tiles = serializer.Deserialize(stream) as TileContainer;
        stream.Close();
        Texture2D texture = Resources.Load<Texture2D>(filename);

        int height = texture.height;
        int width = texture.width;

        this.grid = new EmptyTile[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color color = texture.GetPixel(x, y);

                TileNode tileType = tiles.Nodes.Find(n => new Color(n.Color.r, n.Color.g, n.Color.b, n.Color.a) == color);

                TileNode tn = new TileNode() { Name = "Path", Color = new TileNode.NColor() };

                EmptyTile tile = null;

                EmptyTile leftTile = null;
                EmptyTile bottomTile = null;

                if (x != 0)
                    leftTile = grid[y, x - 1];

                if (y != 0)
                    bottomTile = grid[y - 1, x];

                switch (tn.Name)
                {
                    case TileType.Path:
                        tile = new PathTile(new Vector3(x * gridDimentions.x, 0, y * gridDimentions.y));
                        if (bottomTile == null)
                            ((PathTile)tile).BottomWall = (GameObject)Instantiate(Resources.Load("Wall_Default"));

                        if (leftTile == null)
                            ((PathTile)tile).LeftWall = (GameObject)Instantiate(Resources.Load("Wall_Default"));

                        ((PathTile)tile).Floor = (GameObject)Instantiate(Resources.Load("Floor_Default"));

                        break;

                    case TileType.Empty:
                        tile = new EmptyTile(new Vector3(x * gridDimentions.x, 0, y * gridDimentions.y));
                        break;
                }
                this.grid[y, x] = tile;
            }
        }
    }
}
