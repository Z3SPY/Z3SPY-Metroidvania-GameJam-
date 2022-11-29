using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawnControl : MonoBehaviour
{
    #region Instance
        public static enemySpawnControl instance;

        void Awake() {
            instance = this;
        }
    #endregion


    [SerializeField] GameObject enemyContainer;
    [SerializeField] Enemy enemyReference;
    public GameObject enemyHolder;
    public bool hasSpawned = false;
    public bool _isAlive = false;
    

    
    // Reset Function for debugging
    public void resetLife() {
        if (_isAlive == false && hasSpawned == true) {
            _isAlive = true;
            SpawnPrefab();
            enemyReference.Alive();
        }
    }

    public void enemyDied() {
        _isAlive = false;
    }



    public void SpawnPrefab() {
        
        if (hasSpawned == false) {
            hasSpawned = true;
            enemyHolder = Instantiate(enemyContainer, this.transform.position, Quaternion.identity);
            enemyHolder.transform.parent = gameObject.transform;
            if (enemyHolder != null) {
                enemyReference = enemyHolder.GetComponent<Enemy>();
                enemyReference.setSpawner(this.gameObject);
            }
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
