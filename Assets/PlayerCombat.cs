using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PlayerCombat : MonoBehaviour
{

    private Animator _animator;
    private StarterAssetsInputs _input;
    private bool _inCombat = false;
    private bool _attack = false;
    private bool _hasJustAttacked = false;
    private float _timePassed;
    private float _clipLength;
    private float _clipSpeed;
    private int _animIDAttack;
    private int _animIDDrawWeapon;
    private int _animIDSheathWeapon;
    private int _animIDLeaveCombo;
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
            if(!_animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack")){
                _animator.ResetTrigger(_animIDLeaveCombo);
                return;
            }
            _timePassed += Time.deltaTime;
            _clipLength = _animator.GetCurrentAnimatorClipInfo(1)[0].clip.length;
            _clipSpeed = _animator.GetCurrentAnimatorStateInfo(1).speed;

            if(_timePassed >= _clipLength/ _clipSpeed){
                if(_attack){
                    _attack = false;
                    Attack();
                }
                else{
                    _animator.SetTrigger(_animIDLeaveCombo);
                }
            }
        }
    }

    private void Attack(){
        if(_input.attack){
            _animator.SetTrigger(_animIDAttack);
            _timePassed = 0f;
            _attack = true;
            _hasJustAttacked = false;
            _input.attack = false;
        }
    }

    private void DrawWeapon(){
        if(_input.drawWeapon && !_inCombat){
            Debug.Log("drawing weapon");
            _animator.SetTrigger(_animIDDrawWeapon);
            _inCombat = true;
            _input.drawWeapon = false;
        }
    }
    private void SheathWeapon(){
        if(_input.sheathWeapon && _inCombat){
            Debug.Log("sheathing weapon");
            _animator.SetTrigger(_animIDSheathWeapon);
            _inCombat = false;
            _input.sheathWeapon = false;
        }
    }
}
