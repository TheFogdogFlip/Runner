using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LOAD_LEVEL_SCRIPT : MonoBehaviour 
{

    public Button backText;

    // Use this for initialization
    void Start()
    {
        backText = backText.GetComponent<Button>();
    }

    public void BackPress()
    {
        Application.LoadLevel(0);
    }
}
