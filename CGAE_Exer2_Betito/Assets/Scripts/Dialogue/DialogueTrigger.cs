using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTES
//This code requires asstes in game to function properly.
//This code also requires Ink to function and the Ink add-on in Unity.
//For context, it helps with dialogue.
//To download Ink, go to https://www.inklestudios.com/ink/

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    private void Awake(){
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update(){
        if(playerInRange && !DialogueManager.GetInstance().dialogueisPlaying){
            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)){
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
        } else {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player"){
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if (collider.gameObject.tag == "Player"){
            playerInRange = false;
        }
    }
}
