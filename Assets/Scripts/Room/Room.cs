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

            //Handles Enemy Spawn
            foreach (GameObject item in enemySpawnConatiner)
            {
                if (item.GetComponent<enemySpawnControl>() == true) {
                     item.GetComponent<enemySpawnControl>().SpawnPrefab();
                }
            }

            //CheckPoint Handler
            other.GetComponent<playerScript>().getCheckPoint(mainCheckPoint.transform);
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
