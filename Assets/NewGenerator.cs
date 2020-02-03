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
        ExpandMainBranch();
        PlaceBossRoom();

        //Add Bonus Rooms (locked with Keys, with a Key)


    }
    private void PlaceStartRoom()
    {
        var roomID = Instantiate(roomObject, transform.position, transform.rotation);
        roomID.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 1f);
        roomID.GetComponent<Room>().desciption = "Start";
        allRoomList.Add(roomID.GetComponent<Room>());
    }
    private void PlaceMiniBossRoom()
    {
        //pick last main branch room
        Room lastMainBranchRoom = allRoomList[allRoomList.Count - 1];
        int counter = 0;
        while (lastMainBranchRoom.mainBranch == false)
        {
            
            lastMainBranchRoom = allRoomList[allRoomList.Count -1- counter];
            counter++;
        }

        
        if (nextRoomKeyLocked == true)
        {
            room = CreateNextRoom(lastMainBranchRoom, "key");
            nextRoomKeyLocked = false;
        }
        else
        {
            room = CreateNextRoom(lastMainBranchRoom, "noone");
        }


        room.desciption = "Mini Boss";
        room.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.5f, 0.5f);

    }
    private void PlaceKeyItemRoom()
    {
        //pick last main branch room
        Room lastMainBranchRoom = allRoomList[allRoomList.Count - 1];
        int counter = 0;
        while (lastMainBranchRoom.mainBranch == false)
        {

            lastMainBranchRoom = allRoomList[allRoomList.Count - 1 - counter];
            counter++;
        }

        if (nextRoomKeyLocked == true)
        {
            room = CreateNextRoom(lastMainBranchRoom, "key");
            nextRoomKeyLocked = false;
        }
        else
        {
            room = CreateNextRoom(lastMainBranchRoom, "noone");
        }
        room.desciption = "Key Item";
        room.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.9f, 0.3f);
        hasKeyItem = true;

    }
    private void PlaceBossRoom()
    {
        // Boss Key or Boss Room First
        //randomasd
        
            
        if (UnityEngine.Random.Range(0, 100) >= 50)
        {
            bossRoomBeforeKey = true;
        }
        
        
        if (bossRoomBeforeKey == true)
        {
            //pick last main branch room
            Room lastMainBranchRoom = allRoomList[allRoomList.Count - 1];
            int counter = 0;
            while (lastMainBranchRoom.mainBranch == false)
            {

                lastMainBranchRoom = allRoomList[allRoomList.Count - 1 - counter];
                counter++;
            }

            //Create BossKey Room   
            room = CreateNextRoom(lastMainBranchRoom, "BossKey");
            room.desciption = "Boss";
            room.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.1f, 0.1f);

            FindRandomRoom();


            //Create Boss Room
            if (nextRoomKeyLocked == true)
            {
                room = CreateNextRoom(allRoomList[randomRoom], "key");
                nextRoomKeyLocked = false;
            }
            else
            {
                room = CreateNextRoom(allRoomList[randomRoom], "noone");
            }
            room.desciption = "Boss Key";
            room.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.3f, 0.8f);
        }
        else
        {
            //pick last main branch room
            Room lastMainBranchRoom = allRoomList[allRoomList.Count - 1];
            int counter = 0;
            while (lastMainBranchRoom.mainBranch == false)
            {

                lastMainBranchRoom = allRoomList[allRoomList.Count - 1 - counter];
                counter++;
            }

            //Create BossKey Room
            if (nextRoomKeyLocked == true)
            {
                room = CreateNextRoom(lastMainBranchRoom, "key");
                nextRoomKeyLocked = false;
            }
            else
            {
                room = CreateNextRoom(lastMainBranchRoom, "noone");
            }
            room.desciption = "Boss Key";
            room.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.3f, 0.8f);

            FindRandomRoom();


            //Create Boss Room
            room = CreateNextRoom(allRoomList[randomRoom], "BossKey");
            room.desciption = "Boss";
            room.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.1f, 0.1f);
        }

        

    }
    private void ExpandMainBranch()
    {
        //Room Random Count
        int spawnRoomCount = UnityEngine.Random.Range(1, 2);

        print("Count:" + spawnRoomCount);

        //Create Rooms
        for (int i = 1; i <= spawnRoomCount; i++)
        {
            


            if (nextRoomKeyLocked == true)
            {
                room = CreateNextRoom(allRoomList[allRoomList.Count - 1], "key");
                nextRoomKeyLocked = false;
            }
            else
            {
                room = CreateNextRoom(allRoomList[allRoomList.Count - 1], "noone");
            }



            room.mainBranch = true;
        }

        //Create Locked Room
        roomLevel++;
        //room = CreateNextRoom(allRoomList[allRoomList.Count - 1],"key");
        //room.mainBranch = true;
        nextRoomKeyLocked = true;



        // PLACE KEY
        // Pick Random Room with Free Edges
        roomLevel--;
        FindRandomRoom();

        // Spawn Rooms
        spawnRoomCount = UnityEngine.Random.Range(1, 3);
        for (int i = 1; i <= spawnRoomCount; i++)
        {
            if (i == 1) //First Room Pick Random Room
            {
                if (hasKeyItem == true)
                {
                    CreateNextRoom(allRoomList[randomRoom], "KeyItem");
                }
                else
                {
                    CreateNextRoom(allRoomList[randomRoom], "none");
                }

            }
            else
            {
                CreateNextRoom(allRoomList[allRoomList.Count - 1], "noone");
            }

            if (i == spawnRoomCount) //Place Key in Last Room
            {
                allRoomList[allRoomList.Count - 1].GetComponent<Room>().desciption = "Key";
                allRoomList[allRoomList.Count - 1].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0.3f);
            }

        }
        roomLevel++;

        //- Spawn Rooms -
        spawnRoomCount = UnityEngine.Random.Range(0, 2);
        for (int i = 1; i <= spawnRoomCount; i++)
        {
            Room lastMainBranchRoom = allRoomList[allRoomList.Count - 1];
            int counter = 0;
            while (lastMainBranchRoom.mainBranch == false)
            {

                lastMainBranchRoom = allRoomList[allRoomList.Count - 1 - counter];
                counter++;
            }

            if (nextRoomKeyLocked == true)
            {
                room = CreateNextRoom(lastMainBranchRoom, "key");
                nextRoomKeyLocked = false;
            }
            else
            {
                room = CreateNextRoom(lastMainBranchRoom, "noone");
            }

            room.mainBranch = true;
        }




    }

    private void FindRandomRoom()
    {
        bool foundRoom = false;
        while (foundRoom == false)
        {
            //Pick Random Room
            randomRoom = UnityEngine.Random.Range(1, allRoomList.Count - 1);
            //Move to Room Position
            transform.position = allRoomList[randomRoom].gameObject.transform.position;

            //allRoomList[randomRoom].GetComponent<SpriteRenderer>().color = new Color(0.3f, 1f, 0.3f);

            if (CheckFreeDoors(allRoomList[randomRoom]) > 0)
            {
                foundRoom = true;
            }
        }
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
        var pickedDir = UnityEngine.Random.Range(0, parentRoom.freeEdges.Count);
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
            doorID.GetComponent<SpriteRenderer>().color = new Color(1f, 0.3f, 0.3f);
        }
        if(locked == "KeyItem")
        {
            doorID.GetComponent<SpriteRenderer>().color = new Color(0.3f, 1f, 0.3f);
        }
        if (locked == "BossKey")
        {
            doorID.GetComponent<SpriteRenderer>().color = new Color(1f, 0.3f, 1f);
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