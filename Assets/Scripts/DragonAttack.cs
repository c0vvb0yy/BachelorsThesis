using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class DragonAttack : MonoBehaviour
{
    public float attackForce;
    public int attackDamage;
    public Transform tailTip;
    ThirdPersonController _player;
    PlayerHealthSystem _playerHealth;
    public bool isActive = true;
    // Start is called before the first frame update
    void Start(){
        var playerObject = GameObject.FindWithTag("Player");
        _player = playerObject.GetComponent<ThirdPersonController>();
        _playerHealth = playerObject.GetComponent<PlayerHealthSystem>();
    }

    public void Attack(){
        _player.KnockBack(tailTip.position, attackForce);
        _playerHealth.TakeDamage(this.gameObject, attackDamage);  
    }

    private void OnTriggerEnter(Collider other) {
        if(isActive && other.gameObject.tag == "Player"){
            Attack();
        }
    }
}
