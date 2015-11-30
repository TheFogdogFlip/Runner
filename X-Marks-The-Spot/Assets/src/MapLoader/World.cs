using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

public class TileDirectionNode : DirectionNode
{
    public int X;
    public int Y;

    public TileDirectionNode(int direction, int x, int y, List<ConnectionNode> connections)
    {
        this.Direction = direction;
        this.X = x;
        this.Y = y;
        this.Connections = connections;
    }
}

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

    private Tile[,] grid;

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

    public Tile GetTile(int x, int y)
    {
        return grid[Mathf.FloorToInt(y / gridDimentions.y), Mathf.FloorToInt(x / gridDimentions.x)];
    }

    public void SetTile(int x, int y, Tile tile)
    {
        grid[Mathf.FloorToInt(y / gridDimentions.y), Mathf.FloorToInt(x / gridDimentions.x)] = tile;
       
    }
    private static TileContainer tiles = null;

    private TileNode findTile(string name)
    {
        if (tiles == null)
            tiles = getTileTypes();
        return tiles.Tiles.Find(n => n.TileName.ToLower() == name.ToLower());
    }

    private ColorNode findColor(TileNode tile, float rotation)
    {
        var r = tile.Rotations.Find(c => c.Rotation == rotation);

        if (r == null)
        {
            switch ((int)rotation)
            {
                case 0:
                    rotation = 180;
                    break;
                case 90:
                    rotation = 270;
                    break;
                case 180:
                    rotation = 0;
                    break;
                case 270:
                    rotation = 90;
                    break;
            }
            r = tile.Rotations.Find(c => c.Rotation == rotation);
        }
        return r.Color;
    }

    private void generate()
    {
        width = 64;
        depth = 64;

        Texture2D generatedMap = new Texture2D(width, depth, TextureFormat.RGBA32, false);

        Color white = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < width; x++)
                generatedMap.SetPixel(y, x, white);
        }

        System.Random rand = new System.Random();

        int startX = rand.Next(width);
        int startZ = rand.Next(depth);

        start = new Vector3(startX, 0, startZ);

        var startTile = findTile("start");

        int direction = rand.Next(0, 3);

        int angle = direction * 90;
        startDirection = new Vector3(0, 0, angle);


        var color = findColor(startTile, angle).ToColor();
        generatedMap.SetPixel(startX, startZ, color);

        List<TileDirectionNode> directions = new List<TileDirectionNode>();

        RotationNode rotationNode = startTile.Rotations.Find(node => node.Rotation == angle);
        var newTileDirection = new TileDirectionNode(direction, startX, startZ, rotationNode.Directions[0].Connections);

        directions.Add(newTileDirection);

        while (true)
        {
            if (directions.Count == 0)
                break;

            var dir = directions[0];
            
            var connection = getRandomConnection(dir, rand);
            var tile = findTile(connection.TileName);
            var rotation = getRandomRotation(connection, rand);
            color = findColor(tile, rotation).ToColor();
            var rN = tile.Rotations.Find(r => r.Rotation == rotation);


            switch (dir.Direction)
            {
                case 0:
                    dir.Y += 1;
                    break;
                case 1:
                    dir.X += 1;
                    break;
                case 2:
                    dir.Y -= 1;
                    break;
                case 3:
                    dir.X -= 1;
                    break;
            }

            if (generatedMap.GetPixel(dir.X, dir.Y) == white)
            {
                generatedMap.SetPixel(dir.X, dir.Y, color);

                foreach (var item in rN.Directions)
                {
                    newTileDirection = new TileDirectionNode(item.Direction, dir.X, dir.Y, item.Connections);
                    directions.Add(newTileDirection);
                }
            }

            directions.Remove(dir);
        }
        save("Temp.png", generatedMap);
        load("Temp.png");
    }

    private float getRandomRotation(ConnectionNode connection, System.Random rand)
    {

        int randomNumber = rand.Next(0, 100);
        RotationChanceNode rotation = null;
        
        float chance = 0.0f;

        for (int i = 0; i < connection.Rotations.Count; i++)
        {
            rotation = connection.Rotations[i];
            if (randomNumber >= chance && randomNumber <= (int)connection.Chance + chance)
                break;
            chance += rotation.Chance;
        }
        return rotation.Rotation;
    }

    private ConnectionNode getRandomConnection(TileDirectionNode dir, System.Random rand)
    {

        int randomNumber = rand.Next(0, 100);
        ConnectionNode connection = null;

        float chance = 0.0f;

        for (int i = 0; i < dir.Connections.Count; i++)
        {
            connection = dir.Connections[i];
            if (randomNumber >= chance && randomNumber <= (int)connection.Chance + chance)
                break;
            chance += connection.Chance;
        }
        return connection;
    }

    private void save(string filename, Texture2D texture)
    {
        var rawMap = texture.EncodeToPNG();
        FileStream stream = new FileStream(filename, FileMode.OpenOrCreate);
        stream.Write(rawMap, 0, rawMap.Length);
        stream.Flush();
        stream.Close();
    }

    public void Save(string filename)
    {
        Texture2D generatedMap = new Texture2D(width, depth, TextureFormat.RGBA32, false);

        var tilesTypes = getTileTypes();

        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                try
                {
                    generatedMap.SetPixel(x, y, grid[y, x].Color);
                }
                catch
                {
                    generatedMap.SetPixel(x, y, new Color(1, 1, 1, 1));
                }
            }
        }
        var rawMap = generatedMap.EncodeToPNG();
        FileStream stream = new FileStream(filename, FileMode.OpenOrCreate);
        stream.Write(rawMap, 0, rawMap.Length);
        stream.Flush();
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

    private Texture2D loadTexture2D(string filename)
    {
        FileStream stream = new FileStream(filename, FileMode.Open);
        byte[] buffer = new byte[width * depth * 4];
        stream.Read(buffer, 0, buffer.Length);
        Texture2D texture = new Texture2D(width, depth, TextureFormat.RGBA32, false);
        texture.LoadImage(buffer);
        stream.Flush();
        stream.Close();

        return texture;
    }

    private void load(string filename)
    {
        Texture2D texture = loadTexture2D(filename);
        depth = texture.height;
        width = texture.width;

        grid = new Tile[width, depth];

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

                Tile tile = null;

                if (rotationN != null && !String.IsNullOrEmpty(tileN.TileName))
                {
                    tile = new Tile(new Vector3(x * gridDimentions.x, 0, y * gridDimentions.y), rotationN.Color.ToColor(), tileN.TileName, rotationN.Rotation);
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
