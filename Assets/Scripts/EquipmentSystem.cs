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
        currentWeaponInHand = Instantiate(weapon, weaponHolder.transform);
        _playerCombat.currentWeapon = currentWeaponInHand;
        Destroy(currentWeaponInSheath);
    }

    void SheathWeapon(){
        currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
        _playerCombat.currentWeapon = null;
        Destroy(currentWeaponInHand);
    }

}
