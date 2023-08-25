using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PointOfInterest : MonoBehaviour{
    new public string name;
    public string description;

    public static event Action<string> OnCollect_Data;
    public static event Action<string, string> OnCollect_Display;

    VisualEffect vfx;

    bool _collectable;

    // Start is called before the first frame update
    void Start(){
        vfx = GetComponentInChildren<VisualEffect>();
        _collectable = true;
    }
    
    private void OnTriggerEnter(Collider other) {
        if(_collectable == true && other.tag == "Player"){
            _collectable = false;
            OnCollect_Data.Invoke(name);
            OnCollect_Display.Invoke(name, description);
            vfx.SetFloat("SpawnMult", 0);
            vfx.SetFloat("TrailMult", 5);
            Destroy(this.gameObject, 5f);
        }
    }
}
