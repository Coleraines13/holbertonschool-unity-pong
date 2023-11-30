using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZPong
{

    [RequireComponent(typeof(Collider2D))]
    public class Paddle : MonoBehaviour
    {
        public bool isLeftPaddle = true;

        private float halfPlayerHeight;
        public float screenTop { get; private set; }
        public float screenBottom { get; private set; }
        
        private RectTransform rectTransform;


        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();


            if (PlayerPrefs.HasKey("PaddleSize"))
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, PlayerPrefs.GetFloat("PaddleSize"));
                this.GetComponent<BoxCollider2D>().size = rectTransform.sizeDelta;
            }

            halfPlayerHeight = rectTransform.sizeDelta.y / 2f;

            var height = UIScaler.Instance.GetUIHeight();

            screenTop = height / 2;
            screenBottom = -1 * height / 2;
        }

        public void Move(float movement)
        {
            //Set temporary variable
            Vector2 newPosition = rectTransform.anchoredPosition;

            //Manipulate the temporary variable
            newPosition.y += movement;
            newPosition.y = Mathf.Clamp(newPosition.y, screenBottom + halfPlayerHeight, screenTop - halfPlayerHeight);

            //Apply temporary variable back to original component
            rectTransform.anchoredPosition = newPosition;
        }

        public float GetHalfHeight()
        {
            return halfPlayerHeight;
        }

        public Vector2 AnchorPos()
        {
            return rectTransform.anchoredPosition;
        }

        // public void Reflect(Ball ball)
        // {
        //     float y = BallHitPaddleWhere(ball.GetPosition(), rectTransform.anchoredPosition, rectTransform.sizeDelta.y / 2f);
        //     //Debug.Log("X: " + bounceDirection + " Y: " + y);
        //     ball.Reflect(new Vector2(bounceDirection, y));
        // }


    }
}