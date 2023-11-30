using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance;

    [SerializeField] private AudioMixer audioMixer; // Reference to your Audio Mixer.

    public AudioClip[] musicTracks;  // List of audio clips for background music
    private AudioSource audioSource;
    private int currentTrackIndex;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Ensure this object persists across scenes
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        // Pick a random starting index
        currentTrackIndex = Random.Range(0, musicTracks.Length);

        // Play the first track
        PlayNextTrack();
        
        // Subscribe to the scene loaded event to change tracks when a new scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        
        //Tune volume at the beginning
        float normalizedValue = Mathf.Pow(PlayerPrefs.GetFloat("MasterVolume") / 100.0f, 0.33f);
        float mappedValue = Mathf.Lerp(-80, 0, normalizedValue);

        audioMixer.SetFloat("Master", Mathf.Log10(Mathf.Max(0.001f, Mathf.Pow(10, mappedValue / 20))) * 20);
            
        normalizedValue = Mathf.Pow(PlayerPrefs.GetFloat("MusicVolume") / 100.0f, 0.33f);
        mappedValue = Mathf.Lerp(-80, 0, normalizedValue);
        
        audioMixer.SetFloat("Music", Mathf.Log10(Mathf.Max(0.001f, Mathf.Pow(10, mappedValue / 20))) * 20);

        normalizedValue = Mathf.Pow(PlayerPrefs.GetFloat("SFXVolume") / 100.0f, 0.33f);
        mappedValue = Mathf.Lerp(-80, 0, normalizedValue);
        
        audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Max(0.001f, Mathf.Pow(10, mappedValue / 20))) * 20);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Play the next track when a new scene is loaded
        PlayNextTrack();
    }

    private void PlayNextTrack()
    {
        if (musicTracks.Length == 0)
        {
            Debug.LogError("No music tracks defined in the inspector.");
            return;
        }

        // Ensure the index is within bounds
        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;

        // Set the audio clip and play it
        audioSource.clip = musicTracks[currentTrackIndex];
        audioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        currentTrackIndex = 0;
        audioSource.Stop();
    }

    public void ResumeBackgroundMusic()
    {
        PlayNextTrack();
    }
}