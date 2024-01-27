using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Class that manages most of the dialogue actions
/// </summary>
public class DialogueManager : MonoBehaviour
{
    #region Class Variables
    /// <summary>
    /// TextMeshPro object that is the name of the active m_Speaker
    /// </summary>
    [Header("Dialogue Box Settings")]
    [Tooltip("TextMeshPro that has the name of the speaker")]
    public TextMeshProUGUI m_NameText;

    /// <summary>
    /// TextMeshPro object that is the actual dialogue text being spoken
    /// </summary>
    [Tooltip("TextMeshPro that has the dialogue that is spoken")]
    public TextMeshProUGUI m_SentenceDisplay;

    /// <summary>
    /// Queue that holds all of the dialogue sentences and pops them off when executing
    /// </summary>
    private Queue<DialogueSentence> m_CurrentDialogueStrings;

    /// <summary>
    /// Instance of the dialogue manager
    /// </summary>
    public static DialogueManager m_Instance;


    /// <summary>
    /// Reference to the dialogue animator
    /// </summary>
    public Animator m_DialogueAnimator;

    /// <summary>
    /// Reference to the version of a left sprite
    /// </summary>
    [Header("Character Representation Settings")]
    public DialogueSprite m_LeftSprite;

    /// <summary>
    /// Reference to the version of a right sprite
    /// </summary>
    public DialogueSprite m_RightSprite;

    /// <summary>
    /// Task that is handling the running text display
    /// </summary>
    Task m_RunningTextDisplayTask;

    /// <summary>
    /// Current dialogue sentence that is being displayed
    /// </summary>
    DialogueSentence m_CurrentDialogueString;

    // left side is 0 right side is 1
    public int m_ActiveSpeaker = 0;

    public float m_DialougeSpeed = 20f;

    /// <summary>
    /// Boolean that determines if the dialogue is currently running
    /// </summary>
    [Header("Dialogue State Information")]
#pragma warning disable CS0414
    public bool dialogueRunning = false;
#pragma warning restore CS0414

    /// <summary>
    /// Index of the current dialogue 
    /// </summary>
    int dialogueIndex = 0;

    /// <summary>
    /// Boolean saying if the dialogue is running or not
    /// </summary>
    bool listRunning = false;

    /// <summary>
    /// Current list of dialogue
    /// </summary>
    List<Dialogue> currentList;
    #endregion
    #region Methods
    #region Unity Methods

    /// <summary>
    /// Unity Method called upon scene start before Start
    /// </summary>
    void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        m_CurrentDialogueStrings = new Queue<DialogueSentence>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    #endregion

    #region Custom Methods

    /// <summary>
    /// Method that is called when a scene is loaded
    /// </summary>
    /// <param name="scene">Scene object</param>
    /// <param name="mode">LoadScene mode</param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EndDialogueWithoutUnPausing();
    }

    /// <summary>
    /// Method that takes a dialogue object and starts it
    /// </summary>
    /// <param name="dialogue">Dialogue object to start</param>
    public void StartDialogue(Dialogue dialogue)
    {
        m_DialogueAnimator.SetBool("IsOpen", true);

        m_NameText.text = dialogue.m_Speaker.m_SpeakerName;

        m_CurrentDialogueStrings.Clear();

        SetAndAnimateSprites(dialogue);

        foreach (DialogueSentence dialogueString in dialogue.m_DialogueSentences)
        {
            m_CurrentDialogueStrings.Enqueue(dialogueString);
        }
        dialogueRunning = true;

        DisplayNextSentence();
    }

    /// <summary>
    /// Sets the sprite on the left or right to be the dialogues sprite, removing the inactive m_Speaker and setting the other m_Speaker to inactive
    /// while setting the current dialogue to be the active m_Speaker
    /// </summary>
    /// <param name="dialogue">Dialogue object to set sprites and animate</param>
    void SetAndAnimateSprites(Dialogue dialogue)
    {
        // See if the name already matches
        if (m_LeftSprite.m_CurrentCharacterName == dialogue.m_Speaker.m_SpeakerName)
        {
            m_LeftSprite.MakeActiveSprite(dialogue);
            foreach(DialogueSentence dialogueString in dialogue.m_DialogueSentences)
            {
                dialogueString.dialgoueSpritePosition = Speaker.Position.Left;
            }
            m_RightSprite.MakeInactiveSprite();
        }
        else if (m_RightSprite.m_CurrentCharacterName == dialogue.m_Speaker.m_SpeakerName)
        {
            m_RightSprite.MakeActiveSprite(dialogue);
            foreach (DialogueSentence dialogueString in dialogue.m_DialogueSentences)
            {
                dialogueString.dialgoueSpritePosition = Speaker.Position.Right;
            }
            m_LeftSprite.MakeInactiveSprite();
        }
        // If none of the names already match then go by whichever is not active
        else if (m_LeftSprite.CheckIfActive())
        {
            m_RightSprite.MakeActiveSprite(dialogue);
            foreach (DialogueSentence dialogueString in dialogue.m_DialogueSentences)
            {
                dialogueString.dialgoueSpritePosition = Speaker.Position.Right;
            }
            m_LeftSprite.MakeInactiveSprite();
        }
        else if (m_RightSprite.CheckIfActive())
        {
            m_LeftSprite.MakeActiveSprite(dialogue);
            foreach (DialogueSentence dialogueString in dialogue.m_DialogueSentences)
            {
                dialogueString.dialgoueSpritePosition = Speaker.Position.Left;
            }
            m_RightSprite.MakeInactiveSprite();
        }
        // If neither is active, default to their preferred position
        else if (dialogue.m_Speaker.m_PreferredPosition == Speaker.Position.Right)
        {
            m_RightSprite.MakeActiveSprite(dialogue);
            foreach (DialogueSentence dialogueString in dialogue.m_DialogueSentences)
            {
                dialogueString.dialgoueSpritePosition = Speaker.Position.Right;
            }
            m_LeftSprite.MakeInactiveSprite();
        }
        else
        {
            m_LeftSprite.MakeActiveSprite(dialogue);
            foreach (DialogueSentence dialogueString in dialogue.m_DialogueSentences)
            {
                dialogueString.dialgoueSpritePosition = Speaker.Position.Left;
            }
            m_RightSprite.MakeInactiveSprite();
        }

    }

    /// <summary>
    /// Override that takes a list of dialogue and starts the dialogue in that list
    /// </summary>
    /// <param name="dialogueList">List of dialogue objects to start</param>
    public void StartDialogue(List<Dialogue> dialogueList)
    {
        dialogueIndex = 0;
        listRunning = true;
        currentList = dialogueList;
        StartDialogue(currentList[0]);
    }

    /// <summary>
    /// Coroutine that types out the dialogue sentences
    /// </summary>
    /// <param name="sentence">Sentence to type as a string</param>
    /// <returns></returns>
    IEnumerator TypeSentence (string sentence)
    {
        m_SentenceDisplay.text = "";
        m_CurrentDialogueString.DoWhenStartingDialogueString();
        foreach (char letter in sentence.ToCharArray())
        {
            m_SentenceDisplay.text += letter;
            yield return new WaitForSeconds(1/m_DialougeSpeed);
        }
    }

    /// <summary>
    /// Function that displays the next sentence
    /// </summary>
    public void DisplayNextSentence()
    {
        if (m_RunningTextDisplayTask != null && m_RunningTextDisplayTask.Running)
        {
            m_SentenceDisplay.text = m_CurrentDialogueString.sentence;
            m_RunningTextDisplayTask.Stop();
            return;
        }

        if (m_CurrentDialogueStrings.Count <= 0 && listRunning == false)
        {
            EndDialogue();
            return;
        }
        else if (m_CurrentDialogueStrings.Count <= 0 && listRunning == true)
        {
            dialogueIndex += 1;
            if (dialogueIndex >= currentList.Count)
            {
                EndDialogue();
                return;
            }
            else
            {
                m_DialogueAnimator.SetBool("IsOpen", false);
                StartDialogue(currentList[dialogueIndex]);
                return;
            }
        }

        if (m_CurrentDialogueString != null)
        {
            m_CurrentDialogueString.DoWhenEndingDialogueString();
        }
        m_CurrentDialogueString = m_CurrentDialogueStrings.Dequeue();
        if (m_RunningTextDisplayTask != null)
        {
            m_RunningTextDisplayTask.Stop();
        }
        m_RunningTextDisplayTask = new Task(TypeSentence(m_CurrentDialogueString.sentence));

    }

    /// <summary>
    /// Function that ends the dialogue that is playing
    /// </summary>
    public void EndDialogue()
    {
        m_DialogueAnimator.SetBool("IsOpen", false);
        m_LeftSprite.ResetSprite();
        m_RightSprite.ResetSprite();
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        dialogueRunning = false;
        if (m_CurrentDialogueString != null)
        {
            m_CurrentDialogueString.DoWhenEndingDialogueString();
        }
        m_CurrentDialogueString = null;
    }

    /// <summary>
    /// Function that ends the dialogue without changing the time scale of the game. 
    /// </summary>
    public void EndDialogueWithoutUnPausing()
    {
        if (m_DialogueAnimator != null)
        {
            m_DialogueAnimator.SetBool("IsOpen", false);
            m_LeftSprite.ResetSprite();
            m_RightSprite.ResetSprite();
        }
        if (currentList != null)
        {
            currentList.Clear();
        }
        m_CurrentDialogueString = null;
    }
    #endregion
    #endregion
}
