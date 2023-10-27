using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class PlayerInteraction : MonoBehaviour
{
    StarterAssetsInputs _input;
    NPCDialogueManager _dialogueManager;
    ThirdPersonController _controls;

    bool _inDialogue;

    private void Awake() {
    
        _input = GetComponent<StarterAssetsInputs>();
        _controls = GetComponent<ThirdPersonController>();
        
    }
    // Start is called before the first frame update
    void Start(){
        _inDialogue = true;
        StartDialogue(GameObject.Find("Witch").GetComponent<NPCDialogueManager>());
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

    public void StartDialogue(NPCDialogueManager dialogueManager = null){
        if(_dialogueManager == null && dialogueManager != null){
            _dialogueManager = dialogueManager;
        }
        Cursor.lockState = CursorLockMode.Confined;
        _dialogueManager.StartDialogue();
        _input.interact = false;
        _inDialogue = true;
        _controls.RestrainMovement();
    }

    public void EndDialogue(){
        Cursor.lockState = CursorLockMode.Locked;
        _dialogueManager.EndDialogue();
        _inDialogue = false;
        _input.interact = false;
        _controls.UnleashMovement();
    }
}
