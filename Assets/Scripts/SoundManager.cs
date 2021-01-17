using UnityEngine;

public enum AudioType
{
    glassBreak,
    bump
}

public class SoundManager : MonoBehaviour
{
    public AudioClip glassBreak;
    public AudioClip bump;
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
        switch (audioType)
        {
            case AudioType.glassBreak:
                audioSource.clip = glassBreak;
                break;
            case AudioType.bump:
                audioSource.clip = bump;
                break;
            default:
                break;
        }
        if (PlayerPrefs.GetInt("isSound", 1) == 1)
            audioSource.Play();
    }

}