using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    [SerializedDictionary("Plant Prefab Name", "Map Link")]
    public SerializedDictionary<string, string> mapLinks;

    public string currentMapLink;


    [SerializedDictionary("Plant Prefab Name", "Informative Text")]
    public SerializedDictionary<string, List<string>> plantTextPairs;


    public DialogueHolder dialogueHolder;

    private Dialogue dialogue;


    private Speaker speaker;
    // Start is called before the first frame update
    void Start()
    {
        dialogue = dialogueHolder.dialogueToPlayBack[0];
        speaker = dialogue.speaker;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMapLink(string plantName)
    {
        if (mapLinks.ContainsKey(plantName))
        {
            currentMapLink = mapLinks[plantName];
        }
    }

    public void UpdatePlantDialogue(string plantName)
    {
        if (plantTextPairs.ContainsKey(plantName))
        {
            List<string> dialogueStrings = plantTextPairs[plantName];
            dialogue.dialogueSentences = new DialogueSentence[dialogueStrings.Count];
            
            for (int i = 0; i < dialogue.dialogueSentences.Length; i++)
            {
                DialogueSentence currentDialogueSentence = new()
                {
                    sentence = dialogueStrings[i]
                };
                dialogue.dialogueSentences[i] = currentDialogueSentence; 
            }
        }
        else
        {
            dialogueHolder.dialogueToPlayBack.Clear();
        }
    }
}
