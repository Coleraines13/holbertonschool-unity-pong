using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
namespace ZPong
{

    public class Ball : MonoBehaviour
    {
        public float speed = 5f;

        private float screenTop = 527;
        private float screenBottom = -527;

        private Vector2 direction;
        private Vector2 defaultDirection;

        private bool ballActive;

        protected RectTransform rectTransform;

        private AudioSource bounceSFX;


        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();

            if (PlayerPrefs.HasKey("BallSpeed"))
            {
                speed = PlayerPrefs.GetFloat("BallSpeed");
            }

            if (PlayerPrefs.HasKey("BallSize"))
            {
                var value = PlayerPrefs.GetFloat("BallSize");
                rectTransform.sizeDelta = new Vector2(value, value);
            }

            if (PlayerPrefs.HasKey("PitchDirection"))
            {
                string pitchDirectionValue = PlayerPrefs.GetString("PitchDirection");
    
                if (pitchDirectionValue == "Random")
                {
                    // Generate a random direction between -1 and 1 for the x-axis.
                    float randomX = Random.Range(-1f, 1f);
                    direction = new Vector2(randomX, 0f).normalized;
                }
                else if (pitchDirectionValue == "Right")
                {
                    // Set the direction to move right.
                    direction = new Vector2(1f, 0f);
                }
                else
                {
                    // Default to moving left if the value is not recognized.
                    direction = new Vector2(-1f, 0f);
                }
            }
            else
            {
                // Default to moving left if no value is found in PlayerPrefs.
                direction = new Vector2(-1f, 0f);
            }

            defaultDirection = direction;

            SetHeightBounds();

            bounceSFX = this.GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (ballActive)
            {


                Vector2 newPosition = rectTransform.anchoredPosition + (direction * speed * Time.deltaTime);

                rectTransform.anchoredPosition = newPosition;


                if (rectTransform.anchoredPosition.y >= screenTop || rectTransform.anchoredPosition.y <= screenBottom)
                {
                    direction.y *= -1f;
                    PlayBounceSound();
                }
            }
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Paddle"))
            {
                Paddle paddle = collision.gameObject.GetComponent<Paddle>();

                float y = BallHitPaddleWhere(GetPosition(), paddle.AnchorPos(),
                    paddle.GetComponent<RectTransform>().sizeDelta.y / 2f);
                Vector2 newDirection = new Vector2(paddle.isLeftPaddle ? 1f : -1f, y);

                Reflect(newDirection);
                PlayBounceSound();
            }
            else if (collision.gameObject.CompareTag("Goal"))
            {
                // Debug.Log("pos: " + rectTransform.anchoredPosition.x);
                //Left goal
                if (this.rectTransform.anchoredPosition.x < -1)
                {
                    ScoreManager.Instance.ScorePointPlayer2();
                }
                //Right goal
                else
                {
                    ScoreManager.Instance.ScorePointPlayer1();
                }
            }
        }

        public void Reflect(Vector2 newDirection)
        {
            direction = newDirection.normalized;
        }

        public void SetBallActive(bool value)
        {
            ballActive = value;
            direction = defaultDirection;
        }

        public Vector2 GetPosition()
        {
            return rectTransform.anchoredPosition;
        }

        public void SetHeightBounds()
        {
            var height = UIScaler.Instance.GetUIHeightPadded();

            screenTop = height / 2;
            screenBottom = -1 * height / 2;
        }

        protected float BallHitPaddleWhere(Vector2 ball, Vector2 paddle, float paddleHeight)
        {
            return (ball.y - paddle.y) / paddleHeight;
        }

        void PlayBounceSound()
        {
            bounceSFX.pitch = Random.Range(.8f, 1.2f);
            bounceSFX.Play();
        }
        
        public void DisableBall()
        {
            ballActive = false;
        }

    }
}