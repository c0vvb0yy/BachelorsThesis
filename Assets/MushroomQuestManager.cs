using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MushroomQuestManager : MonoBehaviour
{
    DialogueVariableManager variableStorage;
    MushroomQuestUI ui;
    int _collectedMushrooms = 0;
    // Start is called before the first frame update
    void Start(){
        variableStorage = GameObject.FindWithTag("DVS").GetComponent<DialogueVariableManager>();
        ui = GameObject.Find("MushroomCollection").GetComponent<MushroomQuestUI>();
    }

    [YarnCommand("initUI")]
    public void InitUI(){
        ui.Appear();
    }

    [YarnCommand("finishQuest")]
    public void FinishQuest(){
        variableStorage.UpdateMushroomQuest(true);
        ui.SendOff();
    }

    public void CollectMushroom(){
        _collectedMushrooms++;
        ui.UpdateText(_collectedMushrooms);
        variableStorage.UpdateCollectedMushrooms(_collectedMushrooms);
    }
}
