using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Animal : MonoBehaviour
{
    [SerializeField] float homeRange;
    [SerializeField] float wanderCoolDownMax;
    [SerializeField] float sitCoolDownMax;
    [SerializeField] float idleSpeed;
    Vector3 _home;
    float _wanderCoolDown;
    float _sitCoolDown;
    bool _isSitting;
    float _timePassed;

    Animator _animator;
    NavMeshAgent _agent;
    // Start is called before the first frame update
    void Start(){
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _home = transform.position;
        _wanderCoolDown = Random.Range(1, wanderCoolDownMax);
        _sitCoolDown = Random.Range(5, sitCoolDownMax);
    }

    // Update is called once per frame
    void Update(){
        _timePassed += Time.deltaTime;

        _animator.SetFloat("Speed", _agent.hasPath ? 0.5f:0);
        if(!_isSitting){
            if(_timePassed >= _wanderCoolDown ){
                Vector3 newPos = RandomWanderPosition(_home, homeRange, -1);
                _agent.SetDestination(newPos);
                _timePassed = 0f;
                _wanderCoolDown = Random.Range(1, wanderCoolDownMax);
            }
            if(_timePassed >= _sitCoolDown){
                _agent.SetDestination(transform.position);
                _animator.SetTrigger("Sit");
                _sitCoolDown = Random.Range(1, sitCoolDownMax);
                _timePassed = 0f;
                _isSitting = true;
            }
        } else {
            if(_timePassed >= _sitCoolDown){
                _animator.SetTrigger("Stand");
                _isSitting = false;
                _sitCoolDown = Random.Range(1, sitCoolDownMax);
                _timePassed = 0f;
            }
        }
    }

    Vector3 RandomWanderPosition(Vector3 origin, float maxDist, int layerMask){
        Vector3 randomDirection = Random.insideUnitSphere * maxDist;
        randomDirection += origin;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, maxDist, layerMask);
        return hit.position;
    }
}
