using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StarterAssets;
using UnityEngine.UI;

public class EnemyLockOn : MonoBehaviour
{
    Transform _currentTarget;
    Transform _closestEnemy;
    Animator _animator;

    CameraInput _input;

    [SerializeField] LayerMask targetLayers;
    [SerializeField] Transform enemyTargetLocator;

    [Header("Settings")]
    [SerializeField] bool zeroVertLook;
    [SerializeField] float noticeZone = 10f;
    [SerializeField] float leaveZone = 13f;
    float _activeZone;
    [SerializeField] float lookAtSmoothing = 2f;
    [Tooltip("Angle_Degree")][SerializeField] float maxNoticeAngle = 60f;
    [SerializeField]float crossHairScale = 0.1f;

    Transform _camera;
    bool _enemyLocked;
    float _currentYOffset;
    Vector3 _pos;

    ThirdPersonController _player;

    public static event Action<bool, Vector3> onEnemyLockOn;

    Collider[] _nearbyTargets;
    int _enemyIndex;

   // [SerializeField] CameraFollow cameraFollow;
    [SerializeField] Transform lockOnCanvas;
    DefMovement _defMovement;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _camera = Camera.main.transform;
        _input = GetComponent<CameraInput>();
        lockOnCanvas.gameObject.SetActive(false);
        _activeZone = noticeZone;
        _player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        ScanNearBy();
        if(_nearbyTargets.Length <= 0){
            ResetTarget();
        }
        //cameraFollow.lockedTarget = _enemyLocked;
        if(_input.lockOn){
            _input.lockOn = false;
            if(_enemyLocked){
                ResetTarget();
                return;
            }
            if(_currentTarget = _closestEnemy) FoundTarget();
            else{
                print("this one?");
                ResetTarget();
            } 

        }

        if(_enemyLocked){
            if(!TargetOnRange()){
                ResetTarget();
            }
            if(_input.changeEnemy != 0){
                ChangeTarget();
            }
            LookAtTarget();
        }
        else if(_closestEnemy != null) {
            if(!TargetOnRange()){
                ResetTarget();
            }
            ShowPotentialTarget();
        }
    }

    void ResetTarget(){
        lockOnCanvas.gameObject.SetActive(false);
        _currentTarget = null;
        _closestEnemy = null;
        _enemyLocked = false;
        _animator.Play("FollowState");
        onEnemyLockOn.Invoke(_enemyLocked, Vector3.zero);
    }

    void ChangeTarget(){
        //we cannot change targets if there'S only 1 or less target available
        if(_nearbyTargets.Length <= 1) return;
        if(_input.changeEnemy > 0){
            _enemyIndex += 1;
            if(_enemyIndex >= _nearbyTargets.Length) _enemyIndex = 0;
            _currentTarget = _nearbyTargets[_enemyIndex].transform;
        }
        else{
            _enemyIndex -=1;
            if(_enemyIndex <= 0) _enemyIndex = _nearbyTargets.Length-1;
            _currentTarget = _nearbyTargets[_enemyIndex].transform;
        }
    }

    void ScanNearBy(){
        _nearbyTargets = Physics.OverlapSphere(transform.position, _activeZone, targetLayers);
        float closestAngle = maxNoticeAngle;
        Transform closestTarget = null;
        if(_nearbyTargets.Length <= 0) {
            _activeZone = noticeZone;
            return;
        }
        _activeZone = leaveZone;

        for(int i = 0; i < _nearbyTargets.Length; i++){
            Vector3 dir = _nearbyTargets[i].transform.position - _camera.position;
            dir.y = 0;
            float angle = Vector3.Angle(_camera.forward, dir);

            if(angle < closestAngle){
                closestTarget = _nearbyTargets[i].transform;
                closestAngle = angle;
                _enemyIndex = i;
            } 
        }

        if(!closestTarget) return;
        Vector3 tarPos = GetTargetPos(closestTarget);
        if(Blocked(tarPos)) return;
        _closestEnemy = closestTarget;
    }

    Vector3 GetTargetPos(Transform closestTarget){
        float h1 = closestTarget.GetComponent<NavMeshAgent>().height;
        float h2 = closestTarget.localScale.y;
        float h = h1 * h2;
        float quarter_h = h / 4;
        _currentYOffset = h - quarter_h;
        if(zeroVertLook && _currentYOffset > 1.6f && _currentYOffset < 1.6f * 3)_currentYOffset = 1.6f;
        return closestTarget.position + new Vector3(0,_currentYOffset, 0);
    }

    bool Blocked(Vector3 target){
        RaycastHit hit;
        if(Physics.Linecast(transform.position + Vector3.up * 0.5f, target, out hit)){
            if(!hit.transform.CompareTag("Enemy")) return true;
        }
        return false;
    }

    void FoundTarget(){
        //lockOnCanvas.gameObject.SetActive(true);
        lockOnCanvas.GetComponentInChildren<Image>().color = Color.white;
        _animator.Play("TargetState");
        _enemyLocked = true;
        onEnemyLockOn.Invoke(_enemyLocked, _currentTarget.position);
    }

    void ShowPotentialTarget(){
        lockOnCanvas.gameObject.SetActive(true);
        PrepareCanvas();
        lockOnCanvas.GetComponentInChildren<Image>().color = Color.black;
    }

    bool TargetOnRange(){
        float dis = (transform.position - _pos).magnitude;
        if(dis/2 > leaveZone) return false; else return true;

    }

    void LookAtTarget(){
        if(!_currentTarget){
            ResetTarget();
            return;
        }
        
        PrepareCanvas();

        enemyTargetLocator.position = _pos;
        Vector3 dir = _currentTarget.position - transform.position;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * lookAtSmoothing);
    }

    void PrepareCanvas(){
        _pos = _closestEnemy.position + new Vector3(0, _currentYOffset, 0);
        lockOnCanvas.position = _pos;
        lockOnCanvas.localScale = Vector3.one * ((_camera.position - _pos).magnitude * crossHairScale);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, noticeZone);   
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, leaveZone);   
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _activeZone);   
    }
}
