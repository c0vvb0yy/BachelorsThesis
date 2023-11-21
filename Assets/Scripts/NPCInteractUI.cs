using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteractUI : MonoBehaviour
{
    public Sprite questAvailable;
    public Sprite interact;
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponentInChildren<Image>();
    }

    public void QuestAvailable(){
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        image.sprite = questAvailable;
    }
    public void ShowInteractable(){
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        image.sprite = interact;
    }

    public void CleanUp(){
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }
}
