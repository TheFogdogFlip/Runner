using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

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

public class EndPosition
{
    public int X;
    public int Y;
    public float Rotation;
}

public class World
{
    private static World instance;

    private Texture2D map;

    private int width;
    private int depth;
    private Tile[,] grid;

    private static Vector3 gridDimentions = new Vector3(2, 2, 2);
    private Vector3 start;
    private Vector3 startDirection;

    private static TileContainer tiles = null;

    public static World Instance
    {
        get
        {
            if (instance == null)
                instance = new World();

            return instance;
        }
    }

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

    private World()
    {

    }

    public Tile GetTile(int x, int y)
    {
        return grid[Mathf.FloorToInt(y / gridDimentions.y), Mathf.FloorToInt(x / gridDimentions.x)];
    }

    public void SetTile(int x, int y, Tile tile)
    {
        grid[Mathf.FloorToInt(y / gridDimentions.y), Mathf.FloorToInt(x / gridDimentions.x)] = tile;
       
    }

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

    public void Generate()
    {
        System.Random rand = new System.Random();

        width = 256;
        depth = 256;

        map = new Texture2D(width, depth, TextureFormat.RGBA32, false);

        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < width; x++)
                map.SetPixel(y, x, Color.white);
        }

        int startX = 128;//rand.Next(width);
        int startZ = 128;//rand.Next(depth);

        start = new Vector3(startX, 0, startZ);

        var startTile = findTile("start");

        int direction = rand.Next(0, 3);

        int angle = direction * 90;
        startDirection = new Vector3(0.0f, angle, 0.0f);

        var color = findColor(startTile, angle).ToColor();
        map.SetPixel(startX, startZ, color);

        List<TileDirectionNode> directions = new List<TileDirectionNode>();
        List<EndPosition> ends = new List<EndPosition>();


        RotationNode rotationNode = startTile.Rotations.Find(node => node.Rotation == angle);
        directions.Add(new TileDirectionNode(direction, startX, startZ, rotationNode.Directions[0].Connections));

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

            if (map.GetPixel(dir.X, dir.Y) == Color.white)
            {
                map.SetPixel(dir.X, dir.Y, color);
                if (tile.TileName.ToLower() == "end".ToLower())
                    ends.Add(new EndPosition() { X = dir.X, Y = dir.Y, Rotation = rN.Rotation });
                foreach (var item in rN.Directions)
                    directions.Add(new TileDirectionNode(item.Direction, dir.X, dir.Y, item.Connections));
            }
            directions.Remove(dir);
        }

        var end = getRandomEnd(ends, rand);
        var finishTile = findTile("finish");
        color = findColor(finishTile, end.Rotation).ToColor();
        map.SetPixel(end.X, end.Y, color);

        loadFromMemory();
    }

    private EndPosition getRandomEnd(List<EndPosition> ends, System.Random rand)
    {
        float randomNumber = (float)rand.NextDouble() * 100.0f;
        float itemChance = 100.0f / ends.Count;
        float chance = 0.0f;

        EndPosition end = null;

        for (int i = 0; i < ends.Count; i++)
        {
            end = ends[i];
            if (randomNumber >= chance && randomNumber <= itemChance + chance)
                break;
            chance += itemChance;
        }
        return end;
    }

    private float getRandomRotation(ConnectionNode connection, System.Random rand)
    {

        float randomNumber = (float)rand.NextDouble() * 100.0f;
        RotationChanceNode rotation = null;
        
        float chance = 0.0f;

        for (int i = 0; i < connection.Rotations.Count; i++)
        {
            rotation = connection.Rotations[i];
            if (randomNumber >= chance && randomNumber <= connection.Chance + chance)
                break;
            chance += rotation.Chance;
        }
        return rotation.Rotation;
    }

    private ConnectionNode getRandomConnection(TileDirectionNode dir, System.Random rand)
    {

        float randomNumber = (float)rand.NextDouble() * 100.0f;
        ConnectionNode connection = null;

        float chance = 0.0f;

        for (int i = 0; i < dir.Connections.Count; i++)
        {
            connection = dir.Connections[i];
            if (randomNumber >= chance && randomNumber <= connection.Chance + chance)
                break;
            chance += connection.Chance;
        }
        return connection;
    }

    public void Save(string filename)
    {
        var rawMap = map.EncodeToPNG();
        FileStream stream = new FileStream(filename, FileMode.OpenOrCreate);
        stream.Write(rawMap, 0, rawMap.Length);
        stream.Flush();
        stream.Close();
    }

    private TileContainer getTileTypes()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TileNodes");
        StringReader stringStream = new StringReader(textAsset.text);

        XmlSerializer serializer = new XmlSerializer(typeof(TileContainer));
        var tiles = serializer.Deserialize(stringStream) as TileContainer;
        stringStream.Close();

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

    private void loadFromMemory()
    {
        depth = map.height;
        width = map.width;

        grid = new Tile[width, depth];

        var tilesTypes = getTileTypes();

        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color color = map.GetPixel(x, y);

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
                        startDirection = new Vector3(0.0f, rotationN.Rotation, 0.0f);
                    }
                }
                this.grid[y, x] = tile;
            }
        }
    }

    public void Load(string filename)
    {
        map = loadTexture2D(filename);
        depth = map.height;
        width = map.width;

        grid = new Tile[width, depth];

        var tilesTypes = getTileTypes();

        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color color = map.GetPixel(x, y);

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
                        startDirection = new Vector3(0.0f, rotationN.Rotation, 0.0f);
                    }
                }
                this.grid[y, x] = tile;
            }
        }
    }
}
