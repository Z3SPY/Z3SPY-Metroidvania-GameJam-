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

    public void SpawnPrefab() {
        
        if (hasSpawned == false) {
            hasSpawned = true;
            enemyHolder = Instantiate(enemyContainer, this.transform.position, Quaternion.identity);
        } else {
           //Debug.Log(enemyHolder.transform.position);
            enemyHolder.SetActive(true);
           enemyHolder.transform.position = this.transform.position;
        }
        
        _isAlive = true;

    }

    public void leftRoom() {
        StartCoroutine(despawnDelay());
    }

    IEnumerator despawnDelay() {
        yield return new WaitForSeconds(1f);
        enemyHolder.SetActive(false);
    }

}
