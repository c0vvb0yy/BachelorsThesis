using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteractUI : MonoBehaviour
{
    public Sprite questAvailable;
    public Sprite interact;
    Image _image;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponentInChildren<Image>();
    }

    public void QuestAvailable(){
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
        _image.sprite = questAvailable;
    }
    public void ShowInteractable(){
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
        _image.sprite = interact;
    }

    public void CleanUp(){
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
    }
}
