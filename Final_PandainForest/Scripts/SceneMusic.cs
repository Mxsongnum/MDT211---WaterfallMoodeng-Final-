using UnityEngine;

public class SceneSound : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AudioClip bgmForThisScene; 

    private void Start()
    {
        if (AudioSound.Instance != null)
        {
            AudioSound.Instance.PlayMusic(bgmForThisScene);
        }
       
    }
}