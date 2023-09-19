using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MushroomQuestUI : MonoBehaviour
{
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start(){
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Awake(){
        LeanTween.reset();
    }

    public void UpdateText(int mushrooms){
        text.text = ""+mushrooms+"/5";
    }

    public void Appear(){
        LeanTween.moveLocalX(this.gameObject, 770, 1.5f).setEase(LeanTweenType.easeOutSine);
    }
    public void SendOff(){
        LeanTween.moveLocalX(this.gameObject, 1100, 1.5f).setEase(LeanTweenType.easeOutSine);
    }
}
