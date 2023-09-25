using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour{

    [SerializeField] GameObject _enemy;
    [SerializeField] float _respawnDelay;

    Dictionary<Vector3, GameObject> _activeEnemies = new();
    void OnEnable(){
        Enemy.OnDeath += UpdateActiveEnemies;
    }

    private void OnDisable() {
        Enemy.OnDeath -= UpdateActiveEnemies;
    }

    private void Start() {
        var children = GetComponentsInChildren<Transform>();
        foreach (var item in children)
        {
            Debug.Log(item.name);
        }
        for(int i = 0; i < children.Length; i++){
            Debug.Log(children[i].position);
            GameObject obj = Instantiate(_enemy, children[i].position, Quaternion.identity, this.transform);
            _activeEnemies.Add(children[i].position, obj);
        }
    }

    private void Update() {
        
    }
    void UpdateActiveEnemies(GameObject enemy){
        foreach (var mushroom in _activeEnemies)
        {
            if(enemy == mushroom.Value){
                _activeEnemies[mushroom.Key] = null;
                StartCoroutine("StartRespawnTimer");
                return;
            }
        }
    }

    IEnumerator StartRespawnTimer(){
        float timePassed = 0f;
        while(timePassed < _respawnDelay){
            Debug.Log(timePassed);
            timePassed += Time.deltaTime;
            yield return null;
        }
        
        SpawnEnemy();
    }

    void SpawnEnemy(){
        Debug.Log("Spawning");
        foreach (var spawnPos in _activeEnemies){
            Debug.Log(spawnPos.Key +" "+ spawnPos.Value);
            if(spawnPos.Value == null){
                GameObject obj = Instantiate(_enemy, spawnPos.Key, Quaternion.identity, this.transform);
                _activeEnemies[spawnPos.Key] = obj;
                return;
            }
        }
    }
}
