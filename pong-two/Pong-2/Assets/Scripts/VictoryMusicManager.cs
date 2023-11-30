using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VictoryMusicManager : MonoBehaviour
{
    public AudioClip victoryFanfare; // Sound for victory fanfare
    public AudioClip fireworksShow;  // Sound for fireworks show

    private AudioSource audioSource;
    private BackgroundMusic backgroundMusic;

    public static VictoryMusicManager Instance;

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

        audioSource = GetComponent<AudioSource>();
        backgroundMusic = BackgroundMusic.Instance;
    }

    public void PlayVictoryMusic()
    {
        // Turn off the background music
        backgroundMusic.StopBackgroundMusic();

        // Play the victory fanfare
        audioSource.PlayOneShot(victoryFanfare);

        // Delay playing the fireworks show after the victory fanfare
        float fanfareDuration = victoryFanfare.length;
        Invoke(nameof(PlayFireworksShow), fanfareDuration);
    }

    private void PlayFireworksShow()
    {
        // Play the fireworks show
        audioSource.clip = fireworksShow;
        audioSource.Play();

        // Delay resuming the background music after the fireworks show
        float fireworksDuration = fireworksShow.length;
        Invoke(nameof(ResumeBackgroundMusic), fireworksDuration);
    }

    private void ResumeBackgroundMusic()
    {
        // Tell the background music manager to start playing again
        backgroundMusic.ResumeBackgroundMusic();
    }
}