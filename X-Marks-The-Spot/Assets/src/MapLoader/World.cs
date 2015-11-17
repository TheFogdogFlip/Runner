using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

public class World{

    private static Vector3 gridDimentions = new Vector3(2, 2, 2);
    private Vector3 start;
    private Vector3 startDirection;


    public static Vector3 GridDimentions
    {
        get
        {
            return gridDimentions;
        }
    }

    public Vector3 StartPosition
    {
        get
        {
            return start;
        }
    }

    public Vector3 StartDirection
    {
        get
        {
            return startDirection;
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
        //var tileTypes = getTileTypes();

        //int width = 64;
        //int height = 64;

        //Texture2D generatedMap = new Texture2D(width, height, TextureFormat.RGBA32, false);
        //bool done = false;
        //System.Random rand = new System.Random(System.DateTime.Now.Millisecond);

        //Vector3 startTile = new Vector3(rand.Next(width), 0, rand.Next(height));

        //Dictionary<string, Color> colorLookupTable = new Dictionary<string, Color>();

        //foreach(var tile in tileTypes.Nodes)
        //{

        //}

        ////generatedMap[startTile.x, startTile.z] = new Color();

        //int x = 0;
        //int z = 0;

        //switch(rand.Next(0,1))
        //{
        //    case 0:
        //        x = rand.Next(0, 1) == 0 ? -1 : 1;
        //        break;
        //    case 1:
        //        z = rand.Next(0, 1) == 0 ? -1 : 1;
        //        break; 
        //}

        //startDirection = new Vector3(x, 0, z);

        //while(!done)
        //{

        //}
    }

    public void Save(string filename)
    {
        Texture2D generatedMap = new Texture2D(width, depth, TextureFormat.RGBA32, false);

        var tilesTypes = getTileTypes();

        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                generatedMap.SetPixel(x, y, grid[y, x].Color);
            }
        }
        var rawMap = generatedMap.EncodeToPNG();
        FileStream stream = new FileStream(filename, FileMode.OpenOrCreate);
        stream.Write(rawMap, 0, rawMap.Length);
        stream.Flush();
        stream.Close();
    }

    private void generateTestXMLData()
    {
        TileContainer container = new TileContainer();
        container.Tiles = new List<TileNode>();
        TileNode node = new TileNode();
        node.TileName = "Path";
        node.Directions = new List<DirectionNode>();
        DirectionNode dNode = new DirectionNode();
        dNode.Direction = 1;
        dNode.Connections = new List<ConnectionNode>();
        ConnectionNode cNode = new ConnectionNode() { TileName = "Corner" };
        cNode.Rotations = new List<RotationChanceNode>();
        cNode.Chance = 20;
        cNode.Rotations.Add(new RotationChanceNode() {  Chance = 20, Rotation = 0});
        dNode.Connections.Add(cNode);
        node.Rotations = new List<RotationNode>();
        RotationNode rNode = new RotationNode();
        rNode.Rotation = 0;
        rNode.Color = new ColorNode() { R = 0, G = 0, B = 0, A = 255 };
        node.Rotations.Add(rNode);
        node.Directions.Add(dNode);
        container.Tiles.Add(node);
        FileStream stream = new FileStream("TileNodes.xml", FileMode.Open);
        XmlSerializer serializer = new XmlSerializer(typeof(TileContainer));
        serializer.Serialize(stream, container);
        stream.Close();
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

        depth = texture.height;
        width = texture.width;

        grid = new EmptyTile[width, depth];

        var tilesTypes = getTileTypes();

        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color color = texture.GetPixel(x, y);

                RotationNode rotationN = null;
                TileNode tileN = null;

                foreach (var tileNode in tilesTypes.Tiles)
                {
                    try
                    {
                        rotationN = tileNode.Rotations.Where(r => r.Color.ToColor() == color).First();
                        tileN = tileNode;
                    }
                    catch
                    {
                        // No Element found
                        continue;
                    }
                    break;
                }

                PathTile tile = null;

                if (rotationN != null)
                {
                    tile = new PathTile(new Vector3(x * gridDimentions.x, 0, y * gridDimentions.y), rotationN.Color.ToColor(), tileN.TileName, rotationN.Rotation);
                    if (tileN.TileName.ToLower() == "start")
                    {
                        start = tile.Position;
                        if (rotationN.Rotation == 0.0f)
                            startDirection = new Vector3(0, 0, -1);
                        else if (rotationN.Rotation == 90.0f)
                            startDirection = new Vector3(-1, 0, 0);
                        if (rotationN.Rotation == 180.0f)
                            startDirection = new Vector3(0, 0, 1);
                        else
                            startDirection = new Vector3(1, 0, 0);
                    }
                }
                this.grid[y, x] = tile;
            }
        }
    }
}
