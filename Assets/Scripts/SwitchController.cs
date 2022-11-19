using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{   
        
    [SerializeField] private bool _closed = true;
    [SerializeField] private bool _canPress = true;


    public int id;

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (Input.GetKey(KeyCode.E)) {

                if (_canPress == true) {
                    _canPress = false;
                    StartCoroutine(doorUpdate());
                }
            }
        }
    }



    IEnumerator doorUpdate() {

        Debug.Log("Clicked");
        if (_closed == true) {
            _closed = false;
            GameEvents.current.DoorwayTriggerEnter(id);
        } else if (_closed == false) {
            _closed = true;
            GameEvents.current.DoorwayTriggerClose(id);
        }

        yield return new WaitForSeconds(1.2f);
        _canPress = true;

    }
    
}
