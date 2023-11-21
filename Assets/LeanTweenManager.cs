using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenManager : MonoBehaviour
{
    public GameObject[] leanTweeners;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("InitTweens");
    }
    IEnumerator InitTweens(){
        for(int i = 0; i< leanTweeners.Length; i++){
            leanTweeners[i].SetActive(false);
        }
        int j = 1;
        while (j>0)
        {
            j--;
            yield return null;
        }
        for(int i = 0; i< leanTweeners.Length; i++){
            leanTweeners[i].SetActive(true);
        }
    }
}
