using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    new public string name;
    public string description;

    public static event Action<string> OnCollect;

    Collider coll;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerEnter(Collider other) {
        Debug.Log("entered");
        if(other.tag == "Player")
        {
            OnCollect.Invoke(name);
        }
    }
}
