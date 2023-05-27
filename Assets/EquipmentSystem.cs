using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    [SerializeField] GameObject weaponHolder;
    [SerializeField] GameObject weaponSheath;
    [SerializeField] GameObject weapon;
    
    GameObject currentWeaponInHand;
    GameObject currentWeaponInSheath;
    

    // Start is called before the first frame update
    void Start()
    {
        currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
    }

    void DrawWeapon(){
        currentWeaponInHand = Instantiate(weapon, weaponHolder.transform);
        Destroy(currentWeaponInSheath);
    }

    void SheathWeapon(){
        currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
        Destroy(currentWeaponInHand);
    }
}
