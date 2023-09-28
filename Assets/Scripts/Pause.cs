using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;

public class Pause : MonoBehaviour
{
    StarterAssetsInputs _input;
    public GameObject PauseCanvas;
    public TextMeshProUGUI stats;
    public DisplayPOIInformation stats_POI;
    // Start is called before the first frame update
    void Start(){
        _input = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update(){
        if(_input.pause){
            PauseCanvas.SetActive(!PauseCanvas.activeInHierarchy);
            if(!SaveSystem.Init()){
                DataManager dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
			    dataManager.Save();
            } 
            Time.timeScale = PauseCanvas.activeInHierarchy ? 0 : 1;
            Cursor.lockState = PauseCanvas.activeInHierarchy ? CursorLockMode.Confined : CursorLockMode.Locked;
            UpdateText();
            _input.pause = false;
        }
    }

    public void UpdateText(){
        string firstLine = "You've discovered "+stats_POI.currentPointsCollected+"/"+stats_POI.totalPoints+" points of interest!\n";
        string secondLine = "QUESTLOG:\n";
        SaveData saveData = SaveSystem.Load();
        if(saveData.mushroomQuest_done)
            secondLine += "You've helped forage mushrooms for the village elder!\n";
        else 
            secondLine += "Someone needs your help finding mushrooms\n";
        if(saveData.farmQuest_done)
            secondLine += "You've saved the farm from the monster rampage!\n";
        else
            secondLine += "The farm is currently in danger\n";
        if(saveData.dragon_killed)
            secondLine += "You've slain the dragon threatening the village\n";
        else if(saveData.dragon_pacified)
            secondLine += "You've pacified the dragon and returned it to its peaceful slumber\n";
        else
            secondLine += "The dragon is still at large.\n";
        
        string thirdLine = "\nDon't forget to check out the survey! <3";

        stats.text = firstLine+secondLine+thirdLine;
    }

    public void Resume(){
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SurveyLink(){
        Application.OpenURL("https://docs.google.com/document/d/1HA2--5m_sEPBo6rqw2E4NoINKd0AQA2dmTqzZJNvYGk/edit#heading=h.mu8yjtpnxvqv");
    }
}
