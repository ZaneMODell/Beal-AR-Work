using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that holds all of the dialogue objects for a certain dialogue/text interaction
/// </summary>
public class DialogueHolder : MonoBehaviour
{
    #region Class Variables
    /// <summary>
    /// List of dialogue objects that will be executed
    /// </summary>
    [Header("Dialogue Settings")]
    [Tooltip("The dialogues to play back")]
    public List<Dialogue> m_DialogueToPlayBack;


    /// <summary>
    /// Boolean that determines if the game pauses when dialogue occurs
    /// </summary>
    [Header("Play Settings")]
    [Tooltip("Whether or not to pause when it occurs")]
    public bool m_PauseOnTrigger = true;

    /// <summary>
    /// Boolean that determines whether or not this dialogue plays on scene start
    /// </summary>
    [Tooltip("Whether or not to play this dialogue when the script starts")]
    public bool m_PlayOnStart = false;
    #endregion

    #region Methods
    #region Unity Methods
    /// <summary>
    /// Unity method that is called on scene start
    /// </summary>
    private void Start()
    {
        if (m_PlayOnStart)
        {
            TriggerDialogue();
        }
    }
    #endregion

    #region Custom Methods

    /// <summary>
    /// Triggers the dialogue and gets it playing
    /// </summary>
    public void TriggerDialogue()
    {
        SetUpDialogue();
    }

    /// <summary>
    /// Method that cancels any dialogue from the dialogue manager
    /// </summary>
    public void CancelDialogue()
    {
        DialogueManager.m_Instance.EndDialogue();
    }

    /// <summary>
    /// Sets up the dialogue and starts it, will pause the game if pause on trigger is set to true.
    /// </summary>
    void SetUpDialogue()
    {
        DialogueManager.m_Instance.StartDialogue(m_DialogueToPlayBack);

        if (m_PauseOnTrigger)
        {
            Time.timeScale = 0;
        }
    }
    #endregion
    #endregion
}
