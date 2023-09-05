using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Unity.Services.Analytics;
using UnityEngine.Analytics;

public class EnemyQuestManager : MonoBehaviour
{
    public bool questFulfilled = false;
    public bool questReady = false;

    public DialogueVariableManager variableStorage;
    
    void Start(){
        variableStorage.variableStorage.SetValue("$questReady", questReady);
        variableStorage = GameObject.FindWithTag("DVS").GetComponent<DialogueVariableManager>();
        
    }

    void Update()
    {
        if(!questFulfilled && this.transform.childCount <= 0){
            variableStorage.variableStorage.TryGetValue("$questReady", out questReady);
            questFulfilled = true;
            variableStorage.UpdateFarmQuest(questFulfilled);
            Debug.Log("Quest Success!");
            SendEventData();
        }
    }

    void SendEventData(){
        Debug.Log("Quest vorher angenommen: "+questReady);
        var eventData = new Dictionary<string, object>{
            {"AcceptedQuestBeforehand", questReady}
        };
        AnalyticsService.Instance.CustomData("FarmQuest", eventData);
    }
    
}
