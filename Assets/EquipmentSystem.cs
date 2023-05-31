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

    public void StartDealDamage(){
       currentWeaponInHand.GetComponentInChildren<DamageDealer>().StartDealDamage();
    }

    public void EndDealDamage(){
       currentWeaponInHand.GetComponentInChildren<DamageDealer>().EndDealDamage();
    }

    public void StartWeaponEffect(){
        currentWeaponInHand?.GetComponentInChildren<ParticleSystem>()?.Play(true);
    }

    public void StartVisualEffect(){
        GetComponentInChildren<VisualEffect>()?.Play();

    }

    public void EndWeaponEffect(){
        currentWeaponInHand.GetComponentInChildren<ParticleSystem>().Stop(true);
    }
}
