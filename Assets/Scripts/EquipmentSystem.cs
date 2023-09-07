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
    
    [SerializeField] List<AudioClip> audioClips = new();
    GameObject weapon;
    GameObject weaponHolder;

    GameObject currentWeaponInHand;
    GameObject currentWeaponInSheath;

    PlayerCombat _playerCombat;
    public DialogueVariableManager variableStorage;

    // Start is called before the first frame update
    void Start()
    {
        _playerCombat = GetComponent<PlayerCombat>();
        variableStorage = GameObject.FindWithTag("DVS").GetComponent<DialogueVariableManager>();
    }

    void DrawWeapon(){
        CheckForChildren(weaponHolder);
        currentWeaponInHand = Instantiate(weapon, weaponHolder.transform);
        AudioSource.PlayClipAtPoint(audioClips[0], this.transform.position);
        CheckForChildren(weaponSheath);
    }
    void SheathWeapon(){
        CheckForChildren(weaponSheath);
        currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
        CheckForChildren(weaponHolder);
    }

    void SheathAudio(){
        AudioSource.PlayClipAtPoint(audioClips[1], this.transform.position);
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
        if(currentWeaponInHand != null){
            _playerCombat.ForceSheatheWeapon();
            Destroy(currentWeaponInHand);
        }   
        if(currentWeaponInSheath != null)
            Destroy(currentWeaponInSheath);
    }

    public void getRustySword(){
        ClearWeapons();
        weapon = rustyWeapon;
        weaponHolder = weaponHolders[0];
        _playerCombat.indexStep = 0;
        EquipSword();
    }

    [YarnCommand("getOldSword")]
    public void getOldSword(){
        ClearWeapons();
        weapon = oldWeapon;
        weaponHolder = weaponHolders[1];
        _playerCombat.indexStep = 2;
        EquipSword();
    }

    [YarnCommand("getQuestSword")]
    public void getQuestSword(){
        ClearWeapons();
        weapon = questWeapon;
        weaponHolder = weaponHolders[2];
        _playerCombat.indexStep = 4;
        EquipSword();
    }

    public void EquipSword(){
        currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
        _playerCombat.currentWeapon = weapon;
        variableStorage.UpdateSword(true);
    }
}
