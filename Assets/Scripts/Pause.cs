using System;
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
    private int _creditsIndex = 0;
    public TextMeshProUGUI credits_text;
    private String[] _credits = {
        "Animations provided by Mixamo\nBackground Music by Jenny Walter\nPick up sfx by Kenny.nl\nsword slash sfx by 666HeroHero from Pixabay\nsword sheath Sound Effect by Ryan Lewis",
        "World Assets by BOKI\nPalm Trees by DinV_Studio\nHouses and furniture by Gigel\nFarm assets by JustCreate\n Witch's House by Bizulka",
        "Enemies/Wizard model and animations by Dungeon Mason\nGoats and Sheep by UrsaAnimations\nMage Tower by Matt Art\nSwords by PurePoly\nFire by Indian Ocean Assets",
        "Skybox by Render Knight\nObelisk Model by JUHWAN\nNPCs by Distant Lands\nBlackSmith house by Bitcraft\n& You For Playing <3"
    };
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
            _input.cursorInputForLook = PauseCanvas.activeInHierarchy ? false : true;
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
        
        string thirdLine = "Don't forget to check out the survey! <3";

        stats.text = firstLine+secondLine+thirdLine;
    }

    public void Resume(){
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SurveyLink(){
        Application.OpenURL("https://forms.gle/topd2EgsnYYgYz7h7");
    }

    public void ShowCredits(){
        if(_creditsIndex >= _credits.Length) _creditsIndex = 0;
        stats.text = _credits[_creditsIndex];
        _creditsIndex++;
        credits_text.text = ""+_creditsIndex+"/"+_credits.Length;
    }

}
