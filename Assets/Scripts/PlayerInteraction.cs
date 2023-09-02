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

    bool _inDialogue;
    string _starterNode;
    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(_input.interact && _dialogueRunner != null && !_inDialogue){
            _dialogueRunner.StartDialogue(_starterNode);
            _input.interact = false;
            _inDialogue = true;
            Debug.Log(_dialogueRunner.CurrentNodeName);
            Debug.Log(_lineProvider.LinesAvailable);
        }
    }

    void OnTriggerEnter(Collider other){

        if(other.CompareTag("NPC")){
            _dialogueRunner = other.gameObject.GetComponentInChildren<DialogueRunner>();
            _lineProvider = other.gameObject.GetComponentInChildren<TextLineProvider>();
            _starterNode = other.gameObject.GetComponentInChildren<StarterDialogueNode>().StarterNode;
        }

    }
}
