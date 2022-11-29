using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    public int id;
    [SerializeField] Transform point1, point2;
    public float speedWhenOn = 0.75f, speenWhenOff = 2f;

    void Start()
    {
        GameEvents.current.onDoorwayTriggerEnter += OnDoorwayOpen;
        GameEvents.current.onDoorwayTriggerClose += OnDoorwayClose;

    }

    private void OnDoorwayClose(int id) {

        if (id == this.id) {
            LeanTween.moveLocalY(gameObject, point1.position.y, speedWhenOn).setEaseInQuad();
            Debug.Log("Closing");
        }
    }

    private void OnDoorwayOpen(int id) {

        if (id == this.id) {
            LeanTween.moveLocalY(gameObject, point2.position.y, speenWhenOff).setEaseOutQuad();
            Debug.Log("Opening");
        }
    }
}
