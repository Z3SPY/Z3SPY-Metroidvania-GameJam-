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
        if (health <= 0) {
            OnDeath?.Invoke();
        }
    }

    public void ChangeHealth (int hpUpdate) {
        health -= hpUpdate;
        OnHit?.Invoke();
    }


    public void ResetHealth() {
        health = maxHealth;
    }

    

}
