using System;
using System.Collections;
using UnityEngine;
using TMPro;

public enum DialogueEventType
{
    None,
    OpenDoor,
    CloseDoor,
    LockDoor
}

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public TextMeshProUGUI dialogueText;

    [Header("Audio")]
    public AudioSource audioSource;

    public Action<DialogueEventType> OnDialogueEvent;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayDialogue(DialogueLine[] lines)
    {
        StopAllCoroutines();
        StartCoroutine(RunLines(lines));
    }

    private IEnumerator RunLines(DialogueLine[] lines)
    {
        foreach (DialogueLine line in lines)
        {
            yield return StartCoroutine(HandleLine(line));
        }

        ClearDialogue();
    }

    private IEnumerator HandleLine(DialogueLine line)
    {
        // 📝 Localization
        var op = line.text.GetLocalizedStringAsync();
        yield return op;

        string localizedText = op.Result;

        // 🗣️ UI text
        dialogueText.text = string.IsNullOrEmpty(line.speakerName)
            ? localizedText
            : $"{line.speakerName}: {localizedText}";

        // 🚪 EVENT (clean, single call)
        if (line.eventType != DialogueEventType.None)
        {
            Debug.Log("Triggering Event: " + line.eventType);
            OnDialogueEvent?.Invoke(line.eventType);
        }

        // 🔊 AUDIO or ⏱ WAIT
        if (line.audio != null)
        {
            audioSource.clip = line.audio;
            audioSource.Play();

            yield return new WaitWhile(() => audioSource.isPlaying);
        }
        else
        {
            yield return new WaitForSeconds(line.duration);
        }
    }

    private void ClearDialogue()
    {
        dialogueText.text = "";
    }
}