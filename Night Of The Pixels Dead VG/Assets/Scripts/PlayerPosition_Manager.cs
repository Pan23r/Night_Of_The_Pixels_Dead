using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition_Manager : MonoBehaviour
{
    GameObject player;
    Vector3 fromKitchen = new Vector3(-14.53f, 6.12f, 6.12f); // OLD: Vector3(-15, 6.27f, 6.27f)
    Vector3 fromBathRoom = new Vector3(5.03f, 6.12f, 6.12f); // OLD: Vector3(5.22f, 6.63f, 6.63f)
    Vector3 fromStairs= new Vector3(18.59f, 6.84f, 6.84f); // OLD: Vector3(19.4f, 8.2f, 8.2f)
    Vector3 fromBedRoom = new Vector3(-9.33f, 10.5f, 10.43f);
    Vector3 fromBedRoomGirl = new Vector3(9.9f, 10.5f, 10.26f);
    Vector3 fromLab = new Vector3(20.08f, -10.08f, 10.26f);
    Vector3 fromCellar = new Vector3(15.89f, 6.12f, 6.12f); // OLD: Vector3(16.5f, 6.65f, 10.26f)
    Vector3 fromAisleBunker = new Vector3(-2.86f, 5.76f, 5.76f);

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (Change_room_Manager.fromKitchen)
        {
            Change_room_Manager.fromKitchen = false;
            player.transform.position = fromKitchen;
            Camera.main.transform.position = fromKitchen;
        }
        else if (Change_room_Manager.fromBathRoom)
        {
            Change_room_Manager.fromBathRoom = false;
            player.transform.position = fromBathRoom;
            Camera.main.transform.position = fromBathRoom;
        }
        else if (Change_room_Manager.fromStairs)
        {
            Change_room_Manager.fromStairs = false;
            player.transform.position = fromStairs;
            Camera.main.transform.position = fromStairs;
        }
        else if (Change_room_Manager.fromBedRoom)
        {
            Change_room_Manager.fromBedRoom = false;
            player.transform.position = fromBedRoom;
            Camera.main.transform.position = fromBedRoom;
        }
        else if (Change_room_Manager.fromBedRoomGirl)
        {
            Change_room_Manager.fromBedRoomGirl = false;
            player.transform.position = fromBedRoomGirl;
            Camera.main.transform.position = fromBedRoomGirl;
        }
        else if (Change_room_Manager.fromLab)
        {
            Change_room_Manager.fromLab = false;
            player.transform.position = fromLab;
            Camera.main.transform.position = fromLab;
        }
        else if (Change_room_Manager.fromCellar)
        {
            Change_room_Manager.fromCellar = false;
            player.transform.position = fromCellar;
            Camera.main.transform.position = fromCellar;
        }
        else if (Change_room_Manager.fromAisleBunker)
        {
            Change_room_Manager.fromAisleBunker = false;
            player.transform.position = fromAisleBunker;
            Camera.main.transform.position = fromAisleBunker;
        }

    }
}
