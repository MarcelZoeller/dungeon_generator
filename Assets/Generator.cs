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
    public Transform doorSpawner;

    public enum Direction { up, right, down, left };
    public Direction selectedDirection;

    public float xOffset = 1f, yOffset = 1f;

    public LayerMask whatIsRoom;

    private GameObject endRoom;
    private int roomCounter = 0;


    public List<GameObject> roomList = new List<GameObject>();

    


    // Start is called before the first frame update
    void Start()
    {

        // 1. Generate start Room
        var roomID = Instantiate(roomObject, roomSpawner.position, roomSpawner.rotation);
        roomList.Add(roomID);

        // 2.1. Pick Random Room
        //pick random room
        int pickedRoomID = Random.Range(0, roomList.Count);
        GameObject pickedRoomObj = roomList[pickedRoomID];

        // check if room has empty spaces


        //reset list
        pickedRoomObj.GetComponent<Room>().freeSpaces.Clear();
        //check up 
        roomSpawner.position = pickedRoomObj.transform.position;
        roomSpawner.position += new Vector3(0f, 1f, 0f);
         if (Physics2D.OverlapCircle(roomSpawner.position, .2f, whatIsRoom) == false)
         {
            
            pickedRoomObj.GetComponent<Room>().freeSpaces.Add(1);
         }

        //check down
        roomSpawner.position = pickedRoomObj.transform.position;
        roomSpawner.position += new Vector3(0f, -1f, 0f);
        if (Physics2D.OverlapCircle(roomSpawner.position, .2f, whatIsRoom) == false)
        {
            pickedRoomObj.GetComponent<Room>().freeSpaces.Add(2);
        }

        //check right
        roomSpawner.position = pickedRoomObj.transform.position;
        roomSpawner.position += new Vector3(1f, 0f, 0f);
        if (Physics2D.OverlapCircle(roomSpawner.position, .2f, whatIsRoom) == false)
        {
            pickedRoomObj.GetComponent<Room>().freeSpaces.Add(3);
        }

        //check left
        roomSpawner.position = pickedRoomObj.transform.position;
        roomSpawner.position += new Vector3(1f, 0f, 0f);
        if (Physics2D.OverlapCircle(roomSpawner.position, .2f, whatIsRoom) == true)
        {
            pickedRoomObj.GetComponent<Room>().freeSpaces.Add(4);
        }

        

        if (pickedRoomObj.GetComponent<Room>().freeSpaces.Count > 0) {

            var pickedDir = Random.Range(0, pickedRoomObj.GetComponent<Room>().freeSpaces.Count - 1);
            

            // switch room dir
            
            
            //room has free spaces
            roomID = Instantiate(roomObject, roomSpawner.position, roomSpawner.rotation);
            roomList.Add(roomID);


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

    private void SetDoorSpawnerToRoomSpawner()
    {
        doorSpawner.position = roomSpawner.position;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    } //press R to Reload page

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

    private void CreateDoor()
    {
        Instantiate(doorObject, doorSpawner.position, doorSpawner.rotation);
    }

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
