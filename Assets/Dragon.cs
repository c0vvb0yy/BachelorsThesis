using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    DragonAttack _tail;
    ThirdPersonController _player;
    void Start()
    {
        _tail = GetComponentInChildren<DragonAttack>();
        _player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_tail.tailConnected){
            
            _tail.tailConnected = false;
        }
    }
}
