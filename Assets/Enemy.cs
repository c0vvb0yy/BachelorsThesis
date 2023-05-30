using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 3f;
    
    [Header("IdleBehaviour")]
    [SerializeField] float homeRange;
    [SerializeField] float wanderCoolDown;
    [SerializeField] float idleSpeed;
    Vector3 _home;
    

    [Header("Combat")]
    [SerializeField] float attackCoolDown = 3f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float aggroRange = 4f;
    
    [SerializeField] float combatSpeed;
    

    GameObject _player;
    Animator _animator;
    NavMeshAgent _agent;
    float _timePassed;
    float _newDestinationCoolDown = 0.5f;

    bool _isDead;
    bool _inCombat;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _home = transform.position;
    }

    private void Update() {
        if(_isDead){
            return;
        }
        
        _timePassed += Time.deltaTime;

        if(_inCombat){
            CombatBehaviour();
        }else{
            IdleBehaviour();
        }

        if(Vector3.Distance(_player.transform.position, transform.position) <= aggroRange && !_inCombat){
            StartCombatBehaviour();
        } 
        if(Vector3.Distance(_player.transform.position, transform.position) > aggroRange && _inCombat) {
            StartIdleBehaviour();
        }

    }

    void StartCombatBehaviour(){
        _agent.speed = combatSpeed;
        _inCombat = true;
    }

    void StartIdleBehaviour(){
        _agent.speed = idleSpeed;
        _inCombat = false;
    }

    void CombatBehaviour(){
         
        _animator.SetFloat("Speed", _agent.hasPath ? 1:0);
        if(_timePassed >= attackCoolDown){
            if(Vector3.Distance(_player.transform.position, transform.position) <= attackRange){
                _animator.SetTrigger("Attack");
                _timePassed = 0f;
            }
        }
        if(_newDestinationCoolDown <= 0 ){
                _newDestinationCoolDown = 0.5f;
                _agent.SetDestination(_player.transform.position);
        }
        _newDestinationCoolDown -= Time.deltaTime;
        transform.LookAt(_player.transform);
    }

    void IdleBehaviour(){
        _animator.SetFloat("Speed", _agent.hasPath ? 0.5f:0);
        if(_timePassed >= wanderCoolDown){
            Vector3 newPos = RandomWanderPosition(_home, homeRange, -1);
            _agent.SetDestination(newPos);
            _timePassed = 0f;
        }
    }

    Vector3 RandomWanderPosition(Vector3 origin, float maxDist, int layerMask){
        Vector3 randomDirection = Random.insideUnitSphere * maxDist;
        print(randomDirection);
        randomDirection += origin;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, maxDist, layerMask);
        print(hit.position);
        return hit.position;
    }

    public void TakeDamage(float damageAmount){
        health -= damageAmount;
        _animator.SetTrigger("TakeDamage");

        if(health <= 0){
            Die();
        }
    }

    public void StartDealDamage(){
        GetComponentInChildren<EnemyDamageDealer>().StartDealDamage();
    }

    public void EndDealDamage(){
        GetComponentInChildren<EnemyDamageDealer>().EndDealDamage();
    }

    public void Die(){
        _animator.SetTrigger("Die");
        _isDead = true;
        Destroy(this.gameObject, 5f);
    }
     private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        
    }
}
