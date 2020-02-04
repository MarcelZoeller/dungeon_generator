using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{

    [SerializeField] Sprite open;
    [SerializeField] Sprite key;
    [SerializeField] Sprite bossKey;
    [SerializeField] Sprite keyItem;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSprite(string sprite)
    {
        switch (sprite)
        {
            case "open":
                GetComponent<SpriteRenderer>().sprite = open;
                break;
            case "key":
                GetComponent<SpriteRenderer>().sprite = key;
                break;
            case "bossKey":
                GetComponent<SpriteRenderer>().sprite = bossKey;
                break;
            case "keyItem":
                GetComponent<SpriteRenderer>().sprite = keyItem;
                break;
        }
    }
}
