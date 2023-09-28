using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

    public static void Init(){
        if(!Directory.Exists(SAVE_FOLDER)){
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        if(File.Exists(SAVE_FOLDER + "/save.txt")){
            Load();
        }
    }

    public static void Save(string data){
        File.WriteAllText(SAVE_FOLDER + "/save.txt", data);
    }

    public static SaveData Load(){
        if(File.Exists(SAVE_FOLDER + "/save.txt")){
            string saveString = File.ReadAllText(SAVE_FOLDER + "/save.txt");

            return JsonUtility.FromJson<SaveData>(saveString);

        } else {
            Debug.LogWarning("No Save File to load exists");
            return null;
        }
    }
}
