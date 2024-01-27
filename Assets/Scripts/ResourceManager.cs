using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// Class that manages the resources such as dialogue and external links
/// </summary>
public class ResourceManager : MonoBehaviour
{
    #region Class Variables
    /// <summary>
    /// Dictionary that holds the names of the plant prefabs and a link to their regional map
    /// </summary>
    [Tooltip("Dictionary that contains a mapping of prefab names and map links")]
    [SerializedDictionary("Plant Prefab Name", "Map Link")]
    public SerializedDictionary<string, string> mapLinks;

    /// <summary>
    /// String that is current link of the map button
    /// </summary>
    [ReadOnly]
    [Tooltip("Current link used by the map button")]
    public string currentMapLink;

    /// <summary>
    /// Dictionary that holds the names of the plant prefabs and a list of strings used for their text
    /// </summary>
    [Tooltip("Dictionary that contains a mapping of prefab names and dialogue text")]
    [SerializedDictionary("Plant Prefab Name", "Informative Text")]
    public SerializedDictionary<string, List<string>> plantTextPairs;

    /// <summary>
    /// Dialogue holder reference
    /// </summary>
    [Tooltip("Reference to the DialogueHolder")]
    public DialogueHolder dialogueHolder;

    /// <summary>
    /// Dialogue held within the dialogue holder
    /// </summary>
    private Dialogue dialogue;

    #endregion

    #region Methods
    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        dialogue = dialogueHolder.m_DialogueToPlayBack[0];
    }
    #endregion

    /// <summary>
    /// Function that updates the map link used by the map button
    /// </summary>
    /// <param name="plantName">Name of the plant prefab</param>
    public void UpdateMapLink(string plantName)
    {
        if (mapLinks.ContainsKey(plantName))
        {
            currentMapLink = mapLinks[plantName];
        }
    }

    /// <summary>
    /// Updates the dialogue from the dictionary
    /// </summary>
    /// <param name="plantName">Name of the plant prefab</param>
    public void UpdatePlantDialogue(string plantName)
    {
        //If the dictionary contains the name of the plant, we copy the text from the dictionary into the dialogue
        if (plantTextPairs.ContainsKey(plantName))
        {
            List<string> dialogueStrings = plantTextPairs[plantName];
            dialogue.m_DialogueSentences = new DialogueSentence[dialogueStrings.Count];
            
            for (int i = 0; i < dialogue.m_DialogueSentences.Length; i++)
            {
                DialogueSentence currentDialogueSentence = new()
                {
                    sentence = dialogueStrings[i]
                };
                dialogue.m_DialogueSentences[i] = currentDialogueSentence; 
            }
        }
        else
        {
            dialogueHolder.m_DialogueToPlayBack.Clear();
        }
    }
    #endregion
}
