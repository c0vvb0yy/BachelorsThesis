using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EquipmentSystem : MonoBehaviour
{
    [SerializeField] GameObject weaponHolder;
    [SerializeField] GameObject weaponSheath;
    [SerializeField] GameObject weapon;

    
    
    GameObject currentWeaponInHand;
    GameObject currentWeaponInSheath;

    PlayerCombat _playerCombat;

    // Start is called before the first frame update
    void Start()
    {
        currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
        _playerCombat = GetComponent<PlayerCombat>();
    }

    void DrawWeapon(){
        CheckForChildren(weaponHolder);
        currentWeaponInHand = Instantiate(weapon, weaponHolder.transform);
        _playerCombat.currentWeapon = currentWeaponInHand;
        //Destroy(currentWeaponInSheath);
        CheckForChildren(weaponSheath);
    }
    void SheathWeapon(){
        CheckForChildren(weaponSheath);
        currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
        _playerCombat.currentWeapon = null;
        //Destroy(currentWeaponInHand);
        CheckForChildren(weaponHolder);
    }

    void CheckForChildren(GameObject weaponPosition){
        if(weaponPosition.transform.childCount > 0){
            DestroyChildren(GetChildren(weaponPosition.transform));
        } else {
            if(currentWeaponInHand == null)
                Destroy(currentWeaponInHand); 
            else
                Destroy(currentWeaponInSheath);
        }
    }

    GameObject[] GetChildren(Transform parent){
        GameObject[] allChildren = new GameObject[parent.childCount];
        int i = 0;
        foreach (Transform child in parent){
            allChildren[i] = child.gameObject;
            i++;
        }
        return allChildren;
    }

    void DestroyChildren(GameObject[] children){
        foreach (var child in children)
        {
            Destroy(child);
        }
    }


}
