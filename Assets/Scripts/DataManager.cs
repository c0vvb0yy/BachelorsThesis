using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class DataManager : MonoBehaviour{

    private static readonly string SAVE_FOLDER = "/Saves/";
    public GameObject Player;
    public InformationLogger POILogger;
    public Dragon Dragon; //holds the info on the Obelisks
    public EnemyQuestManager FarmQuestManager;
    public MushroomQuestManager MushroomQuestManager;

    public static event Action<SaveData> OnLoad;

    void OnEnable(){
        InformationLogger.OnCollect += Save;
        CollectableMushroom.OnCollect += Save;
        CollectableSword.OnCollect += Save;
    }
    private void OnDisable(){
        InformationLogger.OnCollect -= Save;
        CollectableMushroom.OnCollect -= Save;
        CollectableSword.OnCollect -= Save;
    }
    
    void Start(){
        SaveSystem.Init();
    }

    public void Save(){
        var currWeap = "";
        var dragon_killed = false;
        if(Player.GetComponent<PlayerCombat>().currentWeapon != null){
            currWeap = Player.GetComponent<PlayerCombat>().currentWeapon.name;
        }
        if(!Dragon.gameObject.activeInHierarchy){
            dragon_killed = true;
        }
        SaveData saveData = new(){
            playerPosition = Player.transform.position,
            collectedPOIs = POILogger.collectedPoints,
            activeObelisks = Dragon.activatedObelisks,
            farmQuest_done = FarmQuestManager.questFulfilled,
            mushroomQuest_done = MushroomQuestManager._fulfilled,
            dragon_pacified = Dragon.pacified,
            dragon_killed = dragon_killed,
            collectedMushrooms = MushroomQuestManager.collectedMushrooms,
            currentWeapon = currWeap
        };
        string json = JsonUtility.ToJson(saveData);
        SaveSystem.Save(json);
        Debug.Log("SAVE");
    }

    public void Load(){
        SaveData saveData = SaveSystem.Load();
        if(saveData != null){
            OnLoad.Invoke(saveData);
        }else{
            Debug.LogWarning("no save data");
        }
        
    }

    public void SaveQuit(){
        Save();
        Application.Quit();
    }
}
