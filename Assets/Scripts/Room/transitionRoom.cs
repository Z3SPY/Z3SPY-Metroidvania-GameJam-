using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transitionRoom : MonoBehaviour
{
    
    enum Type {HORIZONTAL , VERTICAL, TELEPORT};    

    [SerializeField] Type transitionType = Type.HORIZONTAL;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {

            if (transitionType == Type.HORIZONTAL) {
                other.GetComponent<playerScript>().setAutoWalk();
            }
            transitionHandler.instance.transitionIn();
        }
        if (other.CompareTag("Enemy")) {
            other.GetComponent<Enemy>().Kill();
        }
    }
}
