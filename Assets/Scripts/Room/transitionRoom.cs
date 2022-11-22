using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transitionRoom : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<playerScript>().setAutoWalk();
            transitionHandler.instance.transitionIn();
        }
    }
}
