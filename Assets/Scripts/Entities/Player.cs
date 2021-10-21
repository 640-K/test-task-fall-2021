using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class Player : Entity
    {
        // Update is called once per frame
        void Update()
        {
            ApplyMotion();
        }

        protected void ApplyMotion()
        {
            Vector2 motion = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
                motion += Vector2.up;
            if (Input.GetKey(KeyCode.A))
                motion -= Vector2.right;
            if (Input.GetKey(KeyCode.S))
                motion -= Vector2.up;
            if (Input.GetKey(KeyCode.D))
                motion += Vector2.right;

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

        protected override void OnHurt()
        {
        }

        protected override void OnHeal()
        {
        }
    }
}
