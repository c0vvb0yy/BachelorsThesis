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
    }

    public void UpdateSword(bool state){
        variableStorage.SetValue("$hasSword", state);
    }
    public void UpdateFarmQuest(bool state){
        variableStorage.SetValue("$farmQuestComplete", state);
    }
}
