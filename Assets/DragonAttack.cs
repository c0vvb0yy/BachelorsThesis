using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class DragonAttack : MonoBehaviour
{
    public float attackForce;
    public Transform tailTip;
    ThirdPersonController _player;
    public bool isActive = true;
    // Start is called before the first frame update
    void Start(){
        _player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();
    }

    public void Attack(){
        Debug.Log("knockback");
        
        _player.KnockBack(tailTip.position, attackForce);
        
    }

    private void OnTriggerEnter(Collider other) {
        if(isActive && other.gameObject.tag == "Player"){
            Attack();
        }
    }
}
