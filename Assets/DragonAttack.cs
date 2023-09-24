using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class DragonAttack : MonoBehaviour
{
    public float attackForce;
    public bool tailConnected;
    ThirdPersonController _player;
    // Start is called before the first frame update
    void Start(){
        _player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();
    }

    // Update is called once per frame
    void Update(){
        
    }
    public void Attack(){
        Debug.Log("knockback");
        
        _player.KnockBack(transform.position, attackForce);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Trigger enter");
        if(other.gameObject.tag == "Player"){
            Attack();
        }
    }
}
