using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;

public class Obelisk : MonoBehaviour
{
    bool _activated;
    
    public static event Action OnActivation;

    public void Activate(){
        if(!_activated){
            Debug.Log("activating....");
            OnActivation?.Invoke();
            var eventData = new Dictionary<string, object>{
                {"Obelisk", gameObject.name}
            };
            AnalyticsService.Instance.CustomData("Obelisk", eventData);
        
            Debug.Log("obelisk: " + gameObject.name + " activated");
            _activated = true;
        }
    }
}
