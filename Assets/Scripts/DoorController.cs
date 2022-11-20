using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    public int id;

    void Start()
    {
        GameEvents.current.onDoorwayTriggerEnter += OnDoorwayOpen;
        GameEvents.current.onDoorwayTriggerClose += OnDoorwayClose;

    }

    private void OnDoorwayClose(int id) {

        if (id == this.id) {
            LeanTween.moveLocalY(gameObject, -7.6f, 1f).setEaseInQuad();
            Debug.Log("Closing");
        }
    }

    private void OnDoorwayOpen(int id) {

        if (id == this.id) {
            LeanTween.moveLocalY(gameObject, 1.6f, 1f).setEaseOutQuad();
        }
    }
}
