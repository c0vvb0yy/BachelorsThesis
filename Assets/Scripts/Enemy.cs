using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour
{
    [SerializeField] float maxHealth = 3f;
    float _currentHealth = 3f;

    private Healthbar _healthbar;
    
    [Header("IdleBehaviour")]
    [SerializeField] float homeRange;
    [SerializeField] float wanderCoolDownMax;
    [SerializeField] float idleSpeed;
    Vector3 _home;
    float _wanderCoolDown;
    

    [Header("Combat")]
    [SerializeField] float attackCoolDown = 3f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float aggroRange = 4f;
    [SerializeField] float combatSpeed;
    [SerializeField] GameObject onHitEffect;
    [SerializeField] GameObject damageNumber;

    GameObject _player;
    Animator _animator;
    NavMeshAgent _agent;
    AudioSource _audio;
    float _timePassed;
    float _newDestinationCoolDown = 0.5f;

    bool _isDead;
    bool _inCombat;

    public static event Action<GameObject> OnDeath;

    [HideInInspector] public bool dragon;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _audio = GetComponent<AudioSource>();
        _home = transform.position;
        _currentHealth = maxHealth;
        _healthbar = GetComponentInChildren<Healthbar>();
        _wanderCoolDown = UnityEngine.Random.Range(1, wanderCoolDownMax);
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

        if(Vector3.Distance(_player.transform.position, transform.position) <= aggroRange && !_inCombat && CheckVerticality()){
            StartCombatBehaviour();
        } 
        if(Vector3.Distance(_player.transform.position, transform.position) > aggroRange && _inCombat) {
            StartIdleBehaviour();
        }

    }

    void StartCombatBehaviour(){
        _timePassed = 999; //Bumping up time passed so that an enemy can immediealty start attacking
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
                _agent.SetDestination(transform.position);
                _animator.SetFloat("Speed", _agent.hasPath ? 1:0);
                _animator.SetTrigger("Attack");
                _timePassed = 0f;
            }
        }
        if(_newDestinationCoolDown <= 0 ){
                _newDestinationCoolDown = 2.5f;
                _agent.SetDestination(_player.transform.position);
        }
        _newDestinationCoolDown -= Time.deltaTime;
        if(dragon)
            return;
        transform.LookAt(_player.transform);
    }

    void IdleBehaviour(){
        _animator.SetFloat("Speed", _agent.hasPath ? 0.5f:0);
        if(_timePassed >= _wanderCoolDown){
            Vector3 newPos = RandomWanderPosition(_home, homeRange, -1);
            _agent.SetDestination(newPos);
            _timePassed = 0f;
            _wanderCoolDown = UnityEngine.Random.Range(1, wanderCoolDownMax);
        }
    }

    Vector3 RandomWanderPosition(Vector3 origin, float maxDist, int layerMask){
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * maxDist;
        randomDirection += origin;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, maxDist, layerMask);
        return hit.position;
    }

    public void TakeDamage(float damageAmount, Vector3 point){
        if(_isDead) return;
        _currentHealth -= damageAmount;
        _healthbar.UpdateHealthbar(maxHealth, _currentHealth);
        _animator.SetTrigger("TakeDamage");
        SpawnHitEffect(point);
        ShowDamageNumber(damageAmount);
        if(_currentHealth <= 0){
            Die();
        }
    }

    //leaves the enemy with one fraction of health left
    public void ReduceHealth(float fraction){
        _currentHealth = _currentHealth/fraction;
        _healthbar.UpdateHealthbar(maxHealth, _currentHealth);
    }

    public void StartDealDamage(){
        //GetComponentInChildren<EnemyDamageDealer>().StartDealDamage();
    }

    public void EndDealDamage(){
        //GetComponentInChildren<EnemyDamageDealer>().EndDealDamage();
    }

    public void Die(){
        _animator.SetTrigger("Die");
        _isDead = true;
        GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.black;

        var eventData = new Dictionary<string, object>{
            {"EnemyType", this.gameObject.name}
        };
        AnalyticsService.Instance.CustomData("EnemyKill", eventData);
        OnDeath.Invoke(this.gameObject);
        Destroy(this.gameObject, 5f);
    }

    bool CheckVerticality(){
        if(_player.transform.position.y <= transform.position.y + 5 
        && _player.transform.position.y >= transform.position.y - 5)
            return true;
        return false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, homeRange);
    }

    internal void SpawnHitEffect(Vector3 point){
        GameObject hitVFX = Instantiate(onHitEffect, this.transform);
        hitVFX.transform.position = point;
        _audio.Play();
        Destroy(hitVFX, 5f);
    }

    void ShowDamageNumber(float damageAmount){
        var number = Instantiate(damageNumber, transform.position, Quaternion.identity, transform);
        var pos = number.transform.position;
        pos = new Vector3(pos.x, pos.y, pos.z+1f);
        number.transform.position = pos;
        number.GetComponent<TextMeshPro>().text = damageAmount.ToString();
        number.LeanMoveLocalX(UnityEngine.Random.Range(-1.5f, 1.5f), 1f);
        number.LeanScale(Vector3.one, 1f).setEaseOutBounce();
        number.LeanScale(Vector3.zero, 1f).setEaseInBounce().setDelay(3f);
    }
}
