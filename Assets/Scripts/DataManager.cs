using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
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
        CollectableSword.OnCollect -= Save;
    }
    private void OnDisable() {
        InformationLogger.OnCollect -= Save;
        CollectableMushroom.OnCollect -= Save;
        CollectableSword.OnCollect -= Save;
    }
    // Start is called before the first frame update
    void Awake()
    {
        SaveSystem.Init();
    }

    public void Save(){
        var currWeap = "";
        if(Player.GetComponent<PlayerCombat>().currentWeapon != null){
            currWeap = Player.GetComponent<PlayerCombat>().currentWeapon.name;
        }
        SaveData saveData = new(){
            playerPosition = Player.transform.position,
            collectedPOIs = POILogger.collectedPoints,
            activeObelisks = Dragon.activatedObelisks,
            farmQuest_done = FarmQuestManager.questFulfilled,
            mushroomQuest_done = MushroomQuestManager._fulfilled,
            collectedMushrooms = MushroomQuestManager.collectedMushrooms,
            currentWeapon = currWeap
        };
        string json = JsonUtility.ToJson(saveData);
        SaveSystem.Save(json);
    }

    public void Load(){
        SaveData saveData = SaveSystem.Load();
        if(saveData != null){
            OnLoad.Invoke(saveData);
        }
        
    }
}
