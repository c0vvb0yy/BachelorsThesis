using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{

    Animator _animator;
    float _timePassed;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_timePassed >= Random.Range(5, 10)){
            if(_animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle01")){
                _animator.SetTrigger("Idle2");
            } else {
                _animator.SetTrigger("Idle1");
            }
            _timePassed = 0f;
        }
        _timePassed += Time.deltaTime;
    }
}
