using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class IdleBehaviour : MonoBehaviour
{
    [SerializeField] float homeRange;
    [SerializeField] float wanderCoolDown;
    [SerializeField] float idleSpeed;
    [SerializeField] Vector3 home;
    
    float _timePassed;
    List<Vector3> _waypoints = new List<Vector3>();
    Animator _animator;
    NavMeshAgent _agent;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        CollectWayPoints();
    }

    void CollectWayPoints(){
        foreach (Transform transform in GetComponentsInChildren<Transform>())
        {
            if(transform.CompareTag("WayPoint"))
                _waypoints.Add(transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("Speed", _agent.hasPath ? idleSpeed:0);
        _timePassed += Time.deltaTime;
        if(_timePassed >= wanderCoolDown){
            Wander();
        }
    }

    void Wander(){
        Vector3 newPos = RandomDestination();
        _agent.SetDestination(newPos);
        wanderCoolDown = Random.Range(3, 6);
        _timePassed = 0f;
    }

    Vector3 RandomDestination(){
        int random = Random.Range(0, _waypoints.Count-1);
        return _waypoints[random];
    }

    Vector3 RandomWanderPosition(Vector3 origin, float maxDist, int layerMask){
        Vector3 randomDirection = Random.insideUnitSphere * maxDist;
        randomDirection += origin;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, maxDist, layerMask);
        return hit.position;
    }
}
