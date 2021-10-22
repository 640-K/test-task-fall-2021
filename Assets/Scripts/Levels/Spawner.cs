using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI;
using Entities;

public class Spawner : MonoBehaviour
{
    public int min;
    public int max;

    public List<GameObject> prefabs;
    public AIGround ground;

    public void Spawn()
    {
        AI.Grid grid = ground.pathfinding.grid;
        for (int i = 0; i < Random.Range(min, max); i++)
        {
            Vector2 position = grid.GetLocalPosition(Random.Range(0, grid.width), Random.Range(0, grid.height));
            GameObject gameObject = prefabs[Random.Range(0, prefabs.Count)];
            Mob mob = gameObject.GetComponent<Mob>();
            mob.ground = ground;
            mob.maxSpeed = Random.Range(2.5f, 4f);
            mob.health = (uint)Random.Range(15, 50);
            mob.alwaysFollow = true;
            mob.spawner = this;
            mob.waitTime = Random.Range(0.1f, 1f);
            gameObject.transform.position = position;
            gameObject = Instantiate(gameObject) as GameObject;
            gameObject.transform.SetParent(ground.gameObject.transform);
            gameObject.transform.localPosition = position;
        }

    }

public void Die()
    {
        Debug.Log(3);
    }
}
