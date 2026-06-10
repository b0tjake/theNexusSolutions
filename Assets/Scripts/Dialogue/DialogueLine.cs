using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    public LocalizedString text;
    public AudioClip audio;
    public float duration = 10f;
    public DialogueEventType eventType;
}