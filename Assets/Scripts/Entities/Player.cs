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
        public GameObject uiCanvas;
        public GameObject pauseCanvas;
        public SceneTransition transition;

        public uint score = 0;

        public void AddScorePoints(uint points)
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

        public void stopGame()
        {
            Time.timeScale = 0;
            pauseCanvas.SetActive(true);
            uiCanvas.SetActive(false);
        }

        public void resumeGame()
        {
            Time.timeScale = 1;
            pauseCanvas.SetActive(false);
            uiCanvas.SetActive(true);
        }


        public override void Start()
        {
            base.Start();
            pauseCanvas.SetActive(false);
        }

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

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (Time.timeScale == 1)
                    stopGame();
                else
                    resumeGame();
            }

            Move(motion);
        }

        protected override void OnDie()
        {
            StartCoroutine(WaitBeforeExit());
            IEnumerator WaitBeforeExit()
            {
                yield return new WaitForSeconds(3);

                transition.targetScene = "LossScene";
                transition.Transition();
            }
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
