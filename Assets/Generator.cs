﻿using System.Collections;
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

    public int distanceToEnd;

    public int roomAmount = 10;

    public Transform generatorPoint;
    public Transform roomSpawner;
    public Transform doorSpawner;

    public enum Direction { up, right, down, left };
    public Direction selectedDirection;

    public float xOffset = 1f, yOffset = 1f;

    public LayerMask whatIsRoom;

    private GameObject endRoom;
    private int roomCounter = 0;


    // Start is called before the first frame update
    void Start()
    {
        // Generate start Room
        CreateRoom();
        selectedDirection = (Direction)Random.Range(0, 4);
        MoveDoorSpawner();
        CreateDoor();
        MoveRoomSpawner();
        SetDoorSpawnerToRoomSpawner();

        for (int i = 0; i < roomAmount; i++) 
        {
            CreateRoom();

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveDoorSpawner();
            CreateDoor();
            MoveRoomSpawner();
            SetDoorSpawnerToRoomSpawner();

            while (Physics2D.OverlapCircle(roomSpawner.position, .2f, whatIsRoom))
            {
                MoveRoomSpawner();
            }




        }

        /*
        var numberOfDoors = Random.Range(0, 3);

        var startPos = roomSpawner.position;

        for (int i = 0; i < numberOfDoors; i++)
        {
            selectedDirection = (Direction)Random.Range(0, 4);
            MoveDoorSpawner();
            CreateDoor();
            MoveRoomSpawner();
            CreateRoom();
            SetDoorSpawnerToRoomSpawner();
            roomSpawner.position = startPos;
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
