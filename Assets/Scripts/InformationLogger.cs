using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Analytics;
using Unity.Services.Analytics;
using Unity.Services.Core;
using System;
public class InformationLogger : MonoBehaviour{
    float collectionTime;
    [HideInInspector] public List<string> collectedPoints = new();
    public static event Action OnCollect;
    void OnEnable(){
        PointOfInterest.OnCollect_Data += LogPointOfInterest;
        DataManager.OnLoad += Deserialize;
    }

    void OnDisable(){
        PointOfInterest.OnCollect_Data -= LogPointOfInterest;
        DataManager.OnLoad -= Deserialize;
    }
    public void LogPointOfInterest(string pointName){
        var eventData = new Dictionary<string, object>{
            {"PointOfInterest", pointName},
            {"TimeBetween", TimeOfCollection()},
            {"CollectionTime", collectionTime},
        };
        AnalyticsService.Instance.CustomData("CollectedPointOfInterest", eventData);
        
        collectedPoints.Add(pointName);
        OnCollect.Invoke();
        /*
        string path = Application.dataPath + "/test.txt";

        StreamWriter writer = new StreamWriter(path, true);


        writer.WriteLine("{0} {1}", pointName, timeData);
        writer.Close();

        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
        */
    }
    
    async void Start(){
        collectionTime = 0;
        try
        {
            await UnityServices.InitializeAsync();
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        }
        catch (ConsentCheckException e)
        {
          print(e.Reason);// Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
        }
    }

    float TimeOfCollection(){
        var previousCollectionTime = collectionTime;
        collectionTime = Time.time;
        return collectionTime - previousCollectionTime;
    }

    //the list of string hols all points that have been collected
    //so we go over all points in our scene and see if they're uncollected i.e. active 
    //and if they've previously been uncollected in the scene they're deactivated
    public void Deserialize(SaveData saveData){
        List<string> inactivePOI = saveData.collectedPOIs;
        foreach (var obj in GetComponentsInChildren<Transform>()){
            if(obj.gameObject.activeInHierarchy){
                if(obj.gameObject.TryGetComponent<PointOfInterest>(out PointOfInterest poi)){
                    foreach (var collectedPOI in inactivePOI){
                        if(poi.name == collectedPOI){
                            obj.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

    }

}
