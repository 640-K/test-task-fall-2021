using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Entities
{
    public class Player : Entity
    {
        private const uint MaxScore = 100;

        public HealthBar healthBar;
        public Text scoreText;

        public uint score = 0;

        void AddScorePoints(uint points)
        {
            if (score + points <= MaxScore)
            {
                score += points;
                scoreText.text = "Score: " + score;
            }
            else
            {
                // WIN
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
                Attack();

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
            healthBar.SetSliderValueGradually(currentHealth);
        }

        protected override void OnHeal()
        {
            healthBar.SetSliderValueGradually(currentHealth);
        }
    }
}
