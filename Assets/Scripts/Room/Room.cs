using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    enum roomType {Room , Transition};   
    [SerializeField] roomType RoomType = roomType.Room;

    public GameObject virtualCam = null;
    public GameObject[] enemySpawnConatiner;
    //public GameObject mainCheckPoint;


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !other.isTrigger) {
            //Camera Handler

            if ((RoomType == roomType.Room || RoomType == roomType.Transition) && virtualCam != null) {
                virtualCam.SetActive(true);
                other.GetComponent<playerScript>().setCameraReference(virtualCam);
            } 

                    
            //CheckPoint Handler
    
            /*if (mainCheckPoint != null) {
                other.GetComponent<playerScript>().getCheckPoint(mainCheckPoint.transform);
            }*/
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

            if (virtualCam != null)
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
