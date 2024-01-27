using UnityEngine;

/// <summary>
/// This class contains the m_Speaker who is speaking for a dialogue as well as an array of dialogue sentences
/// which are spoken by the m_Speaker when the dialogue is played
/// </summary>
[System.Serializable]
public class Dialogue
{
    #region Class Variables
    /// <summary>
    /// Speaker object for this dialogue
    /// </summary>
    [Tooltip("The character which is speaking this dialogue")]
    public Speaker m_Speaker;

    /// <summary>
    /// Array of dialogue sentences
    /// </summary>
    [Tooltip("The dialogue strings that are being spoken by the m_Speaker")]
    public DialogueSentence[] m_DialogueSentences;
    #endregion
}
