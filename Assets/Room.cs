using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject parent;
    public List<GameObject> children;
    public List<int> freeSpaces;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int ReturnFreeSpacesCount()
    {
        return freeSpaces.Count;
    }
}
