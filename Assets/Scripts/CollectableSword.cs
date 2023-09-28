using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSword : CollectableItem
{
    EquipmentSystem _playerEquipment;
    public static event Action OnCollect;

    protected override void Awake(){
        base.Awake();
    }

    protected override void Start() {
        base.Start();
        _playerEquipment = GameObject.FindWithTag("Player").GetComponent<EquipmentSystem>();
        //LeanTween.rotateAround(this.gameObject, Vector3.up, 360, spinSpeed).setLoopClamp();
    }

    protected override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
        _playerEquipment.getRustySword();
        OnCollect.Invoke();
    }
}
