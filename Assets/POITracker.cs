using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POITracker : MonoBehaviour{
    public int numberOfTotalPoints;
    public int currentPointsCollected;

    void OnEnable(){
        PointOfInterest.OnCollect += UpdateCollection;
    }

    void OnDisable(){
        PointOfInterest.OnCollect -= UpdateCollection;
    }

    // Start is called before the first frame update
    void Start(){
        numberOfTotalPoints = GetComponentsInChildren<Transform>().Length;
        currentPointsCollected = 0;
    }

    void UpdateCollection(){
        currentPointsCollected++;
    }
}
