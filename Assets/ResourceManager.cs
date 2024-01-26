using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    [SerializedDictionary("Plant", "Map Link")]
    public SerializedDictionary<string, string> mapLinks;

    public string currentMapLink;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMapLink(string plantName)
    {
        if (mapLinks.ContainsKey(plantName))
        {
            currentMapLink = mapLinks[plantName];
        }
    }
}
