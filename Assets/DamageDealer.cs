using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public bool canDealDamage;
    List<GameObject> hasDealtDamage;

    [SerializeField] float weaponLength, weaponDamage;
    // Start is called before the first frame update
    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canDealDamage){
            RaycastHit hit;

            int layerMask = 1 << 7;
            if(Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask)){
                if(!hasDealtDamage.Contains(hit.transform.gameObject)){
                    Debug.Log("damage");
                    hasDealtDamage.Add(hit.transform.gameObject);
                }
            }
        }
    }

    public void StartDealDamage(){
        canDealDamage = true;
        hasDealtDamage.Clear();
    }

    public void EndDealDamage(){
        canDealDamage = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
