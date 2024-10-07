using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Story currentStory;

    private static DialogueManager instance;

    public bool dialogueisPlaying { get; private set; }

    private void Awake(){
        if(instance != null){
            Debug.LogWarning("Found more than one Dialogue Manage in the scene.");
        }
        instance = this;
    }

    public static DialogueManager GetInstance(){
        return instance;
    }

    private void Start(){
        dialogueisPlaying = false;
        dialoguePanel.SetActive(false);
    }

    private void Update(){
        if(!dialogueisPlaying){
            return;
        }

        if(Input.GetKeyDown(KeyCode.E)){
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON){
        currentStory = new Story(inkJSON.text);
        dialogueisPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ExitDialogueMode(){
        dialogueisPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory(){
        if(currentStory.canContinue) {
            dialogueText.text = currentStory.Continue();
        } else {
            ExitDialogueMode();
        }
    }
}