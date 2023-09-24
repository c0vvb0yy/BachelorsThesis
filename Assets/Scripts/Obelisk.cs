using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;

public class Obelisk : MonoBehaviour
{
    bool _activated;
    public float spinSpeed;
    public float floatSpeed;
    public GameObject tinyPillars;
    public GameObject mainPillar;
    public static event Action OnActivation;

    public void Activate(){
        if(!_activated){
            OnActivation.Invoke();
            var eventData = new Dictionary<string, object>{
                {"Obelisk", gameObject.name}
            };
            AnalyticsService.Instance.CustomData("Obelisk", eventData);
        
            Debug.Log("obelisk: " + gameObject.name + " activated");
            StartTweens();
            _activated = true;
        }
    }


    void StartTweens(){
        LeanTween.rotateAround(tinyPillars, Vector3.up, 360, spinSpeed);
        LeanTween.rotateAround(mainPillar, Vector3.up, 360, spinSpeed);
        LeanTween.moveLocalY(mainPillar, 1.5f, floatSpeed).setEaseInOutSine().setLoopPingPong();
    }
}
