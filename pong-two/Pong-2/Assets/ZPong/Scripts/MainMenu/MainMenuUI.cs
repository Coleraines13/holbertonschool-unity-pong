using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ZPong
{

    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button singlePlayerButton;
        [SerializeField] private Button multiPlayerButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;

        [SerializeField] private RectTransform settingsUIParent;

        private void Start()
        {
            singlePlayerButton.onClick.AddListener(LoadSingle);
            singlePlayerButton.onClick.AddListener(LoadSingle);
            multiPlayerButton.onClick.AddListener(LoadMulti);
            settingsButton.onClick.AddListener(LoadSettings);
            quitButton.onClick.AddListener(Quit);
        }

        public void LoadSingle()
        {
            LoadScene("Singleplayer");
        }

        public void LoadMulti()
        {
            LoadScene("Multiplayer");
        }

        public void LoadSettings()
        {
            settingsUIParent.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }

        public void Quit()
        {
            Application.Quit();
        }

        void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}