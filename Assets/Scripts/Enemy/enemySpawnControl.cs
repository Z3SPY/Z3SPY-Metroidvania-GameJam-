using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawnControl : MonoBehaviour
{
    [SerializeField] GameObject enemyContainer;
    public bool hasSpawned = false;
    public bool _isAlive = false;
    GameObject enemyHolder;

 
    void Awake() {
    }

    void SpawnPrefab() {
        
        if (hasSpawned == false) {
            hasSpawned = true;
            enemyHolder = Instantiate(enemyContainer, this.transform.position, Quaternion.identity);
        } else {
           //Debug.Log(enemyHolder.transform.position);
           enemyHolder.transform.position = this.transform.position;
        }
        
        _isAlive = true;

    }


    void Update() {
        if (Input.GetKeyDown(KeyCode.O)) {
            SpawnPrefab();
        }
    }
}
