using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    public bool canDealDamage;
    public bool hasDealtDamage;
    public float attackLength, attackDamage;
    // Start is called before the first frame update
    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(canDealDamage && !hasDealtDamage){
            RaycastHit hit;

            int layerMask = 1 << 8;
            if(Physics.Raycast(transform.position, transform.forward, out hit, attackLength, layerMask)){
                if(hit.transform.TryGetComponent(out PlayerHealthSystem playerHealth)){
                    playerHealth.TakeDamage(attackDamage);
                    playerHealth.SpawnHitEffect(hit.point);
                    hasDealtDamage = true;
                }
            }
        }
    }

    public void StartDealDamage(){
        canDealDamage = true;
        hasDealtDamage = false;
    }

    public void EndDealDamage(){
        canDealDamage = false;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * attackLength);
    }
}
