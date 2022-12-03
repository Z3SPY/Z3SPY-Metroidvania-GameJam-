using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transitionRoom : MonoBehaviour
{
    
    enum Type {HORIZONTAL , VERTICAL, TELEPORT, FALL};    

    [SerializeField] Type transitionType = Type.HORIZONTAL;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {

            if (transitionType == Type.HORIZONTAL) {
                other.GetComponent<playerScript>().setAutoWalk();
            }

            if (transitionType == Type.VERTICAL) {
                other.GetComponent<playerScript>().setAutoJump();
            }

            if (transitionType == Type.FALL) {
                if (other.GetComponent<playerScript>().autoJumpBool == false)
                other.GetComponent<playerScript>().autoFall();
            }
            transitionHandler.instance.transitionIn();
        }
        if (other.CompareTag("Enemy")) {
            other.GetComponent<Enemy>().Kill();
        }
    }
}
