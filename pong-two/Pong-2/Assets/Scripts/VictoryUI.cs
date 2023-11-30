using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZPong
{

    public class VictoryUI : MonoBehaviour
    {
        public void Quit()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}