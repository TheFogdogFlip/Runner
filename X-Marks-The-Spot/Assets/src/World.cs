using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

public class World {

    private Vector3 gridDimentions = new Vector3(512, 1024, 512);
    private Tile[,] grid;

    private int width;
    private int depth;

    public static World Instance;

    private World()
    {

    }

    public void Init(string filename)
    {
        Instance = new World();
        Instance.load(filename);
    }

    public void Init()
    {
        Instance = new World();
        Instance.generate();
    }

    public Tile GetTile(int x, int z)
    {
        return grid[Mathf.FloorToInt(x / gridDimentions.x), Mathf.FloorToInt(z / gridDimentions.z)];
    }

    public void SetTile(int x, int z, Tile tile)
    {
        grid[Mathf.FloorToInt(x / gridDimentions.x), Mathf.FloorToInt(z / gridDimentions.z)] = tile;
       
    }

    private void generate()
    {

    }

    public void Save(string filename)
    {

    }

    private void load(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(TileNode));
        FileStream stream = new FileStream("TileNodes.xml", FileMode.Open);

        var tiles = serializer.Deserialize(stream) as TileContainer;

        var texture = Resources.Load<Texture2D>(filename);

        int height = texture.height;
        int width = texture.width;

        this.grid = new Tile[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var color = texture.GetPixel(x, y);

                var tileType = tiles.Nodes.Find(n => n.Color == color);

                Tile tile = null;

                switch (tileType.Name)
                {
                    case ModelTypes.Wall:
                        tile = new WallTile(new Vector3(x * 512, 0, y * 512));
                        tile.Object.SetActive(true);
                        break;

                    case ModelTypes.Pit:
                        tile = new PitTile(new Vector3(x * 512, -512, y * 512));
                        break;

                    case ModelTypes.Crouch:
                        tile = new CrouchTile(new Vector3(x * 512, 0, y * 512));
                        break;

                    case ModelTypes.LeftObstacle:
                        tile = new ObstacleTile(new Vector3(x * 512, 0, y * 512));
                        break;

                    case ModelTypes.RightObstacle:
                        tile = new ObstacleTile(new Vector3(x * 512 + 512 - 50, 0, y * 512));
                        break;
                }
                this.grid[x, y] = tile;
            }
        }
    }
}
