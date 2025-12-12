using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NPCDialogue : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueLine
    {
        [TextArea(3, 5)] public string sentence; 
        public string name; 
    }

    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Image portraitUI;
    public GameObject pressEPrompt; 
    public Sprite npcIcon;
    public DialogueLine[] dialogueLines;
    private int index;
    private bool isPlayerInRange;
    private bool isTalking;

    void Start()
    {
        if (dialogueBox != null) dialogueBox.SetActive(false);
        if (pressEPrompt != null) pressEPrompt.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && !isTalking && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }

        else if (isTalking && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)))
        {
            NextLine();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (pressEPrompt != null) pressEPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (pressEPrompt != null) pressEPrompt.SetActive(false);
            EndDialogue();
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        index = 0;

        if (dialogueBox != null) dialogueBox.SetActive(true);
        if (pressEPrompt != null) pressEPrompt.SetActive(false);
        if (portraitUI != null && npcIcon != null)
        {
            portraitUI.gameObject.SetActive(true);
            portraitUI.sprite = npcIcon;
        }

        ShowLine();
    }

    void ShowLine()
    {
        if (index < dialogueLines.Length)
        {
            if (dialogueText != null) dialogueText.text = dialogueLines[index].sentence;
            if (nameText != null) nameText.text = dialogueLines[index].name;
        }
    }

    void NextLine()
    {
        index++;
        if (index < dialogueLines.Length)
        {
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isTalking = false;
        if (dialogueBox != null) dialogueBox.SetActive(false);
        if (isPlayerInRange && pressEPrompt != null)
        {
            pressEPrompt.SetActive(true);
        }
    }
}