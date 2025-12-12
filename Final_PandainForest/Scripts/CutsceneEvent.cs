using UnityEngine;
using TMPro;
using System.Collections;

public class CutsceneEventFinal : MonoBehaviour
{
    public Animator npcAnimator;
    public GameObject bossObject;
    public GameObject dialogBox;
    public TextMeshProUGUI textComponent;
    [TextArea(3, 10)]
    public string[] sentences;
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1.5f;   
    public float waitBeforeFade = 2.0f; 
    private int index = 0;
    private bool isTalking = false;
    private bool eventFinished = false;

    void Start()
    {
        if (bossObject != null) bossObject.SetActive(false);
        if (dialogBox != null) dialogBox.SetActive(false);
        if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !eventFinished)
        {
            if (!isTalking) StartDialogue();
            else NextSentence();
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        dialogBox.SetActive(true);
        index = 0;
        if (sentences.Length > 0) textComponent.text = sentences[index];
    }

    void NextSentence()
    {
        index++;
        if (index < sentences.Length) textComponent.text = sentences[index];
        else EndDialogueAndStartSequence();
    }

    void EndDialogueAndStartSequence()
    {
        isTalking = false;
        eventFinished = true;
        dialogBox.SetActive(false);

        StartCoroutine(PlayDramaticSequence());
    }

  
    IEnumerator PlayDramaticSequence()
    {
   
        if (npcAnimator != null) npcAnimator.SetTrigger("Die");

     
        yield return new WaitForSeconds(waitBeforeFade);
        yield return StartCoroutine(Fade(0f, 1f));


        if (bossObject != null) bossObject.SetActive(true);

        yield return new WaitForSeconds(1.0f);

 
        yield return StartCoroutine(Fade(1f, 0f));

    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timeElapsed / fadeDuration);
            }
            yield return null;
        }
        if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = endAlpha;
    }
}