using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    public LocalizedString text;
    public AudioClip audio;
    public float duration = 2f;

    [Header("Event")]
    public DialogueEventType eventType;
    public string targetDoorID; // must match DoorController.doorID exactly
}