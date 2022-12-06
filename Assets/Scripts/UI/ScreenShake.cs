using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    

    [SerializeField] private CinemachineImpulseSource source;

    private void Awake() {
        source = GetComponent<CinemachineImpulseSource>();
    }

    
    /*void Start() {
        InvokeRepeating("ShockwaveEvent", 3f, 4f);
    }*/

    public void ShockwaveEvent() {
        source.GenerateImpulseAt(this.transform.position, Vector3.up);
        Debug.Log("Shaking");
    }

}
