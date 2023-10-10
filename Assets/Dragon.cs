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
    DialogueVariableManager _variableStorage;
    float _neededObelisks;
    [HideInInspector] public List<string> activatedObelisks = new();

    public bool pacified;
    public DragonAttack dragonAttack;

    public static event Action DragonSleep;
    public static event Action DragonDeath;

    void OnEnable(){
        Obelisk.OnActivation += RegisterObelisk;
        Enemy.OnDeath += OnDeath;
        DataManager.OnLoad += Deserialize;
    }

    void OnDisable(){
        Obelisk.OnActivation -= RegisterObelisk;
        Enemy.OnDeath -= OnDeath;
        DataManager.OnLoad -= Deserialize;
    }

    void Start(){
        _animator = GetComponent<Animator>();
        _health = GetComponent<Enemy>();
        _agent = GetComponent<NavMeshAgent>();
        _variableStorage = GameObject.FindWithTag("DVS").GetComponent<DialogueVariableManager>();
        
        _player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();
        _neededObelisks = GameObject.FindGameObjectsWithTag("Obelisk").Length;
    }

    void RegisterObelisk(string name){
        activatedObelisks.Add(name);
        if(activatedObelisks.Count == _neededObelisks && !pacified){
            AnalyticsService.Instance.CustomData("DragonPacified");
            Weaken();
        }
    }

    void Weaken(){
        pacified = true;
        dragonAttack.isActive = false;
        _health.ReduceHealth(100);
        _animator.SetTrigger("Sleep");
        _variableStorage.UpdateDragonStatus(pacified);
        DragonSleep.Invoke();
        _agent.isStopped = true;
        GetComponent<Enemy>().dragon = true;
    }
    void OnDeath(GameObject dragon){
        if(dragon == this.gameObject){
            DragonDeath.Invoke();
        }
    }

    void Deserialize(SaveData saveData){
        if(saveData.dragon_killed){
            DragonDeath.Invoke();
            Destroy(this.gameObject);
            return;
        }
        if(saveData.dragon_pacified){
            Weaken();
        }
    }
}
