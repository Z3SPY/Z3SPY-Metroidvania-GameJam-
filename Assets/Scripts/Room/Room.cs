using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject virtualCam;
    public GameObject[] enemySpawnConatiner;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !other.isTrigger) {
            virtualCam.SetActive(true);
            foreach (GameObject item in enemySpawnConatiner)
            {
                if (item.GetComponent<enemySpawnControl>() == true) {
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
