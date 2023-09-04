using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Yarn.Unity;

public class EquipmentSystem : MonoBehaviour
{
    [SerializeField] List<GameObject> weaponHolders = new();
    [SerializeField] GameObject weaponSheath;
    [SerializeField] GameObject oldWeapon;
    [SerializeField] GameObject rustyWeapon;
    [SerializeField] GameObject questWeapon;
    
    GameObject weapon;
    GameObject weaponHolder;

    GameObject currentWeaponInHand;
    GameObject currentWeaponInSheath;

    PlayerCombat _playerCombat;

    // Start is called before the first frame update
    void Start()
    {
        _playerCombat = GetComponent<PlayerCombat>();
    }

    void DrawWeapon(){
        CheckForChildren(weaponHolder);
        currentWeaponInHand = Instantiate(weapon, weaponHolder.transform);
        CheckForChildren(weaponSheath);
    }
    void SheathWeapon(){
        CheckForChildren(weaponSheath);
        currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
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

    void ClearWeapons(){
        if(currentWeaponInHand != null)
            Destroy(currentWeaponInHand);
        if(currentWeaponInSheath != null)
            Destroy(currentWeaponInSheath);
    }

    public void getRustySword(){
        ClearWeapons();
        weapon = rustyWeapon;
        _playerCombat.currentWeapon = weapon;
        weaponHolder = weaponHolders[0];
        currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
        _playerCombat.indexStep = 0;
    }

    [YarnCommand("getOldSword")]
    public void getOldSword(){
        ClearWeapons();
        weapon = oldWeapon;
        _playerCombat.currentWeapon = weapon;
        weaponHolder = weaponHolders[1];
        currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
        _playerCombat.indexStep = 2;
    }

    [YarnCommand("getQuestSword")]
    public void getQuestSword(){
        ClearWeapons();
        weapon = questWeapon;
        _playerCombat.currentWeapon = weapon;
        weaponHolder = weaponHolders[2];
        currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
        _playerCombat.indexStep = 4;
    }

}
