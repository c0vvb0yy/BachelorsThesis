using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MushroomQuestManager : MonoBehaviour
{
    DialogueVariableManager variableStorage;
    // Start is called before the first frame update
    void Start(){
        variableStorage = GameObject.FindWithTag("DVS").GetComponent<DialogueVariableManager>();
    }

    [YarnCommand("finishQuest")]
    public void FinishQuest(){
        variableStorage.UpdateMushroomQuest(true);
    }

    public void CollectMushroom(){
        variableStorage.AddMushroom();
    }
}
