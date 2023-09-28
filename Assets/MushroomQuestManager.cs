using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MushroomQuestManager : MonoBehaviour
{
    DialogueVariableManager variableStorage;
    MushroomQuestUI ui;
    [HideInInspector] public int collectedMushrooms = 0;

    public bool _fulfilled; 
    
    private void OnEnable() {
        DataManager.OnLoad += Deserialize;
    }

    private void OnDisable() {
        DataManager.OnLoad -= Deserialize;
    }


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
            collectedMushrooms = 999;
            variableStorage.UpdateCollectedMushrooms(collectedMushrooms);
            variableStorage.UpdateMushroomQuest(true);
            _fulfilled = !_fulfilled;
        }else{
            collectedMushrooms = 0;
            variableStorage.UpdateCollectedMushrooms(collectedMushrooms);
            variableStorage.UpdateMushroomQuest(false);
            _fulfilled = !_fulfilled;
        }
    }

    public void CollectMushroom(){
        collectedMushrooms++;
        UpdateText();
    }

    public void UpdateText(){
        ui.UpdateText(collectedMushrooms);
        variableStorage.UpdateCollectedMushrooms(collectedMushrooms);
    }

    public void Deserialize(SaveData saveData){
        collectedMushrooms = saveData.collectedMushrooms;
        UpdateText();
        if(saveData.mushroomQuest_done){
            FinishQuest();
        }
    }
}
