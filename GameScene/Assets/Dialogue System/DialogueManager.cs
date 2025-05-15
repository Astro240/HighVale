using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;

    private DialogueData dialogueData;
    private int currentLineIndex = 0;
    public bool IsDialogueActive = false;

    // Call this to start a dialogue file
    public void StartDialogue(string dialogueFileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(dialogueFileName);

        if (jsonFile != null)
        {
            dialogueData = JsonUtility.FromJson<DialogueData>(jsonFile.text);
            currentLineIndex = 0;
            IsDialogueActive = true;
            dialoguePanel.SetActive(true);
            ShowNextLine();  // Show first line immediately when starting
        }
        else
        {
            Debug.LogError($"Dialogue file '{dialogueFileName}.json' not found in Resources folder.");
        }
    }

    // Call this each time player presses F to progress dialogue or start it if not active
    public void OnInteract()
    {
        if (!IsDialogueActive)
        {
            Debug.LogWarning("Dialogue not started. Make sure to call StartDialogue first.");
            return;
        }

        if (currentLineIndex < dialogueData.lines.Count)
        {
            ShowNextLine();
        }
        else
        {
            EndDialogue();
        }
    }

    private void ShowNextLine()
    {
        DialogueLine line = dialogueData.lines[currentLineIndex];
        characterNameText.text = line.characterName;
        dialogueText.text = line.dialogueText;
        currentLineIndex++;
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        IsDialogueActive = false;
        currentLineIndex = 0;
    }

    public void ForceEndDialogue()
    {
        EndDialogue();
    }
}
