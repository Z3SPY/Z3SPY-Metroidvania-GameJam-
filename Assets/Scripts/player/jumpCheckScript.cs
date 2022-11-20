using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpCheckScript : MonoBehaviour
{
    
    public playerScript player;

    void Start() {
        player = GameObject.Find("Player").GetComponent<playerScript>();
    }
 
    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Floor")) {
            player.isGrounded(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        player.isGrounded(false);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Floor")) {
            player.resetJumps();
        }
    }


    
}
