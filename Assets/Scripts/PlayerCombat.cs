using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using StarterAssets;

public class PlayerCombat : MonoBehaviour
{

    [SerializeField] GameObject VFXHolder;
    [SerializeField] List<GameObject> slashVFX;
    GameObject _currentVisualEffect;
    public GameObject currentWeapon;

    private Animator _animator;
    private StarterAssetsInputs _input;
    private bool _inCombat = false;
    private bool _attack = false;
    private float _timePassed;
    private float _clipLength;
    private float _clipSpeed;
    private int _animIDAttack;
    private int _animIDDrawWeapon;
    private int _animIDSheathWeapon;
    private int _animIDLeaveCombo;
    private int _animLayerIndex;
    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();
        AssignAnimationIDs();
    }

    private void AssignAnimationIDs()
    {
        _animIDAttack = Animator.StringToHash("Attack");
        _animIDDrawWeapon = Animator.StringToHash("DrawWeapon");
        _animIDSheathWeapon = Animator.StringToHash("SheathWeapon");
        _animIDLeaveCombo = Animator.StringToHash("LeaveCombo");
    }

    // Update is called once per frame
    void Update()
    {
        SheathWeapon();
        DrawWeapon();
        if(_inCombat){
            Attack();
            ManageCombo();
            CheckDamage();
        }
    }

    private void Attack(){
        if(_input.attack){
            _animator.SetTrigger(_animIDAttack);
            //We look at the current value of Speed to determine in which animationLayer we are
            //If it's true it means we are moving which means we are in the arms layer, rather than the base combat layer
            if(_animator.GetBool("LockOn")){
                _animLayerIndex = 3;
            }else{
                _animLayerIndex = _animator.GetFloat("Speed") > 0.1 ? 2 : 1;
            }
            _timePassed = 0f;
            _attack = true;
            _input.attack = false;
        }
    }

    private void ManageCombo(){
        if(!_animator.GetCurrentAnimatorStateInfo(_animLayerIndex).IsTag("Attack")){
                _animator.ResetTrigger(_animIDLeaveCombo);
                return;
            }
        _timePassed += Time.deltaTime;
        
        _clipSpeed = _animator.GetCurrentAnimatorStateInfo(_animLayerIndex).speed;
        _clipLength = _animator.GetCurrentAnimatorClipInfo(_animLayerIndex)[0].clip.length;

        if(_timePassed >= _clipLength / _clipSpeed){
            if(_attack){
                _attack = false;
                Attack();
            }
            else{
                _animator.SetTrigger(_animIDLeaveCombo);
            }
        }
    }

    private void CheckDamage(){
        if(_currentVisualEffect == null) return;
        
    }

    private void DrawWeapon(){
        if(_input.drawWeapon && !_inCombat){
            _animator.SetTrigger(_animIDDrawWeapon);
            _inCombat = true;
            _input.drawWeapon = false;
            _input.attack = false;
        }
    }
    private void SheathWeapon(){
        if(_input.sheathWeapon && _inCombat){
            _animator.SetTrigger(_animIDSheathWeapon);
            _inCombat = false;
            _input.sheathWeapon = false;
        }
    }

    public void StartVisualEffect(int index){
        _currentVisualEffect = Instantiate(slashVFX[index], VFXHolder.transform);
        _currentVisualEffect.GetComponentInChildren<VisualEffect>().Play();
    }
    public void EndVisualEffect(){
        Destroy(_currentVisualEffect);
    }
}
