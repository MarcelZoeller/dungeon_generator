using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Generator : MonoBehaviour
{
    [SerializeField] GameObject roomObject;
    [SerializeField] GameObject doorObject;

    [Header("Colors")]
    public Color startColor;
    public Color endColor;

    //public int distanceToEnd;

    public int roomAmount = 10;

    public Transform roomSpawner;
    //public Transform doorSpawner;

    public enum Direction { up, right, down, left };
    public Direction selectedDirection;

    public float xOffset = 1f, yOffset = 1f;

    public LayerMask whatIsRoom;

    private GameObject endRoom;
    private int roomCounter = 0;


    public List<GameObject> roomList = new List<GameObject>();
    public List<GameObject> roomWithNoChildren = new List<GameObject>();

    public int keyLevel;

    public GameObject startRoom;
    public GameObject bossRoom;
    public GameObject lastRoom;

    public List<GameObject> pathToStartList = new List<GameObject>();

    /* backtracking_change change you have to go back to previous key levels to open 
     * 
     * 
     * 
     */




    // Start is called before the first frame update
    void Start()
    {

        // 1. Generate start Room
        var roomID = Instantiate(roomObject, roomSpawner.position, roomSpawner.rotation);
        roomList.Add(roomID);
        roomID.gameObject.GetComponent<Room>().UpdateMyRoomID(roomList.Count);
        roomID.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 1f);

        startRoom = roomID;

        // 2.1. Pick Random Room


        //generate tree of rooms
        {

            keyLevel = 0;
            int keyLevelCounter = 5;

            while (roomList.Count != 30)
            {

                //pick random room
                int pickedRoomID = Random.Range(0, roomList.Count);
                GameObject pickedRoomObj = roomList[pickedRoomID];
                

                // check if room has empty spaces

                //reset list
                pickedRoomObj.GetComponent<Room>().freeEdges.Clear();
                

                //check right
                roomSpawner.position = pickedRoomObj.transform.position;
                roomSpawner.position += new Vector3(1f, 0f, 0f);
                if (Physics2D.OverlapCircle(roomSpawner.position, .1f, whatIsRoom) == false)
                {
                    pickedRoomObj.GetComponent<Room>().freeEdges.Add("right");
                }

                //check up 
                roomSpawner.position = pickedRoomObj.transform.position;
                roomSpawner.position += new Vector3(0f, 1f, 0f);
                if (Physics2D.OverlapCircle(roomSpawner.position, .1f, whatIsRoom) == false)
                {
                    pickedRoomObj.GetComponent<Room>().freeEdges.Add("up");
                }

                //check down
                roomSpawner.position = pickedRoomObj.transform.position;
                roomSpawner.position += new Vector3(0f, -1f, 0f);
                if (Physics2D.OverlapCircle(roomSpawner.position, .1f, whatIsRoom) == false)
                {
                    pickedRoomObj.GetComponent<Room>().freeEdges.Add("down");
                }

                //check left
                roomSpawner.position = pickedRoomObj.transform.position;
                roomSpawner.position += new Vector3(-1f, 0f, 0f);
                if (Physics2D.OverlapCircle(roomSpawner.position, .1f, whatIsRoom) == false)
                {
                    pickedRoomObj.GetComponent<Room>().freeEdges.Add("left");
                }

                //reset spawner
                roomSpawner.position = pickedRoomObj.transform.position;

                

                
                // check if picked room has empty spaces, if yes create room
                if (pickedRoomObj.GetComponent<Room>().freeEdges.Count > 0)
                {


                    //pick random direction
                    var pickedDir = Random.Range(0, pickedRoomObj.GetComponent<Room>().freeEdges.Count);

                    //move spawner to doorPos
                    switch (pickedRoomObj.GetComponent<Room>().freeEdges[pickedDir])
                    {
                        case "up":
                            roomSpawner.position += new Vector3(0f, 0.5f, 0f);
                            break;

                        case "down":
                            roomSpawner.position += new Vector3(0f, -0.5f, 0f);
                            break;

                        case "left":
                            roomSpawner.position += new Vector3(-0.5f, 0f, 0f);
                            break;

                        case "right":
                            roomSpawner.position += new Vector3(0.5f, 0f, 0f);
                            break;
                    }

                    //create door
                    Instantiate(doorObject, roomSpawner.position, roomSpawner.rotation);
                    //reset spawner
                    roomSpawner.position = pickedRoomObj.transform.position;


                    //move spawner to pos
                    switch (pickedRoomObj.GetComponent<Room>().freeEdges[pickedDir])
                    {
                        case "up":
                            roomSpawner.position += new Vector3(0f, 1f, 0f);
                            break;

                        case "down":
                            roomSpawner.position += new Vector3(0f, -1f, 0f);
                            break;

                        case "left":
                            roomSpawner.position += new Vector3(-1f, 0f, 0f);
                            break;

                        case "right":
                            roomSpawner.position += new Vector3(1f, 0f, 0f);
                            break;
                    }


                    if (keyLevelCounter <= 0)
                    {
                        keyLevel++;
                        keyLevelCounter = 5;
                    }
                    keyLevelCounter--;


                    //create room
                    roomID = Instantiate(roomObject, roomSpawner.position, roomSpawner.rotation);
                    roomID.gameObject.name = "Room " + roomList.Count.ToString();
                    pickedRoomObj.gameObject.GetComponent<Room>().AddMyChild(roomID);
                    roomList.Add(roomID);
                    roomID.gameObject.GetComponent<Room>().UpdateMyRoomID(roomList.Count);
                    roomID.gameObject.GetComponent<Room>().UpdateMyKeyLevel(keyLevel);
                    roomID.gameObject.GetComponent<Room>().WhoIsMyParent(pickedRoomObj);
                }


            }

        }




       
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].gameObject.GetComponent<Room>().children.Count == 0)
            {
                
                GameObject hisParent = roomList[i].gameObject.GetComponent<Room>().parent;
                print(hisParent.ToString() + "children" + hisParent.gameObject.GetComponent<Room>().children.Count.ToString());
                if (hisParent.gameObject.GetComponent<Room>().children.Count == 1)
                {
                    
                    roomWithNoChildren.Add(roomList[i].gameObject);

                }
            }
        }

        if (roomWithNoChildren.Contains(roomList[1]) == true)
        {
            roomWithNoChildren.Remove(roomWithNoChildren[1]);
        }


        int randomRoom = Random.Range(1, roomWithNoChildren.Count);
        GameObject randomRoomObj = roomWithNoChildren[randomRoom];

        lastRoom = randomRoomObj;
        bossRoom = randomRoomObj.GetComponent<Room>().parent.gameObject;

        randomRoomObj.GetComponent<SpriteRenderer>().color = new Color(1f, 0.1f, 0.1f);
        randomRoomObj.GetComponent<Room>().parent.gameObject.GetComponent<SpriteRenderer>().color = new Color(.7f, 0.1f, 0.1f);


        lastRoom.gameObject.GetComponent<Room>().keyLevel = keyLevel + 1;
        bossRoom.gameObject.GetComponent<Room>().keyLevel = keyLevel + 1;
        lastRoom.gameObject.GetComponent<Room>().UpdateMyKeyLevel(keyLevel+1);
        bossRoom.gameObject.GetComponent<Room>().UpdateMyKeyLevel(keyLevel + 1);


        

        GameObject roomChecker = bossRoom;
        GameObject parentToCheck;

        while(roomChecker != startRoom)
        {
            parentToCheck = roomChecker.gameObject.GetComponent<Room>().parent;
            pathToStartList.Add(parentToCheck);
            roomChecker = parentToCheck;
        }


        // Generate start Room
        //CreateRoom();


        //roomID.Add()
        /*
        for (int i = 0; i < roomAmount; i++) 
        {
            var startPos = roomSpawner.position;
            int children = 2; //Random.Range(1, 3);


                selectedDirection = (Direction)Random.Range(0, 4);
                MoveRoomSpawner();
                if (Physics2D.OverlapCircle(roomSpawner.position, .2f, whatIsRoom))
                {
                    roomSpawner.position = startPos;
                    i--;
                }
                else
                {
                    //Create Room
                    MoveDoorSpawner();
                    CreateDoor();
                    SetDoorSpawnerToRoomSpawner();
                    CreateRoom();

                }






        }
        */

    }

    /*
    private void SetDoorSpawnerToRoomSpawner()
    {
        doorSpawner.position = roomSpawner.position;
    }
    */


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    } //press R to Reload page

    /*
    public void MoveDoorSpawner()
    {
        switch (selectedDirection)
        {
            case Direction.up:
                doorSpawner.position += new Vector3(0f, yOffset/2, 0f);
                break;

            case Direction.down:
                doorSpawner.position += new Vector3(0f, -yOffset/2, 0f);
                break;
            case Direction.right:
                doorSpawner.position += new Vector3(xOffset/2, 0f, 0f);
                break;
            case Direction.left:
                doorSpawner.position += new Vector3(-xOffset/2, 0f, 0f);
                break;
        }
    }
    */

    public void MoveRoomSpawner()
    {
        switch (selectedDirection)
        {
            case Direction.up:
                roomSpawner.position += new Vector3(0f, yOffset, 0f);
                break;

            case Direction.down:
                roomSpawner.position += new Vector3(0f, -yOffset, 0f);
                break;
            case Direction.right:
                roomSpawner.position += new Vector3(xOffset, 0f, 0f);
                break;
            case Direction.left:
                roomSpawner.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }

    /*
    private void CreateDoor()
    {
        Instantiate(doorObject, doorSpawner.position, doorSpawner.rotation);
    }*/

    private void CreateRoom()
    {
        var roomID = Instantiate(roomObject, roomSpawner.position, roomSpawner.rotation);

        if (roomCounter == 0)
        {
            roomID.GetComponent<SpriteRenderer>().color = startColor;
        }

        if (roomCounter == roomAmount)
        {
            roomID.GetComponent<SpriteRenderer>().color = endColor;
        }

        roomCounter++;
    }
}
