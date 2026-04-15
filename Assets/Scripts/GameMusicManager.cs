using UnityEngine;

public class GameMusicManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private float volume = 0.5f;
    
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        if (gameMusic != null)
        {
            audioSource.clip = gameMusic;
            audioSource.volume = volume;
            audioSource.loop = true;
            audioSource.playOnAwake = true;
            audioSource.Play();
        }
    }
    
    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
    
    public void PlayMusic()
    {
        if (audioSource != null && gameMusic != null)
        {
            audioSource.Play();
        }
    }
    
    public void PauseMusic()
    {
        if (audioSource != null)
        {
            audioSource.Pause();
        }
    }
    
    public void UnPauseMusic()
    {
        if (audioSource != null)
        {
            audioSource.UnPause();
        }
    }
}
