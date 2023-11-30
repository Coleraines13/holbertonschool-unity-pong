using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZPong
{

    [RequireComponent(typeof(Paddle))]
    public class Player : MonoBehaviour
    {

        private bool isLeftPaddle;

        public KeyCode upKey;
        public KeyCode downKey;

        public float speed = 5f;

        private Paddle thisPaddle;

        private void Start()
        {
            thisPaddle = GetComponent<Paddle>();

            isLeftPaddle = thisPaddle.isLeftPaddle;
            
            // Retrieve player-specific input keys from PlayerPrefs.
            string upKeyPref = "Player" + (isLeftPaddle ? "One" : "Two") + "UpInput";
            string downKeyPref = "Player" + (isLeftPaddle ? "One" : "Two") + "DownInput";

            if (PlayerPrefs.HasKey(upKeyPref) && PlayerPrefs.HasKey(downKeyPref))
            {
                upKey = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(upKeyPref));
                downKey = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(downKeyPref));
            }
            
            if (PlayerPrefs.HasKey("PaddleSpeed"))
            {
                speed = PlayerPrefs.GetFloat("PaddleSpeed");
            }
        }

        private void Update()
        {
            float verticalInput = 0;
            if (Input.GetKey(upKey))
            {
                verticalInput = 1;
            }
            else if (Input.GetKey(downKey))
            {
                verticalInput = -1;
            }

            thisPaddle.Move(verticalInput * speed * Time.deltaTime);
        }
    }
}