using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public string dialogueFileName = "dialogue_1"; // Set this in Inspector

    [Header("UI Hint")]
    public GameObject interactionHint; // Assign a UI Text GameObject like "Press F to Talk"

    private bool isPlayerInRange = false;
    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (dialogueManager != null)
            {
                if (!dialogueManager.IsDialogueActive)
                {
                    dialogueManager.StartDialogue(dialogueFileName);
                    if (interactionHint != null)
                        interactionHint.SetActive(false);
                }
                else
                {
                    dialogueManager.OnInteract();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactionHint != null && !dialogueManager.IsDialogueActive)
                interactionHint.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionHint != null)
                interactionHint.SetActive(false);

            if (dialogueManager != null)
                dialogueManager.ForceEndDialogue();
        }
    }
}
