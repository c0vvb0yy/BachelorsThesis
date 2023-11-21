using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingDamageNumber : MonoBehaviour
{
    public GameObject damageNumberPrefab;
    
    public void ShowDamageNumber(float damageAmount){
        var number = Instantiate(damageNumberPrefab, transform.position, Quaternion.identity, transform);
        number.transform.position = RecalibratePos(number, 1f);
        number.GetComponent<TextMeshPro>().text = damageAmount.ToString();
        StartTweens(number);
    }
    
    private Vector3 RecalibratePos(GameObject number, float heightAdjustment){
        var pos = number.transform.position;
        pos = new Vector3(pos.x, pos.y, pos.z+heightAdjustment);
        return pos;
    }
    private void StartTweens(GameObject number){
        number.LeanMoveLocalX(Random.Range(-1.5f, 1.5f), 1f);
        number.LeanScale(Vector3.one, 1f).setEaseOutBounce();
        number.LeanScale(Vector3.zero, 1f).setEaseInBounce().setDelay(3f);
        Destroy(number, 5f);
    }
}
