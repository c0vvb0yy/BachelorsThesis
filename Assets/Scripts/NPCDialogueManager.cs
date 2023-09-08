using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Unity.Services.Analytics;
using UnityEngine.UI;

public class NPCDialogueManager : MonoBehaviour
{
    
    [SerializeField]
    public List<string> StarterNodes = new List<string>();
    
    string _starterNode {get;set;}
    int _index = 0;

    List<string> _states = new(){
        "Talking 0", "Talking 1", "Talking 2" 
    };

    int _animatorState;

    public bool hasQuest;

    DialogueRunner _dialogueRunner;
    IdleBehaviour _idleBehaviour;
    NPCInteractUI _interactUI;
    Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _dialogueRunner = GetComponentInChildren<DialogueRunner>();
        _idleBehaviour = GetComponent<IdleBehaviour>();
        _interactUI = GetComponentInChildren<NPCInteractUI>();
        _animator = GetComponent<Animator>();
        _starterNode = StarterNodes[_index];
        SetUpCanvas();
    }

    public void SetUpCanvas(){
        if(hasQuest){
            _interactUI.QuestAvailable();
        } else {
            _interactUI.CleanUp();
        }
    }

    public void StartDialogue(){
        _idleBehaviour.StartConversation();
        _dialogueRunner.StartDialogue(_starterNode);
        //_animator.SetTrigger("EnterDialogue");
        
        PlayRandomTalkAnimation();
        
        
        var eventData = new Dictionary<string, object>{
            {"NPCName", this.gameObject.name},
        };
        AnalyticsService.Instance.CustomData("EngagedNPCDialogue", eventData);
    }

    public void StartRandomTalkAnimation(){
        int randomIndex = Random.Range(0, _states.Count);
        _animatorState = randomIndex;
        _animator.Play(_states[randomIndex]);
    }

    //function used by the animator to determine the next talk animation and play it in a smooth way
    public void PlayRandomTalkAnimation(){
        int randomIndex = Random.Range(0, _states.Count);
        if(_animatorState == randomIndex)
            return;
        _animatorState = randomIndex;
        _animator.SetTrigger(_states[randomIndex]);
    }

    public void EndDialogue(){
        _idleBehaviour.EndConversation();
        _animator.SetTrigger("ExitDialogue");
        ResetTriggers();
        MarkNewStarterNode();
    }

    void ResetTriggers(){
        foreach (var state in _states){
            _animator.ResetTrigger(state);
        }
    }

    public void MarkNewStarterNode(){
        _index++;
        if(_index >= StarterNodes.Count)
            return;
        _starterNode = StarterNodes[_index];
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            _interactUI.ShowInteractable();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            SetUpCanvas();
        }
    }

    [YarnCommand("acceptQuest")]
    public void AcceptQuest(){
        hasQuest = false;
    }
}
