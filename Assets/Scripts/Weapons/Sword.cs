using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;

public class Sword : Weapon
{
    List<Entity> entitiesInRange;

    private void Awake()
    {
        entitiesInRange = new List<Entity>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var entity = collision.GetComponent<Entity>();

        if (entity != null && entity != owner) 
            entitiesInRange.Add(entity);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var entity = collision.GetComponent<Entity>();

        if (entity != null && entity != owner)
            entitiesInRange.Remove(entity);
    }

    public override void Use()
    {
        foreach(var entity in entitiesInRange)
        {
            if(entity.tag != owner.tag)
            {

                if (entity.Hurt(damage) == 1 && entity.gameObject.tag != "Player")
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddScorePoints(1);
            }
            
        }
    }
}
