using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using UnityEngine.VFX;
using Unity.VisualScripting;

public class Obelisk : MonoBehaviour
{
    bool _activated;
    public float spinSpeed;
    public float floatSpeed;
    public GameObject tinyPillars;
    public GameObject mainPillar;
    private Beam _beam;
    public static event Action OnActivation;

    private void Start() {
        _beam = GetComponentInChildren<Beam>();
    }

    public void Activate(){
        if(!_activated){
            Debug.Log("obelisk: " + gameObject.name + " activated");
            OnActivation.Invoke();
            SendData();
            StartTweens();
            InitializeBeam();
            _activated = true;
        }
    }

    void SendData(){
        var eventData = new Dictionary<string, object>{
            {"Obelisk", gameObject.name}
        };
        AnalyticsService.Instance.CustomData("Obelisk", eventData);
    }

    void StartTweens(){
        LeanTween.rotateAround(tinyPillars, Vector3.up, 360, spinSpeed);
        LeanTween.rotateAround(mainPillar, Vector3.up, 360, spinSpeed);
        LeanTween.moveLocalY(mainPillar, 1.5f, floatSpeed).setEaseInOutSine().setLoopPingPong();
    }

    void InitializeBeam(){
        _beam.Go();
    }

}
