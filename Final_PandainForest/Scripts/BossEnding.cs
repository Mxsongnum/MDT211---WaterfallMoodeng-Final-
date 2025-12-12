using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BossEnding : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup; 
    public string nextSceneName = "Scene2"; 
    public float waitBeforeFade = 2.0f;
    bool isDead = false;

    void Update()
    {

        if (isDead)
        {
            BossDie();
        }
    }

    public void BossDie()
    {
        isDead = true;
        StartCoroutine(EndGameSequence());
    }

    IEnumerator EndGameSequence()
    {
        yield return new WaitForSeconds(waitBeforeFade);

        float timer = 0f;
        while (timer < 1.5f)
        {
            timer += Time.deltaTime;
            if (fadeCanvasGroup != null)
            { 
                fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / 1.5f);
            }
            yield return null;
        }
        
        if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 1f;
        SceneManager.LoadScene(nextSceneName);
    }
}