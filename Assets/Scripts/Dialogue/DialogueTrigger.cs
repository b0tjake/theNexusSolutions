using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData dialogueData;
    bool played = false;

    void OnTriggerEnter(Collider other)
    {
        if (played) return;
        if (!other.CompareTag("Player")) return;
        played = true;
        DialogueManager.Instance.PlayDialogue(dialogueData.lines);
    }
}