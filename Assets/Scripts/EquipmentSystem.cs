using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EquipmentSystem : MonoBehaviour
{
    [SerializeField] GameObject weaponHolder;
    [SerializeField] GameObject weaponSheath;
    [SerializeField] GameObject weapon;

    [SerializeField] GameObject VFXHolder;
    [SerializeField] List<GameObject> slashVFX;
    
    GameObject currentWeaponInHand;
    GameObject currentWeaponInSheath;
    GameObject currentVisualEffect;

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

    public void StartVisualEffect(int index){
        currentVisualEffect = Instantiate(slashVFX[index], VFXHolder.transform);
        currentVisualEffect.GetComponentInChildren<VisualEffect>().Play();
    }
    public void EndVisualEffect(){
        Destroy(currentVisualEffect);
    }

    public void EndWeaponEffect(){
        currentWeaponInHand.GetComponentInChildren<ParticleSystem>().Stop(true);
    }
}
