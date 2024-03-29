﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region singleton
    public static GameManager instance;
    public bool plyrAlive = true;

    void Awake() {
        instance = this;
    }
    #endregion

    public float toastFloat = 1f;


    void Update()
    {
        
    }
    

    void FixedUpdate() {
        if (toastFloat <= 1f){ 
            toastFloat += (0.05f) * Time.fixedDeltaTime;
        } if (toastFloat > 1f) {
            toastFloat = 1f;
        }
    }

    public float GetToast() {
        return this.toastFloat;
    }

    public void resetToast() {
        toastFloat = 0f;
    }

    public void increaseToast() {
        toastFloat += 0.01f;
    }

    public void ResetEnemies() {
        enemySpawnControl.instance.resetLife();
    }

    public void playerDead() {
        plyrAlive = false;
    }

    public void playerAlive() {
        plyrAlive = true;
    }
}
