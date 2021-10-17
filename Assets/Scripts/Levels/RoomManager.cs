using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    public List<Room> roomPrefabs;
    public List<Room> rooms;


    public void Awake()
    {
        instance = this;
    }


    public void Start()
    {
        Room instanced = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Count - 1)], transform);

        instanced.transform.position = new Vector3(0f, 0f, 0f);
        instanced.Generate();
        rooms.Add(instanced);

        GenerateNeighbourRooms(instanced);

        instanced.neighbourRoomsGenerated = true;
    }


    public void GenerateNeighbourRooms(Room room)
    {
        foreach(RoomConnector connector in room.roomConnectors)
        {
            List<int> indexes = InstantiateIndexList(roomPrefabs.Count);

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

                    break;
                }
            }

            if (!ok) connector.DisableConnector();
        }

        room.neighbourRoomsGenerated = true;
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
            if (CheckIfOverlap(room, secondRoom)) return false;
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
        return indexes[Random.Range(0, indexes.Count - 1)];
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
