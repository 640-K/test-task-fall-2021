using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;

public class HealthPotion : MonoBehaviour
{
    public uint healingFactor;

    public bool isUsed { get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isUsed) return;

        Player player = collision.GetComponent<Player>();

        if(player != null)
        {
            player.Heal(healingFactor);
            Destroy(gameObject);

            isUsed = true;  
        }
    }
}
