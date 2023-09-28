using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] float weaponDamage;

    private void OnTriggerEnter(Collider other) {
        if(other.transform.TryGetComponent(out Enemy enemy)){
            enemy.TakeDamage(weaponDamage, other.ClosestPoint(transform.position));
        }else if(other.transform.TryGetComponent(out Obelisk obelisk)){
            obelisk.Activate();
        }
    }
    
}
