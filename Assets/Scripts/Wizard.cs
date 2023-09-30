using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(IdleBehaviour))]
public class Wizard : MonoBehaviour
{
    float _timePassed;
    Animator _animator;
    IdleBehaviour _idling;
    NavMeshAgent _agent;
    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _idling = GetComponent<IdleBehaviour>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start() {
        //_idling.StartConversation();
        _agent.isStopped = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_agent.hasPath){
            if(_timePassed >= Random.Range(_idling.wanderCoolDownMax/4, _idling.wanderCoolDownMax)){
                SwitchIdleAnimation();
            }
            _timePassed += Time.deltaTime;
        }
    }

    void SwitchIdleAnimation(){
        if(_animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle01")){
            _animator.SetTrigger("Idle2");
        } else {
            _animator.SetTrigger("Idle1");
        }
        _timePassed = 0f;
    }
}
