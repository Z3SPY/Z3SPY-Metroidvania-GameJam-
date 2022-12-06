using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapController : MonoBehaviour
{

    public float interval = 1f;
    public float offInterval = 1f;
    public bool trapStart = false;
    [SerializeField] Animator anim;


    void Start() {
        TrapOn();
    }

    public void TrapOn() {
        trapStart = true;
        anim.Play("Trap");
        Invoke("TrapOff", interval);
    }

    public void TrapOff() {
        trapStart = false;
        anim.Play("Trapoff");
        Invoke("TrapOn", offInterval);
    }
}
