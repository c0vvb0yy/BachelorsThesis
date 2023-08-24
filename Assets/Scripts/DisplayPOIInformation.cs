using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayPOIInformation : MonoBehaviour
{
    
    public int totalPoints;
    public int currentPointsCollected;
    public TextMeshProUGUI POI_name;
    public TextMeshProUGUI POI_description;
    public TextMeshProUGUI POI_tracker;

    void OnEnable(){
        PointOfInterest.OnCollect_Display += DisplayPoinOfInterest;
    }

    void OnDisable(){
        PointOfInterest.OnCollect_Display -= DisplayPoinOfInterest;
    }

    void Start(){
        totalPoints = GameObject.FindGameObjectsWithTag("POI").Length;
        POI_tracker.text = ""+currentPointsCollected+"/"+totalPoints;
    }

    public void DisplayPoinOfInterest(string name, string desc){
        POI_name.text = name;
        POI_description.text = desc;
        currentPointsCollected++;
        LeanTween.moveLocalY(POI_description.gameObject, -200, 1.5f).setEase(LeanTweenType.easeOutSine);
        LeanTween.moveLocalY(POI_name.gameObject, 500, 1.5f).setEase(LeanTweenType.easeOutSine).setOnComplete(WipeDisplay);
        POI_tracker.text = ""+currentPointsCollected+"/"+totalPoints;
    }
    
    public void WipeDisplay(){
        LeanTween.moveLocalY(POI_description.rectTransform.gameObject, -400, 0.75f).setDelay(3.5f).setEase(LeanTweenType.easeInSine);
        LeanTween.moveLocalY(POI_name.gameObject, 800, 0.75f).setDelay(3.5f).setEase(LeanTweenType.easeInSine);
    }

}
