using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGenerator : MonoBehaviour
{

    [SerializeField] GameObject roomObject;
    [SerializeField] GameObject doorObject;

    private List<Room> allRoomList = new List<Room>();
    public LayerMask whatIsRoom;

    int roomLevel = 0;
    Room room;
    int randomRoom;
    bool hasKeyItem = false;
    bool nextRoomKeyLocked = false;
    bool bossRoomBeforeKey = false;

    Room checkThisRoom;

    //Switches
    /*
     * One OnOff Switch, with Multiplay Trigger and 
     * 
     * One Big Lock with Multiple Triggers 
     * 
     * 
     */


    // Start is called before the first frame update
    void Start()
    {
        //Place Start Room
        PlaceStartRoom();
        ExpandMainBranch();
        PlaceMiniBossRoom();
        ExpandMainBranch();
        PlaceKeyItemRoom();
        ExpandMainBranch();
        ExpandMainBranch();
        PlaceBossRoom();
        //Add Bonus Rooms (locked with Keys, with a Key)


    }
    private void PlaceStartRoom()
    {
        room = Instantiate(roomObject, transform.position, transform.rotation).GetComponent<Room>();
        room.GetComponent<Room>().UpdateIcon("entrance");
        room.mainBranch = true;
        allRoomList.Add(room.GetComponent<Room>());
    }
    private void PlaceMiniBossRoom()
    {
        room = CreateNextMainRoom();
        room.GetComponent<Room>().UpdateIcon("miniBoss");
        room.mainBranch = true;
    }
    private void PlaceKeyItemRoom()
    {
        room = CreateNextMainRoom();
        room.GetComponent<Room>().UpdateSymbol("keyItem");
        //roomLevel++;
        hasKeyItem = true;
        //int keyItemKeyLevel = keyItemKeyLevel; 
    }
    private void PlaceBossRoom()
    {
        // Randomly decide if place Boss Key or Boss Room first
        if (UnityEngine.Random.Range(0, 100) >= 50)
        {
            //Create BossKey Room   
            room = CreateNextRoom(LastMainBranchRoom(), "BossKey"); //Lock with BossKey 
            room.GetComponent<Room>().UpdateIcon("boss");

            roomLevel++;


            //Create BossKey Room
            if (nextRoomKeyLocked == true)
            {
                room = CreateNextRoom(LastMainBranchRoom(), "key");
                nextRoomKeyLocked = false;
            }
            else
            {
                room = CreateNextRoom(allRoomList[randomRoom], "KeyItem");
            }

            room.GetComponent<Room>().UpdateSymbol("bossKey");

        }
        else
        {   //Create BossKey Room befor Boss            
            room = CreateNextMainRoom();
            room.GetComponent<Room>().UpdateSymbol("bossKey");

            roomLevel++;

            //Create Boss Room
            room = CreateNextRoom(allRoomList[randomRoom], "BossKey"); // Lock with BossKey
            room.GetComponent<Room>().UpdateIcon("boss");
        }
        
        


        

    }
    private void ExpandMainBranch()
    {

        // GENERATE ROOMS BEFORE LOCKED ROOM
        int spawnRoomCount = UnityEngine.Random.Range(1, 2+1);  //Set how many rooms to Spawn
        for (int i = 0; i < spawnRoomCount; i++)  //Repeat for every Room
        {
            CreateNextMainRoom();
        }

        nextRoomKeyLocked = true;
        



        // PLACE (Hide) THE KEY 
        spawnRoomCount = UnityEngine.Random.Range(1, 2+1); //Set Room Amount

        Room rndRoom = RandomRoom();
        for (int i = 0; i < spawnRoomCount; i++)
        {
            
            if (i == 0) //First Room Pick Random Room
            {
                if (hasKeyItem == true)
                {
                    room = CreateNextRoom(rndRoom, "KeyItem");  //Lock first Room with Key Item
                }
                else
                {
                    room = CreateNextRoom(rndRoom, "none");  //Create an empty Room
                    room.keyLevel = rndRoom.keyLevel;
                }
            }
            else //All other Rooms
            {
                room = CreateNextRoom(allRoomList[allRoomList.Count - 1], "noone");
                room.keyLevel = rndRoom.keyLevel;

            }
            if (i == spawnRoomCount-1) //Place the Key in Last Room
            {
               allRoomList[allRoomList.Count - 1].GetComponent<Room>().UpdateSymbol("key");
            }

            

        }



        //INCREASE ROOM LEVEL
        roomLevel++;



        // GENERATE ROOMS BEFORE LOCKED ROOM
        spawnRoomCount = UnityEngine.Random.Range(0, 1 + 1);  //Set how many rooms to Spawn
        for (int i = 0; i < spawnRoomCount; i++)  //Repeat for every Room
        {
            CreateNextMainRoom();
        }

        



    }
    private Room CreateNextMainRoom()
    {
        Room NextMainRoom;
        if (nextRoomKeyLocked == true)
        {
            NextMainRoom = CreateNextRoom(LastMainBranchRoom(), "key");
            nextRoomKeyLocked = false;
        }
        else
        {
            NextMainRoom = CreateNextRoom(LastMainBranchRoom(), "noone");
        }
        NextMainRoom.mainBranch = true;
        return NextMainRoom;
    }


    private Room LastMainBranchRoom() //Pick the Last Room from the Main Branch Room List, with free Edges
    {
        int counter = 0;
        do
        {
            checkThisRoom = allRoomList[allRoomList.Count - 1 - counter];
            CheckFreeDoors(checkThisRoom);
            counter++;
        } while ((checkThisRoom.mainBranch == false));  //check if room is main and has free spaces
        return checkThisRoom;
    }
    private Room RandomRoom()  //Pick a random Room with Free Edges
    {
        bool foundRoom = false;
        while (foundRoom == false)
        {
            //Pick Random Room incl Start Room 
            randomRoom = UnityEngine.Random.Range(1, allRoomList.Count - 1);
            //Move Spawner to Room Position
            transform.position = allRoomList[randomRoom].gameObject.transform.position;

            //allRoomList[randomRoom].GetComponent<SpriteRenderer>().color = new Color(0.3f, 1f, 0.3f);

            if (CheckFreeDoors(allRoomList[randomRoom]) > 0)
            {
                foundRoom = true;
            }

            
        }
        return allRoomList[randomRoom];
    }
    private Room LastRoomCreated() //Give Back Last Room was Created
    {
        return allRoomList[allRoomList.Count - 1];
    }

    // locked: noone, key, keyItem, monster, switch, bossKey, Bombs
    private Room CreateNextRoom(Room parentRoom, string locked)
    {
        // select latest Room
        //Room latestRoom = allRoomList[allRoomList.Count - 1];

        //Set Generator Pos to latest Room
        transform.position = parentRoom.gameObject.transform.position;

        // Check which Doors are free
        CheckFreeDoors(parentRoom);

        //move spawner to doorPos
        int pickedDir;
        if (allRoomList.Count == 1)// First dir is always north
        {
            pickedDir = 0;
        }
        else
        {
            pickedDir = UnityEngine.Random.Range(0, parentRoom.freeEdges.Count);
        }

        // if there are no free Edges Reload
        if (parentRoom.freeEdges.Count == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        switch (parentRoom.freeEdges[pickedDir])
        {
            case "up":
                transform.position += new Vector3(0f, 0.5f, 0f);
                break;

            case "down":
                transform.position += new Vector3(0f, -0.5f, 0f);
                break;

            case "left":
                transform.position += new Vector3(-0.5f, 0f, 0f);
                break;

            case "right":
                transform.position += new Vector3(0.5f, 0f, 0f);
                break;
        }

        //Create Door
        var doorID = Instantiate(doorObject, transform.position, transform.rotation);
        if (locked == "key")
        {
            //doorID.GetComponent<SpriteRenderer>().color = new Color(1f, 0.3f, 0.3f);
            doorID.GetComponent<door>().UpdateSprite("key");
        }
        if(locked == "KeyItem")
        {
            //doorID.GetComponent<SpriteRenderer>().color = new Color(0.3f, 1f, 0.3f);
            doorID.GetComponent<door>().UpdateSprite("keyItem");
        }
        if (locked == "BossKey")
        {
            //doorID.GetComponent<SpriteRenderer>().color = new Color(1f, 0.3f, 1f);
            doorID.GetComponent<door>().UpdateSprite("bossKey");
        }



        //move spawner to destination
        switch (parentRoom.freeEdges[pickedDir])
        {
            case "up":
                transform.position += new Vector3(0f, 0.5f, 0f);
                break;

            case "down":
                transform.position += new Vector3(0f, -0.5f, 0f);
                break;

            case "left":
                transform.position += new Vector3(-0.5f, 0f, 0f);
                break;

            case "right":
                transform.position += new Vector3(0.5f, 0f, 0f);
                break;
        }



        //Create Room Instance
        var roomID = Instantiate(roomObject, transform.position, transform.rotation);
        allRoomList.Add(roomID.GetComponent<Room>());
        roomID.GetComponent<Room>().keyLevel = roomLevel;

        return roomID.GetComponent<Room>();
    }

    private int CheckFreeDoors(Room checkThisRoom)
    {
        checkThisRoom.freeEdges.Clear();
        Vector3 startPos = transform.position;

        //check up
        transform.position += new Vector3(0f, 1f, 0f);
        if (Physics2D.OverlapCircle(transform.position, .1f, whatIsRoom) == false)
        {
            checkThisRoom.freeEdges.Add("up");
        }
        transform.position = startPos;

        //check down
        transform.position += new Vector3(0f, -1f, 0f);
        if (Physics2D.OverlapCircle(transform.position, .1f, whatIsRoom) == false)
        {
            checkThisRoom.freeEdges.Add("down");
        }
        transform.position = startPos;

        //check right
        transform.position += new Vector3(1f, 0f, 0f);
        if (Physics2D.OverlapCircle(transform.position, .1f, whatIsRoom) == false)
        {
            checkThisRoom.freeEdges.Add("right");
        }
        transform.position = startPos;

        //check left
        transform.position += new Vector3(-1f, 0f, 0f);
        if (Physics2D.OverlapCircle(transform.position, .1f, whatIsRoom) == false)
        {
            checkThisRoom.freeEdges.Add("left");
        }
        transform.position = startPos;
       
        return checkThisRoom.freeEdges.Count;

        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }



}



//change room Color
//allRoomList[0].gameObject.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 1f);