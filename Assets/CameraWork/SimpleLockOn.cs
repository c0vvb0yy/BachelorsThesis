using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLockOn : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] bool mirror;
    void OnEnable(){
        if(target == null) target = Camera.main.transform;
        StartCoroutine(LookAtTarget());
    }

    private IEnumerator LookAtTarget(){
        while(this.gameObject.activeInHierarchy){
            Vector3 _dir = Vector3.zero;
            if(mirror) _dir = 2 * transform.position - target.position;
            else _dir = target.position - transform.position;
            
            transform.rotation = Quaternion.LookRotation(_dir);
            yield return null;
        }
    }
}
