using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayPOIInformation : MonoBehaviour
{
    public TextMeshProUGUI POI_name;
    public TextMeshProUGUI POI_description;
    public GameObject FatherGascoigne;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        PointOfInterest.OnCollect_Display += DisplayPoinOfInterest;
    }

    void OnDisable()
    {
        PointOfInterest.OnCollect_Display -= DisplayPoinOfInterest;
    }

    public void DisplayPoinOfInterest(string name, string desc){
        POI_name.text = name;
        POI_description.text = desc;
        //LeanTween.moveLocalY(FatherGascoigne, 20, 0.75f).setEase(LeanTweenType.easeOutBounce);
        LeanTween.moveLocalY(POI_description.gameObject, -200, 1.5f).setEase(LeanTweenType.easeOutSine);
        LeanTween.moveLocalY(POI_name.gameObject, 500, 1.5f).setEase(LeanTweenType.easeOutSine).setOnComplete(WipeDisplay);
    }

    public void WipeDisplay(){
        LeanTween.moveLocalY(POI_description.rectTransform.gameObject, -400, 0.75f).setDelay(5f).setEase(LeanTweenType.easeInSine);
        LeanTween.moveLocalY(POI_name.gameObject, 800, 0.75f).setDelay(5f).setEase(LeanTweenType.easeInSine);
    }

}
