using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator animator;

    private bool isOpen = false;
    private bool isLocked = false;

void OnEnable()
{
    DialogueManager.Instance.OnDialogueEvent += HandleDialogueEvent;
}

    void OnDisable()
    {
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueEvent -= HandleDialogueEvent;
    }

    void HandleDialogueEvent(DialogueEventType type)
    {
        switch (type)
        {
            case DialogueEventType.OpenDoor:
                Open();
                break;

            case DialogueEventType.CloseDoor:
                Close();
                break;

            case DialogueEventType.LockDoor:
                Lock();
                break;
        }
    }

    public void Open()
    {
        if (isLocked || isOpen) return;

        isOpen = true;

        if (animator != null)
            animator.SetTrigger("Open");
    }

    public void Close()
    {
        if (!isOpen) return;

        isOpen = false;

        if (animator != null)
            animator.SetTrigger("Close");
    }

    public void Lock()
    {
        isLocked = true;
    }

    public void Unlock()
    {
        isLocked = false;
    }
}