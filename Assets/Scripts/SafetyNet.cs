using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class SafetyNet : MonoBehaviour{
    public Transform _safeSpace;

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            other.gameObject.GetComponent<ThirdPersonController>().Teleport(_safeSpace.position);
        }
    }
}
