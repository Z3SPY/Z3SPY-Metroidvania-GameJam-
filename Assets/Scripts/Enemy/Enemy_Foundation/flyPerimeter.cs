using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyPerimeter : MonoBehaviour
{
    public bool _inRange = false;


    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            _inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        _inRange = false;
    }

    public bool getBool() {
        return _inRange;
    }

}
