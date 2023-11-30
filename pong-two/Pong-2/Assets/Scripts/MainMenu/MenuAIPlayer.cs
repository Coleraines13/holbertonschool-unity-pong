using System.Collections;
using UnityEngine;

namespace ZPong
{

    public class MenuAIPlayer : MonoBehaviour
    {
        public float speed = 5f;
        private float initialY;
        private float halfPlayerHeight;
        private Paddle thisPaddle;

        private bool letsPlay;
        private float screenTop;
        private float screenBottom;

        private void Start()
        {
            letsPlay = false;
            StartCoroutine(StartDelay());
        }

        private void Update()
        {
            if (letsPlay)
            {
                // Calculate new Y position based on a sine curve
                float newY = Mathf.Sin(Time.time * speed) * (screenTop - halfPlayerHeight) +
                             (screenTop + screenBottom) / 2;

                // Update the paddle's position using the existing Paddle.Move() method
                thisPaddle.Move(newY - thisPaddle.AnchorPos().y);
            }
        }

        IEnumerator StartDelay()
        {
            yield return new WaitForSeconds(3f);

            var playerParent = transform.parent;
            thisPaddle = playerParent.GetComponent<Paddle>();

            // Get bounds and half-height from the Paddle component
            screenTop = thisPaddle.screenTop;
            screenBottom = thisPaddle.screenBottom;
            halfPlayerHeight = thisPaddle.GetHalfHeight();

            // Disabling the Player script if present
            Player playerScript = playerParent.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.enabled = false;
            }

            letsPlay = true;
        }
    }
}