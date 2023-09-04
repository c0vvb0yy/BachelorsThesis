using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class PlayerInteraction : MonoBehaviour
{
    StarterAssetsInputs _input;
    DialogueRunner _dialogueRunner;
    TextLineProvider _lineProvider;
    NPCDialogueManager _dialogueManager;

    bool _inDialogue;
    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(_input.interact && _dialogueManager != null && !_inDialogue){
            StartDialogue();
        }
    }

    void OnTriggerEnter(Collider other){

        if(other.CompareTag("NPC")){
             _dialogueManager = other.gameObject.GetComponentInChildren<NPCDialogueManager>();
        }

    }
    void OnTriggerExit(Collider other) {
        if(other.CompareTag("NPC")){
            _dialogueManager = null;
        }
    }

    public void StartDialogue(){
        _dialogueManager.StartDialogue();
        _input.interact = false;
        _inDialogue = true;
    }

    public void EndDialogue(){
        _dialogueManager.EndDialogue();
        _inDialogue = false;
        _input.interact = false;
    }
}
