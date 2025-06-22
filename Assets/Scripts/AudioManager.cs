using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Clips")]
    public AudioClip backgroundClip;
    public AudioClip LoseCLip;
    public AudioClip WinClip;
    public AudioClip moveClip;

    [Header("Audio Sources")]
    private AudioSource backgroundSource;
    private AudioSource effectSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Cấu hình Audio Sources
        backgroundSource = gameObject.AddComponent<AudioSource>();
        effectSource = gameObject.AddComponent<AudioSource>();

        // Cài đặt background
        if (backgroundClip != null)
        {
            backgroundSource.clip = backgroundClip;
            backgroundSource.loop = true;
            backgroundSource.Play();
        }
    }

    public void Lose()
    {
        if (LoseCLip != null && effectSource != null)
        {
            effectSource.PlayOneShot(LoseCLip);
        }
    }

    public void Win()
    {
        if (WinClip != null && effectSource != null)
        {
            effectSource.PlayOneShot(WinClip);
        }
    }

    public void PlayMove()
    {
        if (moveClip != null && effectSource != null)
        {
            effectSource.PlayOneShot(moveClip);
        }
    }

}