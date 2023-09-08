using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSword : CollectableItem
{
    EquipmentSystem _playerEquipment;
    void Start() {
        _playerEquipment = GameObject.FindWithTag("Player").GetComponent<EquipmentSystem>();
    }
    protected override void OnEnable() {
        base.OnEnable();
        Debug.Log("sword enabled");
    }
    protected override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
        _playerEquipment.getRustySword();
        Debug.Log("Sword Log");
    }
}
