using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;

public class EnemyQuestManager : MonoBehaviour
{
    public bool questFulfilled = false;
    public bool questReady = false;

    public DialogueVariableManager variableStorage;
    
    private void OnEnable() {
        DataManager.OnLoad += Deserialize;
    }

    private void OnDisable() {
        DataManager.OnLoad -= Deserialize;
    }

    void Start(){
        variableStorage.variableStorage.SetValue("$questReady", questReady);
        variableStorage = GameObject.FindWithTag("DVS").GetComponent<DialogueVariableManager>();
        
    }

    void Update()
    {
        if(!questFulfilled && this.transform.childCount <= 0){
            FinishQuest();
        }
    }

    public void FinishQuest(){
        variableStorage.variableStorage.TryGetValue("$questReady", out questReady);
        questFulfilled = true;
        variableStorage.UpdateFarmQuest(questFulfilled);
        Debug.Log("Quest Success!");
        SendEventData();
    }
    public void DebugFinishQuest(bool fullfilled){
        variableStorage.variableStorage.TryGetValue("$questReady", out questReady);
        questFulfilled = fullfilled;
        variableStorage.UpdateFarmQuest(questFulfilled);
    }
    void SendEventData(){
        var eventData = new Dictionary<string, object>{
            {"AcceptedQuestBeforehand", questReady}
        };
        AnalyticsService.Instance.CustomData("FarmQuest", eventData);
    }
    
    void Deserialize(SaveData saveData){
        DebugFinishQuest(saveData.farmQuest_done);
    }
}
