using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject open;
    public GameObject close;
    public BoxCollider2D doorCollider;

    public void SetState(bool state)
    {
        if(state) { open.SetActive(true); }
        else { open.SetActive(false); }
        doorCollider.enabled = state;
    }
}
