using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject virtualCam;
    public GameObject[] enemySpawnConatiner;
    public GameObject mainCheckPoint;


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !other.isTrigger) {
            //Camera Handler
            virtualCam.SetActive(true);

            //CheckPoint Handler
    
            if (mainCheckPoint != null) {
                other.GetComponent<playerScript>().getCheckPoint(mainCheckPoint.transform);
            }
        }
    }



    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player") && !other.isTrigger ) {
            //Handles Enemy Spawn
            foreach (GameObject item in enemySpawnConatiner)
            {
                if ((item.GetComponent<enemySpawnControl>() == true && item.GetComponent<enemySpawnControl>().hasSpawned == false) 
                || item.GetComponent<enemySpawnControl>().enemyHolder.activeSelf == false) {
                     item.GetComponent<enemySpawnControl>().SpawnPrefab();
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player") && !other.isTrigger) {
            virtualCam.SetActive(false);

            foreach (GameObject item in enemySpawnConatiner)
            {
                if (item.GetComponent<enemySpawnControl>() == true) {
                     item.GetComponent<enemySpawnControl>().leftRoom();
                }
            }

        }
    }
}
