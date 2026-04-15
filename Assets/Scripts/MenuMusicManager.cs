using UnityEngine;

public class MenuMusicManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private float volume = 0.5f;
    
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        if (menuMusic != null)
        {
            audioSource.clip = menuMusic;
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
        if (audioSource != null && menuMusic != null)
        {
            audioSource.Play();
        }
    }
}
