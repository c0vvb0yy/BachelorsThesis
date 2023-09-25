using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MushroomQuestManager : MonoBehaviour
{
    DialogueVariableManager variableStorage;
    MushroomQuestUI ui;
    int _collectedMushrooms = 0;

    //just for debug purposes
    public bool _fulfilled; 

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

    public void DebugFinishQuest(bool finish){
        if(finish){
            _collectedMushrooms = 999;
            variableStorage.UpdateCollectedMushrooms(_collectedMushrooms);
            variableStorage.UpdateMushroomQuest(true);
            _fulfilled = !_fulfilled;
        }else{
            _collectedMushrooms = 0;
            variableStorage.UpdateCollectedMushrooms(_collectedMushrooms);
            variableStorage.UpdateMushroomQuest(false);
            _fulfilled = !_fulfilled;
        }
    }

    public void CollectMushroom(){
        _collectedMushrooms++;
        ui.UpdateText(_collectedMushrooms);
        variableStorage.UpdateCollectedMushrooms(_collectedMushrooms);
    }
}
