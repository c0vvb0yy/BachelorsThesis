using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    new public string name;
    public string description;

    public static event Action<string> OnCollect_Data;
public static event Action<string, string> OnCollect_Display;

    Collider coll;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider>();
        LeanTween.rotateAround(this.gameObject, Vector3.left, 180f, 1f).setLoopPingPong();
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
            Destroy(this.gameObject);
        }
    }
}
