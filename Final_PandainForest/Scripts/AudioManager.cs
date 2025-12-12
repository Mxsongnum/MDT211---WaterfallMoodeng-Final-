using UnityEngine;

public class AudioSound : MonoBehaviour
{
    public static AudioSound Instance;

    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip newClip)
    {
        if (musicSource.clip == newClip) return;
        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();
    }
}