using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

namespace Entities{
    public class Mob : Entity
    {
        public AIGround ground;

        public GameObject player;

        public bool showPath;

        public bool alwaysFollow;

        public bool showGizmos;

        public float followRadius;

        public bool alwaysFollowAfterSee;

        void Update()
        {
            Vector2 motion = Vector2.zero;
            PathNode playerNode = ground.pathfinding.grid.getNodeFromWorldPosition(player.transform.position);
            PathNode startNode = ground.pathfinding.grid.getNodeFromWorldPosition(transform.position);
            if (playerNode == null || startNode == null) {
                Move(motion);
                return;
            }

            if (!alwaysFollow)
                if (Vector2.Distance(player.transform.position, transform.position) > followRadius)
                {
                    Move(motion);
                    return;
                }
                else
                    if(alwaysFollowAfterSee)
                    alwaysFollow = true;

            List<PathNode> path = ground.pathfinding.FindPath(startNode.x, startNode.y, playerNode.x, playerNode.y);

            if (path != null)
            {
                if (showPath)
                {
                    Debug.DrawLine(ground.pathfinding.grid.GetWorldPositionGizmos(startNode.x, startNode.y), ground.pathfinding.grid.GetWorldPositionGizmos(path[0].x, path[0].y), Color.green);
                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        Debug.DrawLine(ground.pathfinding.grid.GetWorldPositionGizmos(path[i].x, path[i].y), ground.pathfinding.grid.GetWorldPositionGizmos(path[i + 1].x, path[i + 1].y), Color.green);
                    }
                }
                if (startNode.y <= path[0].y)
                    motion += Vector2.up;
                if (startNode.x >= path[0].x)
                    motion -= Vector2.right;
                if (startNode.y >= path[0].y)
                    motion -= Vector2.up;
                if (startNode.x <= path[0].x)
                    motion += Vector2.right;
            }

            Move(motion);
        }

        protected override void OnDie()
        {
        }

        protected override void OnMove()
        {
        }

        protected override void OnStartMoving()
        {
        }

        protected override void OnStopMoving()
        {
        }

        private void OnDrawGizmos()
        {
            if (alwaysFollow|| !showGizmos)
                return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, followRadius);
        }
    }
}