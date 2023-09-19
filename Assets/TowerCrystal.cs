using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCrystal : MonoBehaviour
{
    [SerializeField] float _spinSpeed = 1f;
    [SerializeField] float _floatSpeed = 2f;

    void Awake(){
        LeanTween.reset();
    }
    void OnEnable(){
        LeanTween.rotateAround(this.gameObject, Vector3.up, 360, _spinSpeed).setLoopClamp();
        LeanTween.moveLocalY(this.gameObject, 0.1f, _floatSpeed).setEaseInOutSine().setLoopPingPong();
    }
}
