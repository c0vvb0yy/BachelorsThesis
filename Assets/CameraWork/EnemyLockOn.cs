using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class EnemyLockOn : MonoBehaviour
{
    Transform _currentTarget;
    Animator _animator;

    CameraInput _input;

    [SerializeField] LayerMask targetLayers;
    [SerializeField] Transform enemyTargetLocator;

    [Header("Settings")]
    [SerializeField] bool zeroVertLook;
    [SerializeField] float noticeZone = 10f;
    [SerializeField] float lookAtSmoothing = 2f;
    [Tooltip("Angle_Degree")][SerializeField] float maxNoticeAngle = 60f;
    [SerializeField]float crossHairScale = 0.1f;

    Transform _camera;
    bool _enemyLocked;
    float _currentYOffset;
    Vector3 _pos;

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
    }

    // Update is called once per frame
    void Update()
    {
        //cameraFollow.lockedTarget = _enemyLocked;
        if(_input.lockOn){
            print("lock on pressed");
            if(_currentTarget){
                ResetTarget();
                return;
            }
            if(_currentTarget = ScanNearBy()) FoundTarget();
            else ResetTarget();

            _input.lockOn = false;
        }

        if(_enemyLocked){
            if(!TargetOnRange())
                ResetTarget();
            LookAtTarget();
        }
    }

    void ResetTarget(){
        lockOnCanvas.gameObject.SetActive(false);
        _currentTarget = null;
        _enemyLocked = false;
        //_animator.SetLayerWeight(1,0);
        _animator.Play("FollowState");
    }

    Transform ScanNearBy(){
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);
        float closestAngle = maxNoticeAngle;
        Transform closestTarget = null;
        if(nearbyTargets.Length <= 0) return null;

        for(int i = 0; i < nearbyTargets.Length; i++){
            Vector3 dir = nearbyTargets[i].transform.position - _camera.position;
            dir.y = 0;
            float angle = Vector3.Angle(_camera.forward, dir);

            if(angle < closestAngle){
                closestTarget = nearbyTargets[i].transform;
                closestAngle = angle;
            } 
        }

        if(!closestTarget) return null;
        float h1 = closestTarget.GetComponent<NavMeshAgent>().height;
        float h2 = closestTarget.localScale.y;
        float h = h1 * h2;
        float quarter_h = h / 4;
        _currentYOffset = h - quarter_h;
        if(zeroVertLook && _currentYOffset > 1.6f && _currentYOffset < 1.6f * 3)_currentYOffset = 1.6f;
        Vector3 tarPos = closestTarget.position + new Vector3(0,_currentYOffset, 0);
        if(Blocked(tarPos)) return null;
        return closestTarget;
    }

    bool Blocked(Vector3 target){
        RaycastHit hit;
        if(Physics.Linecast(transform.position + Vector3.up * 0.5f, target, out hit)){
            if(!hit.transform.CompareTag("Enemy")) return true;
        }
        return false;
    }

    void FoundTarget(){
        lockOnCanvas.gameObject.SetActive(true);
        _animator.SetLayerWeight(1, 1);
        _animator.Play("TargetState");
        _enemyLocked = true;
    }

    bool TargetOnRange(){
        float dis = (transform.position - _pos).magnitude;
        if(dis/2 > noticeZone) return false; else return true;

    }

    void LookAtTarget(){
        if(!_currentTarget){
            ResetTarget();
            return;
        }
        _pos = _currentTarget.position + new Vector3(0, _currentYOffset, 0);
        lockOnCanvas.position = _pos;
        lockOnCanvas.localScale = Vector3.one * ((_camera.position - _pos).magnitude * crossHairScale);

        enemyTargetLocator.position = _pos;
        Vector3 dir = _currentTarget.position - transform.position;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * lookAtSmoothing);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, noticeZone);   
    }
}
