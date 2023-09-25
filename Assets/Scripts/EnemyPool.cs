using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool instance; 

    private List<GameObject> pooledEnemies = new List<GameObject>();
    [SerializeField] private int _amountToPool;

    [SerializeField] private GameObject _enemy;

    private void Awake() {
        if(instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start(){
        for(int i = 0; i < _amountToPool; i++){
            GameObject obj = Instantiate(_enemy, transform.position, Quaternion.identity);
            obj.SetActive(false);
            pooledEnemies.Add(obj);
        }
    }

    
    public GameObject GetPooledEnemy(){
        for(int i = 0; i < pooledEnemies.Count; i++){
            if(!pooledEnemies[i].activeInHierarchy){
                return pooledEnemies[i];
            }
        }
        return null;
    }
}
