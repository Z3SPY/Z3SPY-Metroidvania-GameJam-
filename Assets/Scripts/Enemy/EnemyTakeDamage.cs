using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EnemyHealth))]
public class EnemyTakeDamage : MonoBehaviour
{

    EnemyHealth healthReference;

    private void Awake() {
        healthReference = GetComponent<EnemyHealth>();
    }

    public void TakeDamage(int dmgAmt) {
        healthReference.ChangeHealth(dmgAmt);
    } 
}
