using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

public class World{

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
        Texture2D generatedMap = new Texture2D(width, depth);

        var tilesTypes = getTileTypes();

        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //generatedMap.SetPixel(x, y, );
            }
        }

        //XmlSerializer serializer = new XmlSerializer(typeof(TileContainer));
        //FileStream stream = new FileStream("TileNodes.xml", FileMode.OpenOrCreate);
        //TileContainer container = new TileContainer();
        //container.Nodes.Add(new TileNode( "Empty", new TileNode.NColor(1.0f, 1.0f, 1.0f, 1.0f )));
        //container.Nodes.Add(new TileNode("Path"));

        //serializer.Serialize(stream, container);
        //stream.Close();
    }

    private TileContainer getTileTypes()
    {
        FileStream stream = new FileStream("TileNodes.xml", FileMode.Open);
        XmlSerializer serializer = new XmlSerializer(typeof(TileContainer));
        var tiles = serializer.Deserialize(stream) as TileContainer;
        stream.Close();

        return tiles;
    }

    private void load(string filename)
    {

        Texture2D texture = Resources.Load<Texture2D>(filename);

        int height = texture.height;
        int width = texture.width;

        this.grid = new EmptyTile[width, height];

        var tilesTypes = getTileTypes();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color color = texture.GetPixel(x, y);

                TileNode tileType = tilesTypes.Nodes.Find(n => new Color(n.Color.r / 255.0f, n.Color.g / 255.0f, n.Color.b / 255.0f, n.Color.a / 255.0f) == color);
                EmptyTile tile = null;
                if(tileType == null)
                    tile = new EmptyTile(new Vector3(x * gridDimentions.x, 0, y * gridDimentions.y));
                else if (tileType.Name.ToLower() == "empty")
                    tile = new EmptyTile(new Vector3(x * gridDimentions.x, 0, y * gridDimentions.y));
                else
                    tile = new PathTile(new Vector3(x * gridDimentions.x, 0, y * gridDimentions.y), tileType.Name, tileType.Rotation);



                //switch (tileType.Name)
                //{
                //    case TileType.Path:
                //        tile = new PathTile(new Vector3(x * gridDimentions.x, 0, y * gridDimentions.y));
                //        ((PathTile)tile).GameObject = (GameObject)Instantiate(Resources.Load("Path"));
                //        break;

                //    case TileType.Empty:
                //        tile = new EmptyTile(new Vector3(x * gridDimentions.x, 0, y * gridDimentions.y));
                //        break;
                //}
                this.grid[y, x] = tile;
            }
        }
    }
}
