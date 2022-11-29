using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;

    public UnityEvent OnHit, OnDeath;

    


    void Start() {
        ResetHealth();
    }

    void Update() {
        
    }

    public void ChangeHealth (int hpUpdate, GameObject sender) {
        health -= hpUpdate;
        OnHit?.Invoke();

        if (health <= 0) {
            OnDeath?.Invoke();
        }
    }


    public void ResetHealth() {
        health = maxHealth;
    }

    

}
