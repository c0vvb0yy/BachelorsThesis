using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using StarterAssets;

public class PlayerCombat : MonoBehaviour
{

    [SerializeField] GameObject VFXHolder;
    [SerializeField] List<GameObject> slashVFX;
    [SerializeField] List<AudioClip> slashSFX;
    GameObject _currentVisualEffect;
    public GameObject currentWeapon;
    [HideInInspector]public int indexStep;

    private Animator _animator;
    private StarterAssetsInputs _input;
    private AudioSource _audio;
    private bool _inCombat = false;
    private bool _attack = false;
    private float _timePassed;
    private float _clipLength;
    private float _clipSpeed;
    private int _animIDAttack;
    private int _animIDDrawWeapon;
    private int _animIDSheathWeapon;
    private int _animIDLeaveCombo;
    private int _animLayerIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
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
        GetAnimLayerIndex();
        SheathWeapon();
        DrawWeapon();
        if(_inCombat){
            Attack();
            ManageCombo();
            CheckDamage();
        }
    }

    void GetAnimLayerIndex(){
        if(_animator.GetBool("LockOn")){
                _animLayerIndex = _animator.GetFloat("Speed") > 0.1 ? 4 : 3;
        }else{
            //We look at the current value of Speed to determine in which animationLayer we are
            //If it's true it means we are moving which means we are in the arms layer, rather than the base combat layer
            _animLayerIndex = _animator.GetFloat("Speed") > 0.1 ? 2 : 1;
        }
    }

    private void Attack(){
        if(_input.attack){
            _animator.SetTrigger(_animIDAttack);
            _timePassed = 0f;
            _attack = true;
            _input.attack = false;
            _input.drawWeapon = false;
            _input.sheathWeapon = false;
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
        if(currentWeapon != null && _input.drawWeapon && !_inCombat && EnsureSyncOn("Default")){
            _animator.SetTrigger(_animIDDrawWeapon);
            _inCombat = true;
            _input.attack = false;
        }
            _input.drawWeapon = false;
    }
    private void SheathWeapon(){
        if(currentWeapon != null && _input.sheathWeapon && _inCombat && EnsureSyncOn("CombatBlend")){
            _animator.SetTrigger(_animIDSheathWeapon);
            _inCombat = false;
            _input.drawWeapon = false;
        }
            _input.sheathWeapon = false;
    }

    public void ForceSheatheWeapon(){
        _animator.SetTrigger(_animIDSheathWeapon);
        _inCombat = false;
        _input.drawWeapon = false;
        _input.sheathWeapon = false;
    }

    private bool EnsureSyncOn(string tag){
        if(_animator.GetCurrentAnimatorStateInfo(1).IsTag(tag) 
        && _animator.GetCurrentAnimatorStateInfo(2).IsTag(tag)){
            return true;
        }
        return false;
    }

    public void StartVisualEffect(int index){
        _currentVisualEffect = Instantiate(slashVFX[index+indexStep], VFXHolder.transform);
        _currentVisualEffect.GetComponentInChildren<VisualEffect>().Play();
        _audio.clip = slashSFX[index];
        _audio.Play();
    }
    public void EndVisualEffect(){
        DestroyChildren(GetChildren(VFXHolder.transform));
    }

    GameObject[] GetChildren(Transform parent){
        GameObject[] allChildren = new GameObject[parent.childCount];
        int i = 0;
        foreach (Transform child in parent){
            allChildren[i] = child.gameObject;
            i++;
        }
        return allChildren;
    }

    void DestroyChildren(GameObject[] children){
        foreach (var child in children)
        {
            Destroy(child);
        }
    }
}
