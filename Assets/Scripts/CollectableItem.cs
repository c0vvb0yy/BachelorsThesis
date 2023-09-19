using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;

public class CollectableItem : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            SendCollectionData();
            Destroy(gameObject, 0.25f);
        }
    }

    void SendCollectionData(){
        var eventData = new Dictionary<string, object>{
            {"ItemName", this.gameObject.name},
        };
        AnalyticsService.Instance.CustomData("PickedUpItem", eventData);
    }
    
}
