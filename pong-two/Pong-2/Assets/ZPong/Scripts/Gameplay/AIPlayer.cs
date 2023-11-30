using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZPong
{

    public enum AILevel
    {
        Easy = 0,
        Medium = 1,
        Hard = 2
    }

    public class AIPlayer : MonoBehaviour
    {
        public float speed = 5f;
        public AILevel difficulty = AILevel.Easy;

        private float halfPlayerHeight;
        private Ball ball;

        private Paddle thisPaddle;

        private bool letsPlay;

        private void Start()
        {
            if (PlayerPrefs.HasKey("PaddleSpeed"))
            {
                speed = PlayerPrefs.GetFloat("PaddleSpeed");
            }
            if (PlayerPrefs.HasKey("AILevel"))
            {
                difficulty = (AILevel) PlayerPrefs.GetInt("AILevel");
            }

            StartCoroutine(StartDelay());
        }

        private void FixedUpdate()
        {
            if (letsPlay)
            {
                if (difficulty == AILevel.Easy)
                {
                    thisPaddle.Move(Math.Sign(ball.transform.position.y - transform.position.y) * speed *
                                    Time.fixedDeltaTime);
                }
                else if (difficulty == AILevel.Medium)
                {
                    thisPaddle.Move(Math.Sign(ball.transform.position.y - transform.position.y) * speed *
                                    Time.fixedDeltaTime * 1.2f);
                }
                else
                {
                    thisPaddle.Move(Math.Sign(ball.transform.position.y - transform.position.y) * speed *
                                    Time.fixedDeltaTime * 1.5f);
                }
            }
        }

        IEnumerator StartDelay()
        {
            //Disable AI from playing
            letsPlay = false;

            var playerParent = transform.parent;

            halfPlayerHeight = transform.localScale.y / 2f;

            thisPaddle = playerParent.GetComponent<Paddle>();

            // Disabling the Player script if present
            Player playerScript = playerParent.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.enabled = false;
            }
            
            //Delay start
            yield return new WaitForSeconds(3f);
            
            //Enable AI to react
            ball = GameManager.Instance.activeBall;
            letsPlay = true;
        }
    }
}