using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DamageDealer : MonoBehaviour
{
    public bool canDealDamage;
    List<GameObject> hasDealtDamage;


    [SerializeField] float weaponDamage;
    // Start is called before the first frame update
    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();
    }

    public void StartDealDamage(){
        canDealDamage = true;
        hasDealtDamage.Clear();
    }

    public void EndDealDamage(){
        canDealDamage = false;
    }

    private void OnTriggerEnter(Collider other) {
        if(!canDealDamage || hasDealtDamage.Contains(other.transform.gameObject)) return;
        if(other.transform.TryGetComponent(out Enemy enemy)){
            hasDealtDamage.Add(other.transform.gameObject);
            enemy.TakeDamage(weaponDamage);
            enemy.SpawnHitEffect(other.ClosestPoint(transform.position));
        }
    }
}
