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

    /**---------------------------------------------------------------------------------
     * Constructor
     */
    public
    TileDirectionNode(int direction, int x, int y, List<ConnectionNode> connections)
    {
        Direction = direction;
        X = x;
        Y = y;
        Connections = connections;
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

    private Texture2D map = new Texture2D(width, depth, TextureFormat.RGBA32, false);

    private const int width = 256;
    private const int depth = 256;
    private Tile[,] grid;

    private static Vector3 gridDimentions = new Vector3(2, 2, 2);
    private Vector3 start;
    private Vector3 startDirection;

    /**---------------------------------------------------------------------------------
    *   Used for storing mapcolors which will be used to load the map in the main thread.
    */
    ColorNode[,] mapcolor = new ColorNode[depth, width];

    private TileContainer tileContainer = null;

    /**---------------------------------------------------------------------------------
    *   Container for all the different tile types. Tile data cache.
    */
    private TileContainer
    tileTypes
    {
        get
        {
            if (tileContainer == null)
                tileContainer = getTileTypes();

            return tileContainer;
        }
    }

    /**---------------------------------------------------------------------------------
    *   Holds the instance of the singleton class.
    */
    public static World
    Instance
    {
        get
        {
            if (instance == null)
                instance = new World();

            return instance;
        }
    }

    /**---------------------------------------------------------------------------------
    *   Grid dimentions used for each tile.
    */
    public static Vector3
    GridDimentions
    {
        get
        {
            return gridDimentions;
        }
    }

    /**---------------------------------------------------------------------------------
    *   Indicates the starting position of the start tile in the world.
    */
    public Vector3
    StartPosition
    {
        get
        {
            return start;
        }
    }

    /**---------------------------------------------------------------------------------
    *   Inidicates which direction the start tile is pointing at.
    */
    public Vector3
    StartDirection
    {
        get
        {
            return startDirection;
        }
    }

    /**---------------------------------------------------------------------------------
    *   Load the XML file with all the tile data.
    */
    public void
    LoadXML()
    {
        tileContainer = getTileTypes();
    }

    /**---------------------------------------------------------------------------------
    *   Constructor made private so only one instance can be created.
    */
    private
    World()
    {

    }

    /**---------------------------------------------------------------------------------
    *   Gets the tile at specified position in the world.
    */
    public Tile
    GetTile(int x, int y)
    {
        return grid[Mathf.FloorToInt(y / gridDimentions.y), Mathf.FloorToInt(x / gridDimentions.x)];
    }

    /**---------------------------------------------------------------------------------
    *   Sets the tile at specified position in the world.
    */
    public void
    SetTile(int x, int y, Tile tile)
    {
        grid[Mathf.FloorToInt(y / gridDimentions.y), Mathf.FloorToInt(x / gridDimentions.x)] = tile;
    }

    /**---------------------------------------------------------------------------------
    *   Marks all the tiles to don't be destoyed so they will appear in the next scene;
    */
    public void
    UseGenerated()
    {
        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Tile tile = grid[y, x];
                try
                {
                    if (tile != null)
                        UnityEngine.Object.DontDestroyOnLoad(tile.GameObject);
                }
                catch
                {

                }
            }
        }
    }

    /**---------------------------------------------------------------------------------
    *   Finds the tile with specified name.
    */
    private TileNode
    findTile(string name)
    {
        return tileTypes.Tiles.Find(n => n.TileName.ToLower() == name.ToLower());
    }

    /**---------------------------------------------------------------------------------
    *   Finds the color of specified tile with a specific rotation
    */
    private ColorNode
    findColor(TileNode tile, float rotation)
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

    /**---------------------------------------------------------------------------------
    *   Sets the texture color for each pixel, in the main thread.
    */
    public void 
    SetMapColor()
    {
        map = new Texture2D(width, depth, TextureFormat.RGBA32, false);
        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                map.SetPixel(x, y, mapcolor[y, x].ToColor());
            }
        }
    }

    /**---------------------------------------------------------------------------------
    *   Generates a random world
    */
    public void
    Generate()
    {
        System.Random rand = new System.Random();

        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < width; x++)
                mapcolor[y, x] = new ColorNode(255.0f, 255.0f, 255.0f, 255.0f);
        }

        int startX = 128;
        int startZ = 128;

        start = new Vector3(startX, 0, startZ);

        var startTile = findTile("start");

        int direction = rand.Next(0, 3);

        int angle = direction * 90;
        startDirection = new Vector3(0.0f, angle, 0.0f);

        var color = findColor(startTile, angle);
        mapcolor[startZ, startX] = color;

        List<TileDirectionNode> directions = new List<TileDirectionNode>();
        List<EndPosition> ends = new List<EndPosition>();

        RotationNode rotationNode = startTile.Rotations.Find(node => node.Rotation == angle);
        directions.Add(new TileDirectionNode(direction, startX, startZ, rotationNode.Directions[0].Connections));

        int i = 0;
        while (true)
        {
            if (directions.Count == 0 || i > 200)
                break;

            i++;

            var dir = directions[0];

            var connection = getRandomConnection(dir, rand);
            var tile = findTile(connection.TileName);
            var rotation = getRandomRotation(connection, rand);
            color = findColor(tile, rotation);
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

            if (mapcolor[dir.Y, dir.X].R == 255.0f && mapcolor[dir.Y, dir.X].G == 255.0f && mapcolor[dir.Y, dir.X].B == 255.0f && mapcolor[dir.Y, dir.X].A == 255.0f)
            {
                mapcolor[dir.Y, dir.X] = color;
                if (tile.TileName.ToLower() == "end".ToLower())
                    ends.Add(new EndPosition() { X = dir.X, Y = dir.Y, Rotation = rN.Rotation });
                foreach (var item in rN.Directions)
                    directions.Add(new TileDirectionNode(item.Direction, dir.X, dir.Y, item.Connections));
            }
            directions.Remove(dir);
        }

        foreach (var dir in directions)
        {
            var connection = dir.Connections.Find(t => t.TileName.ToLower() == "end");
            var tile = findTile(connection.TileName);
            var rotation = getRandomRotation(connection, rand);
            color = findColor(tile, rotation);
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

            if (mapcolor[dir.Y, dir.X].R == 255.0f && mapcolor[dir.Y, dir.X].G == 255.0f && mapcolor[dir.Y, dir.X].B == 255.0f && mapcolor[dir.Y, dir.X].A == 255.0f)
            {
                mapcolor[dir.Y, dir.X] = color;
                ends.Add(new EndPosition() { X = dir.X, Y = dir.Y, Rotation = rN.Rotation });
            }
        }

        var end = getRandomEnd(ends, rand);
        var finishTile = findTile("finish");
        color = findColor(finishTile, end.Rotation);
        mapcolor[end.Y, end.X] = color;
    }

    /**---------------------------------------------------------------------------------
    *   Gets one random position from the list of positions.
    */
    private EndPosition
    getRandomEnd(List<EndPosition> ends, System.Random rand)
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

    /**---------------------------------------------------------------------------------
    *   Gets a random rotation of the specified tile.
    */
    private float
    getRandomRotation(ConnectionNode connection, System.Random rand)
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

    /**---------------------------------------------------------------------------------
    *   Gets a random tile from the list of possible tiles.
    */
    private ConnectionNode
    getRandomConnection(TileDirectionNode dir, System.Random rand)
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

    /**---------------------------------------------------------------------------------
    *   Write the map to disk as a png
    */
    public void
    Save(string filename)
    {
        var rawMap = map.EncodeToPNG();
        FileStream stream = new FileStream(filename, FileMode.OpenOrCreate);
        stream.Write(rawMap, 0, rawMap.Length);
        stream.Flush();
        stream.Close();
    }

    /**---------------------------------------------------------------------------------
    *   Load XML file with tile data.
    */
    private TileContainer
    getTileTypes()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TileNodes");
        StringReader stringStream = new StringReader(textAsset.text);

        XmlSerializer serializer = new XmlSerializer(typeof(TileContainer));
        var tiles = serializer.Deserialize(stringStream) as TileContainer;
        stringStream.Close();

        return tiles;
    }

    /**---------------------------------------------------------------------------------
    *   Load 2D texture from disk
    */
    private Texture2D
    loadTexture2D(string filename)
    {
        FileStream stream = new FileStream(filename, FileMode.Open);
        byte[] buffer = new byte[width * depth * 4];
        stream.Read(buffer, 0, buffer.Length);
        Texture2D texture = new Texture2D(width, depth, TextureFormat.RGBA32, false);
        texture.LoadImage(buffer);
        stream.Close();

        return texture;
    }

    /**---------------------------------------------------------------------------------
    *   Load map from memory.
    */
    public void
    loadFromMemory()
    {
        grid = new Tile[width, depth];

        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color color = map.GetPixel(x, y);

                RotationNode rotationN = null;
                TileNode tileN = null;

                foreach (var tileNode in tileTypes.Tiles)
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

    /**---------------------------------------------------------------------------------
    *   Load map from disk.
    */
    public void
    Load(string filename)
    {
        map = loadTexture2D(filename);
        //depth = map.height;
        //width = map.width;

        grid = new Tile[width, depth];

        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color color = map.GetPixel(x, y);

                RotationNode rotationN = null;
                TileNode tileN = null;

                foreach (var tileNode in tileTypes.Tiles)
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
