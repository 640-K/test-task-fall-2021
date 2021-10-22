using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

namespace Entities{
    public class Mob : Entity
    {
        public AIGround ground;

        private GameObject player;

        public bool showPath;

        public bool alwaysFollow;

        public bool showGizmos;

        public float followRadius;

        public bool alwaysFollowAfterSee;

        private bool isAttack = false;

        public Spawner spawner;

        [Range(0.1f, 5)]
        public float waitTime;

        public override void Start()
        {
            base.Start();
            player = GameObject.FindGameObjectWithTag("Player");
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            foreach (GameObject mob in GameObject.FindGameObjectsWithTag("Mob"))
                if(mob != this)
                    Physics2D.IgnoreCollision(mob.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            


        }

        void Update()
        { 



            Vector2 motion = Vector2.zero;
            if (isAttack || player.GetComponent<Player>().dead)
            {
                Move(Vector2.zero);
                return;
            }
            if (Vector2.Distance(player.transform.position, transform.position) < 1)
            {
                Move(Vector2.zero);
                if(!isAttack)
                    StartCoroutine(WaitAttack());
                
                return;
            }

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


        IEnumerator WaitAttack()
        {
            isAttack = true;
            yield return new WaitForSeconds(0.1f);
            Attack();
            yield return new WaitForSeconds(waitTime);
            isAttack = false;
        }

        protected override void OnDie()
        {

            spawner.Die();
            StartCoroutine(AfterDie());
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

        protected override void OnHurt()
        {
        }

        protected override void OnHeal()
        {
        }

        private void OnDrawGizmos()
        {
            if (alwaysFollow|| !showGizmos)
                return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, followRadius);
        }

        IEnumerator AfterDie()
        {
            yield return new WaitForSeconds(5);
            Destroy(this);
        }
    }
}