using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            enemySpawnControl.instance.resetLife();
        }
    }
}
