using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Entities
{

    public abstract class Entity : MonoBehaviour
    {
        [Header("Features")]
        public uint health;


        [Header("Movement")]
        public float maxSpeed;

        [Range(0, 1)] [Tooltip("Ability to develop speed. If 1 is given, the speed change is instant.")]
        public float accelerationRate;

        [Range(0, 1)] [Tooltip("Speed decrease when the player does not move. If 1 is given, the speed drops to 0 instantly.")] 
        public float deccelerationRate;

        [Range(0, 1)] [Tooltip("Ability to resist excentric force when turning. If 1 is given, the excentic force does not influence the movement.")]
        public float maneuverability;



        [Header("Components")]
        public GameObject body;
        public Rigidbody2D physicsBody;
        public Collider2D collisionBounds;
        public Animator animator;
        public Weapon weapon;



        public uint currentHealth { get; protected set; }
        public Vector2 motionDirection { get; protected set; }
        public bool dead { get => currentHealth == 0; }


        public virtual void Start()
        {
            currentHealth = health;
            motionDirection = Vector2.zero;
        }

        public virtual void OnValidate()
        {
            // acceleration can't have 0 value, or else the player won't move,
            // so it is set to physics default time
            if (accelerationRate == 0)
                accelerationRate = Time.fixedDeltaTime;
        }

        protected virtual void FixedUpdate()
        {
            if (dead)
                motionDirection = Vector2.zero;
            

            if (motionDirection.magnitude > 0)
            {
                Vector2 perpMotionDirection = Vector2.Perpendicular(motionDirection);
                physicsBody.AddForce(-perpMotionDirection * physicsBody.mass * Vector2.Dot(physicsBody.velocity, perpMotionDirection) * maneuverability, ForceMode2D.Impulse);

                physicsBody.AddForce(motionDirection * physicsBody.mass * (maxSpeed - Vector2.Dot(physicsBody.velocity, motionDirection)) * accelerationRate, ForceMode2D.Impulse);

                OnMove();
            }
            else
                physicsBody.AddForce(-physicsBody.velocity * physicsBody.mass * deccelerationRate, ForceMode2D.Impulse);
        }




        public virtual void Move(Vector2 direction)
        {
            direction.Normalize();
            if (dead || direction == motionDirection) return;

            if (motionDirection.magnitude == 0)
            {
                if (direction.magnitude != 0)
                {
                    animator.SetInteger("state", 1);
                    OnStartMoving();
                }
            }
            else if (direction.magnitude == 0)
            {
                animator.SetInteger("state", 0);
                OnStopMoving();
            }

            if (direction.x > 0)
                body.transform.localScale = Vector3.one;
            if(direction.x < 0)
                body.transform.localScale = new Vector3(-1, 1, 1);


            motionDirection = direction;
        }

        public int Hurt(uint damage)
        {
            if (dead) return 0;

            currentHealth -= Math.Min(damage, currentHealth);

            if (dead)
            {
                animator.SetInteger("state", 2);
                OnDie();
                return 1;
            }
            OnHurt();
            return 0;
        }

        public void Heal(uint factor)
        {
            if (dead) return;

            currentHealth = Math.Min(currentHealth + factor, health);
            OnHeal();
        }

        public void Kill() => Hurt(health);
        
        public virtual void Attack() => animator.SetTrigger("attack");


        protected abstract void OnStartMoving();
        
        protected abstract void OnMove();

        protected abstract void OnStopMoving();

        protected abstract void OnDie();

        protected abstract void OnHurt();

        protected abstract void OnHeal();


    }
}
