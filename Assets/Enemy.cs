using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 3f;

    [Header("Combat")]
    [SerializeField] float attackCoolDown = 3f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float aggroRange = 4f;
    

    GameObject _player;
    Animator _animator;
    NavMeshAgent _agent;
    float _timePassed;
    float _newDestinationCoolDown = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        //attackRange = GetComponentInChildren<EnemyDamageDealer>().attackLength;
    }

    private void Update() {
        _animator.SetFloat("Speed", _agent.velocity.magnitude / _agent.speed);

        if(_timePassed >= attackCoolDown){
            if(Vector3.Distance(_player.transform.position, transform.position) <= attackRange){
                _animator.SetTrigger("Attack");
                _timePassed = 0f;
            }
        }
        _timePassed += Time.deltaTime;

        if(_newDestinationCoolDown <= 0 
            && Vector3.Distance(_player.transform.position, transform.position) <= aggroRange){
                _newDestinationCoolDown = 0.5f;
                _agent.SetDestination(_player.transform.position);
        }
        _newDestinationCoolDown -= Time.deltaTime;
        transform.LookAt(_player.transform);
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
        Destroy(this.gameObject, 5f);
    }
     private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        
    }
}
