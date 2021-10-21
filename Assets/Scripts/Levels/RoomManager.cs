using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    public Room currentRoom;

    public List<Room> roomPrefabs;
    public List<Room> rooms;

    public UnityEvent onRoomDiscover;

    public int startRoomIndex;


    public void Awake()
    {
        instance = this;
    }


    public void Start()
    {
        Room instanced = Instantiate(roomPrefabs[startRoomIndex], transform);

        instanced.transform.position = new Vector3(0f, 0f, 0f);
        instanced.Generate();
        rooms.Add(instanced);

        GenerateNeighbourRooms(instanced);

        instanced.neighbourRoomsGenerated = true;

        currentRoom = instanced;
    }


    public void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            if (currentRoom.doorState) { currentRoom.CloseDoors(); }
            else
            {
                currentRoom.OpenDoors();
            }
        }
    }


    public void CloseDoorsInCurrentRoom()
    {
        currentRoom.CloseDoors();
    }


    public void OpenDoorsInCurrentRoom()
    {
        currentRoom.OpenDoors();
    }


    public void GenerateNeighbourRooms(Room room)
    {
        foreach(RoomConnector connector in room.roomConnectors)
        {
            List<int> indexes = InstantiateIndexList(roomPrefabs.Count);

            if (TryConnectToOtherRooms(connector)) continue;

            bool ok = false;

            while(indexes.Count > 0)
            {
                int rnd = RandomIndex(indexes);
                indexes.Remove(rnd);

                int secondRoomIndex;

                if (ConnectRoomToConnector(connector, roomPrefabs[rnd], out secondRoomIndex))
                {
                    Room instanced = Instantiate(roomPrefabs[rnd], transform);

                    Vector2 roomShift = new Vector2(
                        connector.connectionPoint.transform.position.x - instanced.roomConnectors[secondRoomIndex].connectionPoint.transform.position.x,
                        connector.connectionPoint.transform.position.y - instanced.roomConnectors[secondRoomIndex].connectionPoint.transform.position.y);

                    instanced.transform.position = new Vector3(
                        instanced.transform.position.x + roomShift.x, 
                        instanced.transform.position.y + roomShift.y, 
                        instanced.transform.position.z);

                    instanced.Generate();
                    rooms.Add(instanced);
                    ok = true;

                    connector.pairedConnector = instanced.roomConnectors[secondRoomIndex];
                    connector.occupied = true;
                    instanced.roomConnectors[secondRoomIndex].pairedConnector = connector;
                    instanced.roomConnectors[secondRoomIndex].occupied = true;
                    instanced.ForceOpenDoors();

                    break;
                }
            }

            if (!ok && !connector.occupied) connector.DisableConnector();
        }

        room.neighbourRoomsGenerated = true;
    }


    public bool TryConnectToOtherRooms(RoomConnector connector)
    {
        foreach(Room room in rooms)
        {
            if (connector.parentRoom != room)
            {
                foreach (RoomConnector _connector in room.roomConnectors)
                {
                    if (VectorApproximately(_connector.connectionPoint.transform.position, connector.connectionPoint.transform.position) && !_connector.occupied)
                    {
                        _connector.pairedConnector = connector;
                        _connector.occupied = true;
                        connector.pairedConnector = _connector;
                        connector.occupied = true;

                        connector.parentRoom.OpenDoors();

                        return true;
                    }
                }
            }
        }

        return false;
    }


    public bool VectorApproximately(Vector3 first, Vector3 second)
    {
        if (Mathf.Approximately(first.x, second.x) &&
            Mathf.Approximately(first.y, second.y) &&
            Mathf.Approximately(first.z, second.z)) return true;
        return false;
    }


    public bool ConnectRoomToConnector(RoomConnector connector, Room room, out int roomConnectorIndex)
    {
        if (connector.occupied) { roomConnectorIndex = -1; return false; }

        for(int i = 0; i < room.roomConnectors.Count; i++)
        {
            if (CheckIfConnectorsAreEligibleForConnecting(connector, room.roomConnectors[i]))
            {
                Vector2 roomShift = new Vector2(
                    connector.connectionPoint.transform.position.x - room.roomConnectors[i].connectionPoint.transform.position.x,
                    connector.connectionPoint.transform.position.y - room.roomConnectors[i].connectionPoint.transform.position.y);

                room.transform.position = new Vector3(
                    room.transform.position.x + roomShift.x,
                    room.transform.position.y + roomShift.y,
                    room.transform.position.z);

                if (!CheckIfRoomIsIntersectingWithOthers(room))
                {
                    roomConnectorIndex = i;
                    return true;
                }
            }
        }

        roomConnectorIndex = -1;
        return false;
    }


    public bool CheckIfRoomIsIntersectingWithOthers(Room room)
    {

        foreach (Room secondRoom in rooms)
        {
            if (CheckIfOverlap(room, secondRoom)) return true;
        }

        return false;
    }


    public bool CheckIfOverlap(Room firstRoom, Room secondRoom)
    {
        if (firstRoom.UpLeftCorner.position.x == firstRoom.DownRightCorner.position.x 
            || firstRoom.UpLeftCorner.position.y == firstRoom.DownRightCorner.position.y 
            || secondRoom.UpLeftCorner.position.x == secondRoom.DownRightCorner.position.x
            || secondRoom.UpLeftCorner.position.y == secondRoom.DownRightCorner.position.y)
        {
            return false;
        }

        if (firstRoom.UpLeftCorner.position.x >= secondRoom.DownRightCorner.position.x 
            || secondRoom.UpLeftCorner.position.x >= firstRoom.DownRightCorner.position.x)
        {
            return false;
        }

        if (firstRoom.DownRightCorner.position.y >= secondRoom.UpLeftCorner.position.y 
            || secondRoom.DownRightCorner.position.y >= firstRoom.UpLeftCorner.position.y)
        {
            return false;
        }
        return true;
    }


    public bool CheckIfConnectorsAreEligibleForConnecting(RoomConnector first, RoomConnector second)
    {
        return Mathf.Approximately(
            Vector2.Angle(
            (Quaternion.AngleAxis(first.connectionPoint.transform.rotation.z, Vector3.up) * first.connectionPoint.transform.up).normalized, 
            (Quaternion.AngleAxis(second.connectionPoint.transform.rotation.z, Vector3.up) * second.connectionPoint.transform.up).normalized), 
            180);
    }


    public List<int> InstantiateIndexList(int count)
    {
        List<int> temp = new List<int>();

        for(int i = 0; i < count; i++)
        {
            temp.Add(i);
        }

        return temp;
    }


    public int RandomIndex(List<int> indexes)
    {
        return indexes[Random.Range(0, indexes.Count)];
    }


    public void DisableAllRoomsExcept(List<Room> _rooms)
    {
        foreach(Room room in rooms)
        {
            if (!_rooms.Contains(room))
            {
                room.gameObject.SetActive(false);
            }
        }
    }
}
