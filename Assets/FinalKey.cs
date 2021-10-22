using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;

public class FinalKey : MonoBehaviour
{
    public bool isUsed { get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isUsed) return;

        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            player.transition.Transition();
            Destroy(gameObject);

            isUsed = true;
        }
    }
}
