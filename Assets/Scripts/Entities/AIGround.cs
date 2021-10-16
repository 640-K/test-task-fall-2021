using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class AIGround : MonoBehaviour
{
    [Range(0.1f, 1)]
    public float size;

    public bool showGizmos;

    public Transform end;
    public LayerMask walkableMask;

    public Pathfinding pathfinding;

    void Awake()
    {
        pathfinding = new Pathfinding(size, transform, end, walkableMask);
    }



    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;
        AI.Grid grid = new AI.Grid(size, transform.position, end.position, walkableMask);
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                PathNode node = grid.GetPathNode(x, y);
                if (node.isWalkable)
                    Gizmos.color = new Color(0, 1, 0, 0.5f);
                else
                    Gizmos.color = new Color(1, 0, 0, 0.5f);
                Gizmos.DrawCube(grid.GetWorldPositionGizmos(x, y), Vector3.one * size);
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(grid.GetWorldPositionGizmos(x, y), Vector3.one*size);
            }
        }
    }
}
