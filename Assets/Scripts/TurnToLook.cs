using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurnToLook : MonoBehaviour
{
    public Transform LookGoal;
    NavMeshAgent _agent;

    bool _hasLooked;

    private void Start() {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if(!_agent.hasPath && !_hasLooked){
            Look();
            _hasLooked = true;
        }
        else
        _hasLooked = false;
    }
    public void Look(){
        _agent.updateRotation = false;
        _agent.isStopped = true;
        transform.LookAt(LookGoal);
        _agent.isStopped = false;
        _agent.updateRotation = true;
    }

}
