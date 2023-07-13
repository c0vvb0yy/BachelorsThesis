using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PointOfInterest : MonoBehaviour
{
    new public string name;
    public string description;

    public static event Action<string> OnCollect_Data;
    public static event Action<string, string> OnCollect_Display;

    Collider coll;
    VisualEffect vfx;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider>();
        vfx = GetComponentInChildren<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerEnter(Collider other) {
        Debug.Log("entered");
        if(other.tag == "Player")
        {
            OnCollect_Data.Invoke(name);
            OnCollect_Display.Invoke(name, description);
            vfx.SetFloat("SpawnMult", 0);
            vfx.SetFloat("TrailMult", 5);
            Destroy(this.gameObject, 5f);
        }
    }
}
