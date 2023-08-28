using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    public bool canDealDamage;
    public bool hasDealtDamage;
    public float attackRange, attackDamage;
    public float attackheight;
    private Vector3 _attackOrigin;
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
            _attackOrigin = transform.position;
            _attackOrigin.y += attackheight;
            if(Physics.Raycast(_attackOrigin, transform.forward, out hit, attackRange, layerMask)){
                Debug.Log("attack");
                if(hit.transform.TryGetComponent(out PlayerHealthSystem playerHealth)){
                    Debug.Log("attack hit");
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
        Vector3 attackOrigin = transform.position;
        attackOrigin.y += attackheight;
        Gizmos.DrawLine(attackOrigin, attackOrigin + transform.forward * attackRange);
    }
}
