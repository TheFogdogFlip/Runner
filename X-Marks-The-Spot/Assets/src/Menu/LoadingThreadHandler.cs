using UnityEngine;
using System.Collections;
using System.Threading;

public class LoadingThreadHandler : Object {

    private Thread loadingThread;
    private string loadingText;
    private bool loaded = false;
    private bool generated = false;

    public string LoadingText
    {
        get { return loadingText; }
    }

    public bool Loaded
    {
        get { return loaded; }
    }

    public bool Generated
    {
        get { return generated; }
    }

    public void GenerateWorld()
    {
        loadingThread = new Thread(new ThreadStart(generateWorld));
        loadingThread.Start();
    }

    private void generateWorld()
    {
        loadingText = "Generating World...";
        World.Instance.Generate();

        generated = true;
    }

    public void LoadWorld(string path)
    {
        if (!generated)
            loadingThread.Abort();

        worldPath = path;
        loadingThread = new Thread(new ThreadStart(loadWorld));
        loadingThread.Start();
    }

    private string worldPath;

    private void loadWorld()
    {
        loadingText = "Loading World...";
        World.Instance.Load(worldPath);
    }

    public void LoadAssets(string path)
    {
        if (!generated || !loaded)
            loadingThread.Join();

        loadingThread = new Thread(new ThreadStart(loadAssets));
        loadingThread.Start();
    }

    private void loadAssets()
    {
        loadingText = "Loading Player...";
        Instantiate(Resources.Load("Player", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection));

        loadingText = "Loading Camera...";
        Instantiate(Resources.Load("PlayerCamera", typeof(GameObject)), World.Instance.StartPosition, Quaternion.Euler(World.Instance.StartDirection));
    }

    public void UseGenerated()
    {
        World.Instance.UseGenerated();
    }
}
