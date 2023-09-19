using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSword : MonoBehaviour
{
    public float spinSpeed;
    EquipmentSystem _playerEquipment;
    private void Awake() {
        LeanTween.reset();
    }

    void Start() {
        _playerEquipment = GameObject.FindWithTag("Player").GetComponent<EquipmentSystem>();
    }
    void OnEnable(){
        LeanTween.rotateAround(this.gameObject, Vector3.up, 360, spinSpeed).setLoopClamp();
    }
    private void OnTriggerEnter(Collider other) {
        //base.OnTriggerEnter(other);
        _playerEquipment.getRustySword();
        Debug.Log("Sword Log");
    }
}
