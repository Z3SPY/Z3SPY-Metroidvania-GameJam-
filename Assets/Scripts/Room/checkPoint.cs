using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    public Transform mainCheckPoint;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !other.isTrigger) {
            //CheckPoint Handler
            if (mainCheckPoint != null) {
                other.GetComponent<playerScript>().getCheckPoint(mainCheckPoint.transform);
            }
        }
    }
}
