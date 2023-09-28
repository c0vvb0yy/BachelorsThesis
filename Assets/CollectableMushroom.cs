using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableMushroom : CollectableItem
{
    public float spinSpeed;
    private MushroomQuestManager questManager;

    public static event Action OnCollect;

    void Awake()
    {
        LeanTween.reset();
    }

    void Start(){
        questManager = FindQuestManager();
    }
    
    protected virtual void OnEnable(){
        LeanTween.rotateAround(this.gameObject, Vector3.up, 360, spinSpeed).setLoopClamp();
    }

    // Update is called once per frame
    protected override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
        questManager.CollectMushroom();
        OnCollect.Invoke();
    }

    private MushroomQuestManager FindQuestManager(){
        var objects = GameObject.FindGameObjectsWithTag("QuestManager");
        foreach (var manager in objects){
            if(manager.TryGetComponent<MushroomQuestManager>(out MushroomQuestManager component)){
                return component;
            }
        }
        Debug.LogWarning("No Mushroom Quest Manager found in scene");
        return null;
    }
}
