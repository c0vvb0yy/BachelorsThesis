using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueVariableManager : MonoBehaviour
{
    public InMemoryVariableStorage variableStorage;
    private int _obeliskAmount = 0;

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
        variableStorage.SetValue("$mushroomQuestComplete", state);
    }
    public void UpdateCollectedMushrooms(int amount){
        variableStorage.SetValue("$MushroomsCollected", amount);
    }
    public void UpdateObelisk(string obelisk){
        _obeliskAmount++;
        variableStorage.SetValue("$obelisks", _obeliskAmount);
        string key = "$"+obelisk+"Activated";
        variableStorage.SetValue(key, true);
    }

    public void UpdateDragonStatus(bool state){
        variableStorage.SetValue("$dragonAsleep", state);
    }
    public void UpdatePlayerDeathStatus(bool died){
        variableStorage.SetValue("$PlayerIsDead", died);
    }

    public void UpdatePlayerHealth(int amount){
        variableStorage.SetValue("$PlayerCurrentHealth", amount);
    }
}
