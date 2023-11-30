using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace ZPong
{
    public class VolumeControl : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer; // Reference to your Audio Mixer.

        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        
        [SerializeField] private TMP_Text masterVolumeText;
        [SerializeField] private TMP_Text musicVolumeText;
        [SerializeField] private TMP_Text sfxVolumeText;
        
        //Default Values
        private const int dMaster = 100;
        private const int dMusic = 100;
        private const int dSfx = 100;

        private void Start()
        {
            // Load the saved volume levels from PlayerPrefs (if previously set).
            LoadVolumeLevels();

            // Add listeners to the sliders to update the audio mixer.
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        private void OnEnable()
        {
            LoadVolumeLevels();
        }

        private void LoadVolumeLevels()
        {
            if (PlayerPrefs.HasKey("MasterVolume"))
            {
                float masterVolume = PlayerPrefs.GetFloat("MasterVolume");
                SetMasterVolume(masterVolume);
            }

            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
                SetMusicVolume(musicVolume);
            }

            if (PlayerPrefs.HasKey("SFXVolume"))
            {
                float sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
                SetSFXVolume(sfxVolume);
            }
        }

        private void SetMasterVolume(float volume)
        {
            float normalizedValue = Mathf.Pow(volume / 100.0f, 0.33f);
            float mappedValue = Mathf.Lerp(-80, 0, normalizedValue);

            audioMixer.SetFloat("Master", Mathf.Log10(Mathf.Max(0.001f, Mathf.Pow(10, mappedValue / 20))) * 20);
            PlayerPrefs.SetFloat("MasterVolume", volume);
            
            masterVolumeSlider.value = volume;
            masterVolumeText.text = volume + "";
        }

        private void SetMusicVolume(float volume)
        {
            float normalizedValue = Mathf.Pow(volume / 100.0f, 0.33f);
            float mappedValue = Mathf.Lerp(-80, 0, normalizedValue);

            audioMixer.SetFloat("Music", Mathf.Log10(Mathf.Max(0.001f, Mathf.Pow(10, mappedValue / 20))) * 20);
            PlayerPrefs.SetFloat("MusicVolume", volume);
            
            musicVolumeSlider.value = volume;
            musicVolumeText.text = volume + "";
        }

        private void SetSFXVolume(float volume)
        {
            float normalizedValue = Mathf.Pow(volume / 100.0f, 0.33f);
            float mappedValue = Mathf.Lerp(-80, 0, normalizedValue);

            audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Max(0.001f, Mathf.Pow(10, mappedValue / 20))) * 20);
            PlayerPrefs.SetFloat("SFXVolume", volume);
            
            sfxVolumeSlider.value = volume;
            sfxVolumeText.text = volume + "";
        }

        [ContextMenu("Reset To Default Settings")]
        public void ResetToDefaultSettings() 
        {
            SetMasterVolume(dMaster);
            SetMusicVolume(dMusic);
            SetSFXVolume(dSfx);
        }
    }
}