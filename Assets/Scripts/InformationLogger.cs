using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Analytics;
using Unity.Services.Analytics;
using Unity.Services.Core;
public class InformationLogger : MonoBehaviour{
    float collectionTime;
    void OnEnable(){
        PointOfInterest.OnCollect_Data += LogPointOfInterest;
    }

    void OnDisable(){
        PointOfInterest.OnCollect_Data -= LogPointOfInterest;
    }
    public void LogPointOfInterest(string pointName){
        var eventData = new Dictionary<string, object>{
            {"PointOfInterest", pointName},
            {"TimeBetween", TimeOfCollection()},
            {"CollectionTime", collectionTime},
        };
        AnalyticsService.Instance.CustomData("CollectedPointOfInterest", eventData);
        
        
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

}
