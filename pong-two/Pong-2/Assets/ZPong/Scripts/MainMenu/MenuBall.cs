using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace ZPong
{

    public class MenuBall : Ball
    {

        protected new void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Paddle"))
            {
                Paddle paddle = collision.gameObject.GetComponent<Paddle>();

                float y = BallHitPaddleWhere(GetPosition(), paddle.AnchorPos(),
                    paddle.GetComponent<RectTransform>().sizeDelta.y / 2f);
                Vector2 newDirection = new Vector2(paddle.isLeftPaddle ? 1f : -1f, y);

                Reflect(newDirection);
            }
            else if (collision.gameObject.CompareTag("Goal"))
            {
                // Debug.Log("pos: " + rectTransform.anchoredPosition.x);
                //Left goal
                if (this.rectTransform.anchoredPosition.x < -1)
                {
                    GameManager.Instance.ResetBall();
                }
                //Right goal
                else
                {
                    GameManager.Instance.ResetBall();
                }
            }
        }
    }
}