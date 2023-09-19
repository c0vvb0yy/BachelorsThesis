using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueVariableManager : MonoBehaviour
{
    public InMemoryVariableStorage variableStorage;

    // Start is called before the first frame update
    void Start()
    {
        variableStorage = GetComponent<InMemoryVariableStorage>();
        variableStorage.SetValue("$hasSword", false);
        variableStorage.SetValue("$MushroomsCollected", 0);
    }

    public void UpdateSword(bool state){
        variableStorage.SetValue("$hasSword", state);
    }
    public void UpdateFarmQuest(bool state){
        variableStorage.SetValue("$farmQuestComplete", state);
    }
    public void UpdateMushroomQuest(bool state){
        variableStorage.SetValue("$MushroomQuestComplete", state);
    }
    public void UpdateCollectedMushrooms(int amount){
        variableStorage.SetValue("$MushroomsCollected", amount);
    }
}
