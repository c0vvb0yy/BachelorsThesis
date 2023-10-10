using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;

public class EnemyQuestManager : MonoBehaviour
{
    public bool questFulfilled = false;
    public bool questReady = false;

    public DialogueVariableManager variableStorage;

    public Animator FarmerAnimator;
    
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
        GameObject.Find("DataManager").GetComponent<DataManager>().Save();
        FarmerAnimator.SetTrigger("QuestDone");
        Debug.Log("Quest Success!");
        SendEventData();
    }
    public void DebugFinishQuest(bool fullfilled){
        variableStorage.variableStorage.TryGetValue("$questReady", out questReady);
        questFulfilled = fullfilled;
        variableStorage.UpdateFarmQuest(questFulfilled);
        FarmerAnimator.SetTrigger("QuestDone");
    }
    void SendEventData(){
        var eventData = new Dictionary<string, object>{
            {"AcceptedQuestBeforehand", questReady}
        };
        AnalyticsService.Instance.CustomData("FarmQuest", eventData);
    }
    
    void Deserialize(SaveData saveData){
        if(saveData.farmQuest_done){
            GameObject.Find("Smith DIALOGUE BEARER").GetComponent<NPCDialogueManager>().AcceptQuest();
            DebugFinishQuest(true);
            foreach (var enemy in GetComponentsInChildren<Transform>()){
                Destroy(enemy.gameObject);
            }
        }
    }
}
