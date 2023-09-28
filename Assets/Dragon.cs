using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using Unity.Services.Analytics;
using UnityEngine.AI;
using System.Linq;
using TMPro;

public class Dragon : MonoBehaviour
{
    ThirdPersonController _player;
    Animator _animator;
    Enemy _health;
    NavMeshAgent _agent;
    float _neededObelisks;
    [HideInInspector] public List<string> activatedObelisks = new();
    
    void OnEnable(){
        Obelisk.OnActivation += RegisterObelisk;
    }

    void OnDisable(){
        Obelisk.OnActivation -= RegisterObelisk;
    }

    void Start(){
        _animator = GetComponent<Animator>();
        _health = GetComponent<Enemy>();
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();
        _neededObelisks = GameObject.FindGameObjectsWithTag("Obelisk").Length;
    }

    // Update is called once per frame
    void Update(){
        
    }

    void RegisterObelisk(string name){
        activatedObelisks.Add(name);
        if(activatedObelisks.Count == _neededObelisks){
            AnalyticsService.Instance.CustomData("DragonPacified");
            Weaken();
        }
    }

    void Weaken(){
        _health.ReduceHealth(100);
        _animator.SetTrigger("Sleep");
        _agent.isStopped = true;
    }
}
