using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] float health = 100f; 
    [SerializeField] GameObject onHitEffect; 

    Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damageAmount){
        health -= damageAmount;
        _animator.SetTrigger("TakeDamage");
        
        if(health <= 0){
            Die();
        }
    }

    void Die(){
        Destroy(this.gameObject);
    }

    public void SpawnHitEffect(Vector3 point){
        GameObject hitVFX = Instantiate(onHitEffect, this.transform);
        hitVFX.transform.position = point;
        Destroy(hitVFX, 5f);
    }
}
