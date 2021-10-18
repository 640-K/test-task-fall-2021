using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public SpriteRenderer doorSprite;
    public BoxCollider2D doorCollider;

    private bool _state;

    public void SetState(bool state)
    {
        _state = state;

        int value = state == true ? 0 : 1;
        LeanTween.alpha(doorSprite.gameObject, value, 0.5f).setOnComplete(ChangeColliderState);

        doorCollider.enabled = state;
    }


    public void ChangeColliderState()
    {
        if (_state) doorCollider.enabled = false;
        else doorCollider.enabled = true;
    }
}
