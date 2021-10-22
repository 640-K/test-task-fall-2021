using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<RoomConnector> roomConnectors;

    [Space]
    [Space]

    public List<SpawnArea> spawnAreas;

    [Space]
    [Space]

    public List<GameObject> roomObjects;

    public AIGround ground;

    public Transform UpLeftCorner;
    public Transform DownRightCorner;

    public bool roomDiscovered;
    public bool neighbourRoomsGenerated;

    public bool doorState = true;


    public void Generate()
    {
        gameObject.SetActive(true);

        foreach(SpawnArea area in spawnAreas)
        {
            area.parentRoom = this;
            area.Spawn();
            roomObjects.AddRange(area.instanciatedObjects);
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
            OnRoomEnter();
    }


    public void OnRoomEnter()
    {
        if (!neighbourRoomsGenerated)
        {
            RoomManager.instance.GenerateNeighbourRooms(this);
        }
        

        if (!roomDiscovered) {


            roomDiscovered = true;
            RoomManager.instance.onRoomDiscover?.Invoke();

            if (GetComponentInParent<RoomManager>().rooms[0] != this)
            {
                GetComponent<Spawner>().Spawn();
                CloseDoors();
            }
        }

        List<Room> roomToEnable = GetNeighbourRooms();
        roomToEnable.Add(this);

        foreach(Room room in roomToEnable)
        {
            room.gameObject.SetActive(true);
        }

        RoomManager.instance.DisableAllRoomsExcept(roomToEnable);

        RoomManager.instance.currentRoom = this;

     
    }


    public void OpenDoors()
    {
        foreach(RoomConnector roomConnector in roomConnectors)
        {
            roomConnector.OpenDoor();
            roomConnector.pairedConnector?.OpenDoor();
        }

        doorState = true;
    }


    public void ForceOpenDoors()
    {
        foreach (RoomConnector roomConnector in roomConnectors)
        {
            roomConnector.ForceOpenDoor();
            roomConnector.pairedConnector?.ForceOpenDoor();
        }

        doorState = true;
    }

    public void CloseDoors()
    {
        foreach (RoomConnector roomConnector in roomConnectors)
        {
            roomConnector.CloseDoor();
            roomConnector.pairedConnector?.CloseDoor();
        }

        doorState = false;
    }


    public List<Room> GetNeighbourRooms()
    {
        List<Room> rooms = new List<Room>();

        foreach(RoomConnector roomConnector in roomConnectors)
        {
            if (roomConnector.occupied) rooms.Add(roomConnector.pairedConnector.parentRoom);
        }

        return rooms;
    }
}
