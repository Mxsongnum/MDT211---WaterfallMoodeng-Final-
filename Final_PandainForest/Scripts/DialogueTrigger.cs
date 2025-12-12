using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueTrigger : MonoBehaviour
{
    [System.Serializable]
    public struct TutorialStep
    {
        [TextArea(3, 5)] public string sentence;
        public KeyCode requiredKey;
        public KeyCode alternativeKey;
        public string promptMessage;
        public Sprite buttonSprite;
    }

    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    public Image portraitUI;
    public GameObject promptBubbleRoot;
    public TMP_Text promptText;
    public Image promptImage;
    public Sprite npcIcon;
    public TutorialStep[] tutorialSteps;
    public GameObject questIcon;
    private int index;
    private bool isRunning; 

    void Start()
    {
        if (dialogueBox != null) dialogueBox.SetActive(false);
        if (questIcon != null) questIcon.SetActive(true);
        if (promptBubbleRoot != null) promptBubbleRoot.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isRunning)
        {
            StartDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
        }
    }

    void StartDialogue()
    {
        isRunning = true; 
        index = 0;
        dialogueBox.SetActive(true);
        if (questIcon != null) questIcon.SetActive(false);

        if (portraitUI != null && npcIcon != null)
        {
            portraitUI.gameObject.SetActive(true);
            portraitUI.sprite = npcIcon;
        }

        StopAllCoroutines();
        StartCoroutine(TutorialRoutine());
    }

    IEnumerator TutorialRoutine()
    {
        while (index < tutorialSteps.Length)
        {
            TutorialStep currentStep = tutorialSteps[index];

            if (dialogueText != null) dialogueText.text = currentStep.sentence;
            UpdatePromptUI(currentStep);

            yield return null;
            yield return new WaitUntil(() => CheckInput(currentStep));
            if (currentStep.requiredKey != KeyCode.None)
                yield return new WaitForSeconds(1.0f);
            else
                yield return new WaitForSeconds(0.2f);

            index++;
        }

        EndDialogue();
    }

    bool CheckInput(TutorialStep step)
    {
        if (step.requiredKey != KeyCode.None)
        {
            bool mainPressed = Input.GetKeyDown(step.requiredKey);
            bool altPressed = (step.alternativeKey != KeyCode.None) && Input.GetKeyDown(step.alternativeKey);
            return mainPressed || altPressed;
        }
        else
        {
            return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return);
        }
    }

    void UpdatePromptUI(TutorialStep step)
    {
        if (promptBubbleRoot == null) return;
        bool hasPrompt = !string.IsNullOrEmpty(step.promptMessage) || step.buttonSprite != null;
        promptBubbleRoot.SetActive(hasPrompt);

        if (hasPrompt)
        {
            if (promptText != null) promptText.text = step.promptMessage;
            if (promptImage != null)
            {
                promptImage.gameObject.SetActive(step.buttonSprite != null);
                promptImage.sprite = step.buttonSprite;
                if (step.buttonSprite != null) promptImage.SetNativeSize();
            }
        }
    }

    void EndDialogue()
    {
        isRunning = false; 
        if (dialogueBox != null) dialogueBox.SetActive(false);
        if (promptBubbleRoot != null) promptBubbleRoot.SetActive(false);
    }
}