using UnityEngine;

public enum AudioType
{
    glassBreak,
    bump,
    levelCleared,
    levelFailed
}

public class SoundManager : MonoBehaviour
{
    public AudioClip glassBreak;
    public AudioClip bump;
    public AudioClip levelCleared;
    public AudioClip levelFailed;
    public AudioSource audioSource;
    public AudioSource backgroundAudioSource;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        if (PlayerPrefs.GetInt("isMusic", 1) == 1)
            backgroundAudioSource.Play();
    }

    public void playAudio(AudioType audioType)
    {
        if (audioSource.clip == levelCleared)
            if (audioSource.isPlaying)
                return;
        switch (audioType)
        {
            case AudioType.glassBreak:
                audioSource.clip = glassBreak;
                break;
            case AudioType.bump:
                audioSource.clip = bump;
                break;
            case AudioType.levelCleared:
                audioSource.clip = levelCleared;
                break;
            case AudioType.levelFailed:
                audioSource.clip = levelFailed;
                break;
            default:
                break;
        }
        if (PlayerPrefs.GetInt("isSound", 1) == 1)
            audioSource.Play();
    }

}