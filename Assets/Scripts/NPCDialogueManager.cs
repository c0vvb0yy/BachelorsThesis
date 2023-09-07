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

    public bool hasQuest;

    DialogueRunner _dialogueRunner;
    IdleBehaviour _idleBehaviour;
    NPCInteractUI _interactUI;

    // Start is called before the first frame update
    void Start()
    {
        _dialogueRunner = GetComponentInChildren<DialogueRunner>();
        _idleBehaviour = GetComponent<IdleBehaviour>();
        _interactUI = GetComponentInChildren<NPCInteractUI>();
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
        var eventData = new Dictionary<string, object>{
            {"NPCName", this.gameObject.name},
        };
        AnalyticsService.Instance.CustomData("EngagedNPCDialogue", eventData);
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
