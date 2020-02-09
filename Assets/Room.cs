using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Room : MonoBehaviour
{
   
    

    public List<int> freeSpaces;
    public List<string> freeEdges;
    public GameObject textChild;
    
    TextMeshPro text;

    public GameObject parent;
    public List<GameObject> children;
    

    public int keyLevel;
    public int RoomId;
    public bool mainBranch = false;

    public string desciption = "none";

    [SerializeField] GameObject SymbolChild;

    [SerializeField] Sprite open;
    [SerializeField] Sprite key;
    [SerializeField] Sprite bossKey;
    [SerializeField] Sprite keyItem;


    [SerializeField] GameObject iconChild;

    [SerializeField] Sprite iconEntrance;
    [SerializeField] Sprite iconMiniBoss;
    [SerializeField] Sprite iconBoss;

    [SerializeField] GameObject mumbersChild;

    [SerializeField] List<Sprite> Mumbers;



    // Start is called before the first frame update
    void Start()
    {

        //idTextChild = gameObject.transform.Find("IdText").gameObject;
        //keylevelTextChild = gameObject.transform.Find("keylevelText").gameObject;

        //iconChild.GetComponent<SpriteRenderer>().sprite = null;


    }

    // Update is called once per frame
    void Update()
    {
        text = textChild.gameObject.GetComponent<TextMeshPro>() as TextMeshPro;

        text.text = desciption;// + "\nRLvl: "+ keyLevel;

        mumbersChild.GetComponent<SpriteRenderer>().sprite = Mumbers[keyLevel];
        //GetComponent<SpriteRenderer>().sprite = mumb
    }

    public int ReturnFreeSpacesCount()
    {
        return freeSpaces.Count;
    }

    public void UpdateMyRoomID(int id)
    {
        text = textChild.gameObject.GetComponent<TextMeshPro>() as TextMeshPro;
        text.text = id.ToString();

    }

    public void UpdateMyKeyLevel(int keylevel)
    {

        text = textChild.gameObject.GetComponent<TextMeshPro>() as TextMeshPro;
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


    public void UpdateSymbol(string symbol)
    {
        switch (symbol)
        {
            case "open":
                SymbolChild.GetComponent<SpriteRenderer>().sprite = open;
                break;
            case "key":
                SymbolChild.GetComponent<SpriteRenderer>().sprite = key;
                break;
            case "bossKey":
                SymbolChild.GetComponent<SpriteRenderer>().sprite = bossKey;
                break;
            case "keyItem":
                SymbolChild.GetComponent<SpriteRenderer>().sprite = keyItem;
                break;
        }
    }

    public void UpdateIcon(string icon)
    {
        switch (icon)
        {
            case "entrance":
                iconChild.GetComponent<SpriteRenderer>().sprite = iconEntrance;
                break;
            case "miniBoss":
                iconChild.GetComponent<SpriteRenderer>().sprite = iconMiniBoss;
                break;
            case "boss":
                iconChild.GetComponent<SpriteRenderer>().sprite = iconBoss;
                break;
 
        }
    }



}

