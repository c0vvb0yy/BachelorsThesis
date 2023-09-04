using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPCDialogueManager : MonoBehaviour
{
    
    [SerializeField]
    public List<string> StarterNodes = new List<string>();
    
    string _starterNode {get;set;}
    int _index = 0;

    DialogueRunner _dialogueRunner;
    IdleBehaviour _idleBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        _dialogueRunner = GetComponentInChildren<DialogueRunner>();
        _idleBehaviour = GetComponent<IdleBehaviour>();
        _starterNode = StarterNodes[_index];
    }

    public void StartDialogue(){
        _idleBehaviour.StartConversation();
        _dialogueRunner.StartDialogue(_starterNode);
    }

    public void EndDialogue(){
        _idleBehaviour.EndConversation();
        MarkNewStarterNode();
    }

    public void MarkNewStarterNode(){
        _index++;
        if(_index >= StarterNodes.Count)
            return;
        _starterNode = StarterNodes[_index];
    }

}
