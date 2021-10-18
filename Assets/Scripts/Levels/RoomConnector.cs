using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnector : MonoBehaviour
{
    public bool occupied;

    public Door door;
    public Room parentRoom;

    [SerializeField] private GameObject onDisabledConnectorObject;

    public Transform connectionPoint;

    public RoomConnector pairedConnector;


    public void OpenDoor()
    {
        door.SetState(true);
    }

    public void CloseDoor()
    {
        door.SetState(false);
    }

    public void DisableConnector()
    {
        gameObject.SetActive(false);
        onDisabledConnectorObject.SetActive(true);
    }
}
