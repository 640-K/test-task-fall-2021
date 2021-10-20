using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject spikesUp;
    public GameObject tempSpikes;
    public GameObject spikesDown;
    public BoxCollider2D doorCollider;

    private bool _state;

    public void SetState(bool state)
    {
        _state = state;

        LeanTween.value(gameObject, 0f, 0f, 0.25f).setOnComplete(Temp);

        doorCollider.enabled = state;
    }

    public void Temp()
    {
        if (_state)
        {
            tempSpikes.gameObject.SetActive(true);
            spikesUp.gameObject.SetActive(false);
            LeanTween.value(gameObject, 0f, 0f, 0.25f).setOnComplete(ChangeColliderState);
        }
        else
        {
            tempSpikes.gameObject.SetActive(true);
            spikesDown.gameObject.SetActive(false);
            LeanTween.value(gameObject, 0f, 0f, 0.25f).setOnComplete(ChangeColliderState);
        }
    }


    public void ChangeColliderState()
    {
        if (_state)
        {
            doorCollider.enabled = false;
            tempSpikes.gameObject.SetActive(false);
            spikesDown.gameObject.SetActive(true);
        }
        else
        {
            tempSpikes.gameObject.SetActive(false);
            spikesUp.gameObject.SetActive(true);
            doorCollider.enabled = true;
        }
    }
}
