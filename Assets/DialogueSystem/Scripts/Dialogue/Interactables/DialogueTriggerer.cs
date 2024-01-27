using UnityEngine;

/// <summary>
/// This class allows dialogue to be called when the player moves into a trigger in the world
/// </summary>
public class DialogueTriggerer : MonoBehaviour
{
    #region Class Variables
    [Header("Trigger Information")]
    [Tooltip("Whether or not this dialogue trigger should only happen once")]
    public bool m_PlayOnce = false;
    [Tooltip("Whether or not this trigger has already played its dialogue")]
    public bool m_AlreadyPlayed = false;
    [Tooltip("Whether or not to set this gameobject to inactive after playing the dialogue")]
    public bool m_SetToInactiveAfterPlay = false;

    [Header("Script References")]
    [Tooltip("The Dialogue Holder whose dialogue will be played back once the player enters the trigger")]
    public DialogueHolder m_DialogueHolder;
    #endregion

    #region Methods
    #region Unity Methods
    /// <summary>
    /// Unity Method that is called when another collider collides with this object's trigger
    /// </summary>
    /// <param name="other">The other collider that interacts with this trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (m_PlayOnce == true && m_AlreadyPlayed == true)
            {
                return;
            }
            else
            {
                m_AlreadyPlayed = true;
                m_DialogueHolder.TriggerDialogue();
            }

            if(m_SetToInactiveAfterPlay)
            {
                gameObject.SetActive(false);
            }
        }
    }
    #endregion
    #endregion
}
