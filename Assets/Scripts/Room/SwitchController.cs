using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{   
        
    [SerializeField] private bool _closed = true;
    [SerializeField] private bool _canPress = true;
    Animator anim;


    public int id;

    void Awake() {
        anim = GetComponent<Animator>();
    }

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
            anim.Play("Switch-On");
            GameEvents.current.DoorwayTriggerEnter(id);
        } else if (_closed == false) {
            _closed = true;
            anim.Play("Switch");
            GameEvents.current.DoorwayTriggerClose(id);
        }

        yield return new WaitForSeconds(1.2f);
        _canPress = true;

    }
    
}
