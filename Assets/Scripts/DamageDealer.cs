using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] float weaponDamage;

    private void OnTriggerEnter(Collider other) {
        if(other.transform.TryGetComponent(out Enemy enemy)){
            enemy.TakeDamage(weaponDamage);
            enemy.SpawnHitEffect(other.ClosestPoint(transform.position));
        }
    }
    
}
