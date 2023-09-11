using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class IdleBehaviour : MonoBehaviour
{
    [SerializeField] float wanderCoolDownMax;
    float _wanderCoolDown;
    [SerializeField] float idleSpeed;
    
    bool _isFree = true;
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
        if(_isFree && wanderCoolDownMax > 0){
            _animator.SetFloat("Speed", _agent.hasPath ? idleSpeed:0);
            _timePassed += Time.deltaTime;
            if(_timePassed >= _wanderCoolDown){
                Wander();
            }
        }
    }

    void Wander(){
        Vector3 newPos = RandomDestination();
        _agent.SetDestination(newPos);
        _wanderCoolDown = Random.Range(3, wanderCoolDownMax);
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

    public void StartConversation(){
        _agent.updateRotation = false;
        _agent.isStopped = true;
        _animator.SetFloat("Speed",0);
        transform.LookAt(GameObject.FindWithTag("Player").transform);
        _isFree = false;
    }

    public void EndConversation(){
        _agent.isStopped = false;
        _agent.updateRotation = true;
        _isFree = true;
    }
}
