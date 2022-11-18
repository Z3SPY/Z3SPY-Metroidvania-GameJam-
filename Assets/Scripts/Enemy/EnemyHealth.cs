using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;

    void Start() {
        ResetHealth();
    }

    public void ChangeHealth (int hpUpdate) {
        health += hpUpdate;
    }

    public void ResetHealth() {
        health = maxHealth;
    }

    

}
