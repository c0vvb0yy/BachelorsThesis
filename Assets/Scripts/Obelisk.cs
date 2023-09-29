using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using UnityEngine.VFX;
using Unity.VisualScripting;

[RequireComponent(typeof(AudioSource))]
public class Obelisk : MonoBehaviour
{
    bool _activated;
    public float spinSpeed;
    public float floatSpeed;
    public GameObject tinyPillars;
    public GameObject mainPillar;
    public AudioClip StoneRotate;
    public AudioClip PowerUp;
    public AudioClip ActiveSound;
    
    private Beam _beam;
    private AudioSource _audio;
    public static event Action<string> OnActivation;

    private void OnEnable() {
        DataManager.OnLoad += Deserialize;
    }

    private void OnDisable() {
        DataManager.OnLoad -= Deserialize;
    }

    private void Start() {
        _beam = GetComponentInChildren<Beam>();
        _audio = GetComponent<AudioSource>();
    }

    

    public void Activate(){
        if(!_activated){
            Debug.Log("obelisk: " + gameObject.name + " activated");
            OnActivation.Invoke(gameObject.name);
            SendData();
            StartTweens();
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
        _audio.clip = StoneRotate;
        _audio.loop = true;
        _audio.Play();
        LeanTween.rotateAround(tinyPillars, Vector3.up, 180, spinSpeed).setOnComplete(InitializeBeam);
        LeanTween.rotateAround(mainPillar, Vector3.up, 180, spinSpeed);
        LeanTween.moveLocalY(mainPillar, 1.5f, floatSpeed).setEaseInOutSine().setLoopPingPong();
    }

    void InitializeBeam(){
        StartCoroutine(Sounds());
        _beam.Go();
    }

    IEnumerator Sounds(){
        _audio.loop = false;
        _audio.clip = PowerUp;
        _audio.Play();
        while(_audio.isPlaying){
            yield return null;
        }
        _audio.clip = ActiveSound;
        _audio.loop = true;
        _audio.Play();
    }

    public void Deserialize(SaveData saveData){
        foreach (var obelisk in saveData.activeObelisks){
            if(obelisk == gameObject.name)
                Activate();
        }
    }
}
