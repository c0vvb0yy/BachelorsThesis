using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class Mage : MonoBehaviour
{
    [SerializeField] float _idleSwitchCooldown;
    float _timePassed;
    float _timeNeeded;
    Animator _animator;
    NavMeshAgent _agent;
    public Transform beforeDragon;
    NPCDialogueManager _dialogue;
    private void OnEnable() {
        Dragon.DragonSleep += DragonAsleep;
        Dragon.DragonDeath += DragonDeath;
    }
    private void OnDisable() {
        Dragon.DragonSleep -= DragonAsleep;
        Dragon.DragonDeath -= DragonDeath;
    }

    private void Awake() {
        GameObject.FindWithTag("Runner").GetComponent<DialogueRunner>().AddCommandHandler(
            "AddObeliskTalk",
            AddObeliskTalk
        );
    }
    // Start is called before the first frame update
    void Start(){
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        _dialogue = GetComponent<NPCDialogueManager>();
        _timeNeeded = Random.Range(_idleSwitchCooldown/4, _idleSwitchCooldown);
        foreach (Transform transform in GetComponentsInChildren<Transform>()){
            if(transform.CompareTag("WayPoint"))
                beforeDragon = transform;
        }
    }

    void Update(){
        if(!_agent.hasPath){
            if(_timePassed >= _timeNeeded){
                SwitchIdleAnimation();
                _timePassed = 0f;
                _timeNeeded = Random.Range(_idleSwitchCooldown/4, _idleSwitchCooldown);
            }
        }
        _timePassed += Time.deltaTime;
    }

    void SwitchIdleAnimation(){
        if(_animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle01")){
            _animator.SetTrigger("Idle2");
        } else {
            _animator.SetTrigger("Idle1");
        }
    }

    void DragonAsleep(){
        _agent.isStopped = true;
        this.transform.position = beforeDragon.position;
        this.transform.rotation = beforeDragon.rotation;
        _agent.isStopped = false;
    }

    void DragonDeath(){
        _dialogue.StarterNodes.Add("Consequence");
        _dialogue.index += 1;
        _dialogue.StarterNodes.Add("End");
    }

    [YarnCommand("AddObeliskTalk")]
    void AddObeliskTalk(){
        _dialogue.StarterNodes.Add("ObeliskTalk");
        _dialogue.index += 1;//_dialogue.StarterNodes.Count;
    }
}
