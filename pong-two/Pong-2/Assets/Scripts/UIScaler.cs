using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZPong
{

    public class UIScaler : MonoBehaviour
    {
        public static UIScaler Instance { get; private set; }

        public float HeightPadding;

        private RectTransform rectTransform;

        private float windowHeight;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            windowHeight = rectTransform.rect.height;

            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            //Debug.Log("Height: " + windowHeight);

            //Double padding here
            HeightPadding *= 2;
        }

        public float GetUIHeight()
        {
            return windowHeight;
        }

        public float GetUIHeightPadded()
        {
            return windowHeight - HeightPadding;
        }
    }
}