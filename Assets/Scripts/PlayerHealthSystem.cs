using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f; 
    private float _currentHealth;
    [SerializeField] GameObject onHitEffect; 
    [SerializeField] Healthbar healthBar; 
    [SerializeField] AudioClip onHitSound;
    

    Animator _animator;
    AudioSource _audio;
    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = maxHealth;
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damageAmount){
        _currentHealth -= damageAmount;
        healthBar.UpdateHealthbar(maxHealth, _currentHealth);
        _animator.SetTrigger("TakeDamage");
        
        if(_currentHealth <= 0){
            Die();
        }
    }

    void Die(){
        Destroy(this.gameObject);
    }

    public void SpawnHitEffect(Vector3 point){
        GameObject hitVFX = Instantiate(onHitEffect, this.transform);
        hitVFX.transform.position = point;
        _audio.clip = onHitSound;
        _audio.Play();
        Destroy(hitVFX, 2f);
    }
}
