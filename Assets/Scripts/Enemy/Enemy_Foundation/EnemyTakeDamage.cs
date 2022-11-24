using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EnemyHealth))]
public class EnemyTakeDamage : MonoBehaviour
{

    EnemyHealth healthReference;
    float dmgDelay = 0.1f;
    bool _canTakeDmg = true;

    private void Awake() {
        healthReference = GetComponent<EnemyHealth>();
    }

    public void TakeDamage(int dmgAmt, GameObject sender) {
        if (_canTakeDmg == true) {
            healthReference.ChangeHealth(dmgAmt, sender);
            StartCoroutine(dmgCounter());
            print("Damage Enemy");
        }
        
    } 

    IEnumerator dmgCounter() {
        _canTakeDmg = false;
        yield return new WaitForSeconds(dmgDelay);
        _canTakeDmg = true;
    }

    public void healthReset() {
        Debug.Log("Reset");
        //Resets Variables
        _canTakeDmg = true;
        healthReference.ResetHealth();
    }
}
