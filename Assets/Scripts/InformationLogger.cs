using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class InformationLogger : MonoBehaviour
{
    float collectionTime;
    public void LogPointOfInterest(string pointName)
    {
        string path = Application.dataPath + "/test.txt";

        StreamWriter writer = new StreamWriter(path, true);

        string timeData = TimeOfCollection();

        writer.WriteLine("{0} {1}", pointName, timeData);
        writer.Close();

        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }
    
    void OnEnable()
    {
        PointOfInterest.OnCollect_Data += LogPointOfInterest;
    }

    void OnDisable()
    {
        PointOfInterest.OnCollect_Data -= LogPointOfInterest;
    }


    private void Start() {
        
        collectionTime = 0;
    }

    string TimeOfCollection(){
        
        var previousCollectionTime = collectionTime;
        collectionTime = Time.time;
        var timeBetween = collectionTime - previousCollectionTime;
        string text = string.Format("collected at {0}. Time since last collect {1}", collectionTime, timeBetween);
        return text;
    }

}
