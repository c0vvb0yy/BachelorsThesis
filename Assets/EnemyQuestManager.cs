using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class EnemyQuestManager : MonoBehaviour
{
    public bool questFulfilled = false;

    public DialogueVariableManager variableStorage;
    
    void Update()
    {
        if(!questFulfilled && this.transform.childCount <= 0){
            questFulfilled = true;
            variableStorage.UpdateFarmQuest(questFulfilled);
            Debug.Log("Quest Success!");
        }
    }

    
}
