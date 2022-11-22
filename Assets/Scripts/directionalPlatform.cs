using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class directionalPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float waitTime;
    public bool _startWait = false;

    void Start() {
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update() {

        if(Input.GetKeyUp(KeyCode.S)) {
            waitTime = 0.5f;
        }

        if (Input.GetKey(KeyCode.S) ) {
            if (waitTime <= 0) {
                effector.rotationalOffset = 180f;
                waitTime = 0.5f;
            } else {
                waitTime -= Time.deltaTime;
            }
        }


        if (Input.GetKeyDown(KeyCode.Space)) {
            effector.rotationalOffset = 0f;
        }
    }
 
}
