using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Room : MonoBehaviour
{
   
    

    public List<int> freeSpaces;
    public List<string> freeEdges;
    public GameObject idTextChild;
    public GameObject keylevelTextChild;
    TextMeshPro text;

    public GameObject parent;
    public List<GameObject> children;
    

    public int keyLevel;
    public int RoomId;



    // Start is called before the first frame update
    void Start()
    {

        //idTextChild = gameObject.transform.Find("IdText").gameObject;
        //keylevelTextChild = gameObject.transform.Find("keylevelText").gameObject;




    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int ReturnFreeSpacesCount()
    {
        return freeSpaces.Count;
    }

    public void UpdateMyRoomID(int id)
    {
        text = idTextChild.gameObject.GetComponent<TextMeshPro>() as TextMeshPro;
        text.text = id.ToString();

    }

    public void UpdateMyKeyLevel(int keylevel)
    {

        text = keylevelTextChild.gameObject.GetComponent<TextMeshPro>() as TextMeshPro;
        text.text = keylevel.ToString();

    }


    public void WhoIsMyParent(GameObject mypartent)
    {

        parent = mypartent;

    }

    public void AddMyChild(GameObject mychild)
    {
        children.Add(mychild);


    }

    public bool IsMyChild(GameObject childToCheck)
    {
        for (int i = 0; children.Count > 0; i++)
        {
            if (children[i]  == childToCheck)
            {
                return true;
                
            }
        }

        return true;
    }
}

