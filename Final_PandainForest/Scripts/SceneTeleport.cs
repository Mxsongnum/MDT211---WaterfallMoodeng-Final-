using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTeleport : MonoBehaviour
{
    public string nextSceneName;        
    public CanvasGroup fadePanel;     
    public float fadeDuration = 1.0f;   
    public bool requireButtonPress = true; 

    private bool isPlayerInRange = false;
    private bool isFading = false;

    void Start()
    {
        if (fadePanel != null)
        {
            fadePanel.alpha = 1f; 
            StartCoroutine(Fade(1f, 0f)); 
        }
    }

    void Update()
    {
        if (isPlayerInRange && requireButtonPress && !isFading)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(FadeAndLoad());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (!requireButtonPress && !isFading)
            {
                StartCoroutine(FadeAndLoad());
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    IEnumerator FadeAndLoad()
    {
        isFading = true;
        yield return StartCoroutine(Fade(0f, 1f));

        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            if (fadePanel != null)
            {
                fadePanel.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            }
            yield return null;
        }
        if (fadePanel != null) fadePanel.alpha = endAlpha;
    }
}