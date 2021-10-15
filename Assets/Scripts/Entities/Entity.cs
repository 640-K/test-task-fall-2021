using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Entities
{
    public abstract class Entity : MonoBehaviour
    {
        [Header("Features")]
        public uint health;
        public uint armor;



        [Header("Movement")]
        public float maxSpeed;

        [Range(0, 1)] [Tooltip("Ability to develop speed. If 1 is given, the speed change is instant.")]
        public float accelerationRate;

        [Range(0, 1)] [Tooltip("Speed decrease when the player does not move. If 1 is given, the speed drops to 0 instantly.")] 
        public float deccelerationRate;

        [Range(0, 1)] [Tooltip("Ability to resist excentric force when turning. If 1 is given, the excentic force does not influence the movement.")]
        public float maneuverability;



        [Header("Components")]
        public Rigidbody2D physicsBody;
        public Collider2D collisionBounds;



        public uint currentHealth { get; private set; }
        public uint currentArmor { get; private set; }
        public Vector2 motionDirection { get; private set; }
        public bool dead { get => currentHealth == 0; }



        public virtual void Start()
        {
            currentHealth = health;
            currentArmor = armor;
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
            if (motionDirection.magnitude > 0)
            {
                Vector2 perpMotionDirection = Vector2.Perpendicular(motionDirection);
                physicsBody.AddForce(-perpMotionDirection * Vector2.Dot(physicsBody.velocity, perpMotionDirection) * physicsBody.mass * maneuverability, ForceMode2D.Impulse);

                physicsBody.AddForce(motionDirection * physicsBody.mass * (maxSpeed - Vector2.Dot(physicsBody.velocity, motionDirection)) * accelerationRate, ForceMode2D.Impulse);

                OnMove();
            }
            else
                physicsBody.AddForce(-physicsBody.velocity * physicsBody.mass * deccelerationRate, ForceMode2D.Impulse);

        }
            


        public virtual void Move(Vector2 direction)
        {
            direction.Normalize();
            motionDirection = direction;
        }



        public void Hurt(uint damage)
        {
            if (currentHealth == 0) return;

            if (currentArmor > 0)
                currentArmor -= Math.Min(damage, currentArmor);
            else
                currentHealth -= Math.Min(damage, currentHealth);

            if (currentHealth == 0)
                OnDie();
        }

        public void Heal(uint factor)
        {
            currentHealth = Math.Min(currentHealth + factor, health);
        }

        public void RepairArmor(uint factor)
        {
            currentArmor = Math.Min(currentArmor + factor, armor);
        }


        protected abstract void OnMove();

        protected abstract void OnDie();
    }
}
