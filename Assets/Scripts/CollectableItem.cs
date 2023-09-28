using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;

public class CollectableItem : MonoBehaviour
{
    public float spinSpeed;
    protected virtual void Awake() {
        LeanTween.reset();
    }
    protected virtual void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            SendCollectionData();
            Destroy(gameObject, 0.25f);
        }
    }

    protected virtual void Start(){
        LeanTween.rotateAround(this.gameObject, Vector3.up, 360, spinSpeed).setLoopClamp();
    }

    void SendCollectionData(){
        var eventData = new Dictionary<string, object>{
            {"ItemName", this.gameObject.name},
        };
        AnalyticsService.Instance.CustomData("PickedUpItem", eventData);
    }
    
}
