using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class contatins the sentence to be spoken and also contains functions and settings for when the specific dialogue stirng is read out by the dialogue manager
/// </summary>
[System.Serializable]
public class DialogueSentence
{
    /// <summary>
    /// Sentence that is contained within this dialogue sentence
    /// </summary>
    [Tooltip("Sentence spoken in this dialogue")]
    public string sentence;

    /// <summary>
    /// Boolean that states whether or not the dialogue ends after this sentence
    /// </summary>
    [Tooltip("Boolean stating whether or not we leave this dialogue after this sentence")]
    public bool leaveAtEndOfSentence = false;

    [HideInInspector]
    public Speaker.Position dialgoueSpritePosition;

    /// <summary>
    /// Event that may be called before this dialogue string is started
    /// </summary>
    [Tooltip("Events that may be called upon the dialogue string beginning")]
    public UnityEvent eventsWhenStartingDialogueString;

    /// <summary>
    /// Event that may be called after this dialogue string is finished
    /// </summary>
    [Tooltip("Events that may be called upon the dialogue string ending")]
    public UnityEvent eventsWhenFinishingDialogueString;

    /// <summary>
    /// Function that handles any events that happen before the dialogue string
    /// </summary>
    public void DoWhenStartingDialogueString()
    {
        if (eventsWhenStartingDialogueString != null)
        eventsWhenStartingDialogueString.Invoke();
    }

    /// <summary>
    /// Function that handles any events that happen after the dialogue string
    /// </summary>
    public void DoWhenEndingDialogueString()
    {
        if (leaveAtEndOfSentence)
        {
            if (dialgoueSpritePosition == Speaker.Position.Left)
            {
                DialogueManager.m_Instance.m_LeftSprite.ResetSprite();
            }
            else if (dialgoueSpritePosition == Speaker.Position.Right)
            {
                DialogueManager.m_Instance.m_RightSprite.ResetSprite();
            }
        }
        if (eventsWhenFinishingDialogueString != null)
        eventsWhenFinishingDialogueString.Invoke();
    }
}
