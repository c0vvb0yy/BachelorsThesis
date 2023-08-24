using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PointOfInterest : MonoBehaviour{
    new public string name;
    public string description;

    public static event Action OnCollect;
    public static event Action<string> OnCollect_Data;
    public static event Action<string, string> OnCollect_Display;

    Collider coll;
    VisualEffect vfx;

    // Start is called before the first frame update
    void Start(){
        coll = GetComponent<Collider>();
        vfx = GetComponentInChildren<VisualEffect>();
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            OnCollect.Invoke();
            OnCollect_Data.Invoke(name);
            OnCollect_Display.Invoke(name, description);
            vfx.SetFloat("SpawnMult", 0);
            vfx.SetFloat("TrailMult", 5);
            Destroy(this.gameObject, 5f);
        }
    }
}
