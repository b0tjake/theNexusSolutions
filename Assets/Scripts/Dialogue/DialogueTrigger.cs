using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData dialogueData;
    public DoorController targetDoor; // drag the specific door here in Inspector
    bool played = false;

    void OnTriggerEnter(Collider other)
    {
        if (played) return;
        if (!other.CompareTag("Player")) return;
        played = true;
        DialogueManager.Instance.PlayDialogue(dialogueData.lines);

        if (targetDoor != null)
            targetDoor.OpenDoor();
    }
}