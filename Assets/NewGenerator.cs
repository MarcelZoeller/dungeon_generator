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




    // Start is called before the first frame update
    void Start()
    {
        //Place Start Room
        PlaceStartRoom();
        ExpandMainBranch();

    }
    private void PlaceStartRoom()
    {
        var roomID = Instantiate(roomObject, transform.position, transform.rotation);
        roomID.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 1f);
        roomID.GetComponent<Room>().desciption = "Start";
        allRoomList.Add(roomID.GetComponent<Room>());
    }
    private void ExpandMainBranch()
    { 
        //Room Random Count
        int spawnRoomCount = Random.Range(1, 4);

        print("Count:" + spawnRoomCount);

        //Create Room
        for (int i = 1; i <= spawnRoomCount; i++)
        {
            print("i="+i);
            // select latest Room
            Room latestRoom = allRoomList[allRoomList.Count - 1];

            //Set Generator Pos to latest Room
            transform.position = latestRoom.gameObject.transform.position;

            // Check which Doors are free
            CheckFreeDoors(latestRoom);

            //move spawner to doorPos
            var pickedDir = Random.Range(0, latestRoom.freeEdges.Count);
            switch (latestRoom.freeEdges[pickedDir])
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
            Instantiate(doorObject, transform.position, transform.rotation);

            //move spawner to destination
            switch (latestRoom.freeEdges[pickedDir])
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



            //Create Room
            var roomID = Instantiate(roomObject, transform.position, transform.rotation);
            allRoomList.Add(roomID.GetComponent<Room>());
        }

    }

    private void CheckFreeDoors(Room checkThisRoom)
    {

        Vector3 startPos = transform.position;

        //check right
        transform.position += new Vector3(1f, 0f, 0f);
        if (Physics2D.OverlapCircle(transform.position, .1f, whatIsRoom) == false)
        {
            checkThisRoom.freeEdges.Add("right");
        }
        transform.position = startPos;

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

        //check left
        transform.position += new Vector3(0f, 1f, 0f);
        if (Physics2D.OverlapCircle(transform.position, .1f, whatIsRoom) == false)
        {
            checkThisRoom.freeEdges.Add("left");
        }
        transform.position = startPos;

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