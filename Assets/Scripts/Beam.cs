using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Beam : MonoBehaviour
{
    VisualEffect _vfx;
    public Transform _dragon;
    bool _isActive;
    // Start is called before the first frame update
    void Start(){
        _vfx = GetComponent<VisualEffect>();
        //_dragon = GameObject.Find("Dragon").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update(){
        if(_isActive){
            AlignBeam();
        }
    }

    public void Go(){
        _isActive = true;
        AlignBeam();
        _vfx.Play();
    }

    void AlignBeam(){
        transform.LookAt(_dragon);
        var offset = _dragon.position - transform.position;
        var distance = offset.magnitude / 5;
        Debug.Log(distance+" "+offset);
        _vfx.SetFloat("length", distance);
    }
}
