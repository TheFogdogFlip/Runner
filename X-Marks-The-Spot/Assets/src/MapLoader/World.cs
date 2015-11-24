using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

public struct TileDirectionNode
{
    public DirectionNode Direction;
    public int X;
    public int Y;
    public int Rotation;
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

    private void generate()
    {
        var tileTypes = getTileTypes();

        width = 64;
        depth = 64;

        Texture2D generatedMap = new Texture2D(width, depth, TextureFormat.RGBA32, false);

        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                generatedMap.SetPixel(y, x, new Color(1, 1, 1, 1));
            }
        }

        System.Random rand = new System.Random();

        Vector3 startTilePos = new Vector3(rand.Next(width), rand.Next(depth), 0);

        var startTile = tileTypes.Tiles.Find(n => n.TileName.ToLower() == "start");

        int direction = 0;//rand.Next(0, 3);

        float angle = 0.0f;

        switch (direction)
        {
            case 0:
                startDirection = new Vector3(0, 0, 1);
                angle = 0.0f;
                break;
            case 1:
                startDirection = new Vector3(1, 0, 0);
                angle = 90.0f;
                break;
            case 2:
                startDirection = new Vector3(0, 0, -1);
                angle = 180.0f;
                break;
            case 3:
                startDirection = new Vector3(-1, 0, 0);
                angle = 270.0f;
                break;
        }
        startDirection = new Vector3(0, 0, angle);
        generatedMap.SetPixel((int)startTilePos.x, (int)startTilePos.y, startTile.Rotations.Find(c => c.Rotation == angle).Color.ToColor());

        List<TileDirectionNode> directions = new List<TileDirectionNode>();

        int currentRotation = (int)angle;
        TileNode currentTile = startTile;
        int currentX = (int)startTilePos.x;
        int currentY = (int)startTilePos.y;
        int currentDirection = -1;

        int dirX;
        int dirZ;
        foreach (var item in currentTile.Directions)
        {
            if (item.Direction != currentDirection)
            {
                switch (currentRotation)
                {
                    case 0:
                        break;
                    case 90:
                        if (item.Direction == 0)
                            item.Direction = 3;
                        else
                            item.Direction -= 1;
                        currentRotation = 0;
                        break;
                    case 180:
                        switch(item.Direction)
                        {
                            case 0:
                                item.Direction = 2;
                                currentRotation = 90;
                                break;
                            case 1:
                                item.Direction = 3;
                                currentRotation = 90;
                                break;
                            case 2:
                                item.Direction = 0;
                                currentRotation = 90;
                                break;
                            case 3:
                                item.Direction = 1;
                                currentRotation = 90;
                                break;
                        }
                        break;
                    case 270:
                        switch (item.Direction)
                        {
                            case 0:
                                item.Direction = 1;
                                currentRotation = 180;
                                break;
                            case 1:
                                item.Direction = 2;
                                currentRotation = 180;
                                break;
                            case 2:
                                item.Direction = 3;
                                currentRotation = 180;
                                break;
                            case 3:
                                item.Direction = 0;
                                currentRotation = 180;
                                break;
                        }
                        break;
                }

                directions.Add(new TileDirectionNode { Direction = item, X = currentX, Y = currentY , Rotation = currentRotation});
            }
        }

        int i = 0;
        while (i < 50)
        {
            if (directions.Count == 0)
                break;
            i++;
            var dir = directions[0];

            currentX = dir.X;
            currentY = dir.Y;

            dirX = 0;
            dirZ = 0;

            switch (dir.Direction.Direction)
            {
                case 0:
                    dirZ = 1;
                    break;
                case 1:
                    dirX = 1;
                    break;
                case 2:
                    dirZ = -1;
                    break;
                case 3:
                    dirX = -1;
                    break;
            }

            currentX += dirX;
            currentY += dirZ;
            var oldTile = currentTile;
            var connection = getRandomConnection(dir, rand);
            currentTile = tileTypes.Tiles.Find(n => n.TileName == connection.TileName);

            if (currentX > width || currentY > depth || currentX < 0 || currentY < 0)
            {

            }
            else
            {
                Color mapColor = generatedMap.GetPixel(currentX, currentY);
                int delDir = -1;
                if (mapColor == new Color(1, 1, 1, 1))
                {
                    if(oldTile != null)
                        if(oldTile.TileName.ToLower() == "corner")
                            switch(currentRotation)
                            {
                                case 0:
                                    currentRotation = 180;
                                    break;
                                case 90:
                                    currentRotation = 270;
                                    break;
                                case 180:
                                    currentRotation = 0;
                                    break;
                                case 270:
                                    currentRotation = 90;
                                    break;
                            }
                    float randomRotation = getRandomRotation(connection, rand, currentRotation);
                    RotationNode tile = currentTile.Rotations.Find(r => r.Rotation == randomRotation);

                    if (tile == null)
                    {
                        switch ((int)randomRotation)
                        {
                            case 0:
                                randomRotation = 180;
                                break;
                            case 90:
                                randomRotation = 270;
                                break;
                            case 180:
                                randomRotation = 0;
                                break;
                            case 270:
                                randomRotation = 90;
                                break;
                        }
                        tile = currentTile.Rotations.Find(r => r.Rotation == randomRotation);
                    }

                    generatedMap.SetPixel(currentX, currentY, tile.Color.ToColor());
                    foreach (var item in currentTile.Directions)
                    {
                        if (currentTile.TileName.ToLower() == "corner")
                        {

                        }
                        else
                        {
                            switch (dir.Direction.Direction)
                            {
                                case 0:
                                    delDir = 2;
                                    break;
                                case 1:
                                    delDir = 3;
                                    break;
                                case 2:
                                    delDir = 0;
                                    break;
                                case 3:
                                    delDir = 1;
                                    break;
                            }
                        }

                        if (item.Direction != delDir)
                        {
                            if(currentTile.TileName.ToLower() == "corner")
                            {
                                switch (item.Direction)
                                {
                                    case 0:
                                        item.Direction = 2;
                                        break;
                                    case 1:
                                        item.Direction = 3;
                                        break;
                                    case 2:
                                        item.Direction = 0;
                                        break;
                                    case 3:
                                        item.Direction = 1;
                                        break;
                                }
                                directions.Add(new TileDirectionNode { Direction = item, X = currentX, Y = currentY });
                            }
                            else
                                directions.Add(new TileDirectionNode { Direction = item, X = currentX, Y = currentY });
                        }
                    }
                }
            }
            directions.Remove(dir);
        }
        save("Temp.png", generatedMap);
        load("Temp.png");
    }

    private float getRandomRotation(ConnectionNode connection, System.Random rand, int currentRotation)
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
        rotation.Rotation -= currentRotation;

        switch((int)rotation.Rotation)
        {
            case -90:
                rotation.Rotation = 270;
                break;
            case -180:
                rotation.Rotation = 180;
                break;
            case -270:
                rotation.Rotation = 90;
                break;
        }

        return rotation.Rotation;
    }

    private ConnectionNode getRandomConnection(TileDirectionNode dir, System.Random rand)
    {

        int randomNumber = rand.Next(0, 100);
        ConnectionNode connection = null;

        float chance = 0.0f;

        for (int i = 0; i < dir.Direction.Connections.Count; i++)
        {
            connection = dir.Direction.Connections[i];
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
