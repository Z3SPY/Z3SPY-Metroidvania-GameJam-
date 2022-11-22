using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transitionHandler : MonoBehaviour
{   


    [SerializeField] GameObject transitionHolder;
    [SerializeField] Animator transitionAnimator;

    public static transitionHandler instance;

    void Awake() {
        instance = this;
        transitionHolder.SetActive(true);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            transitionIn();
        } 

         if (Input.GetKeyDown(KeyCode.L)) {
            transitionOut();
        } 
    }

    public void transitionIn() {
        transitionAnimator.Play("Transition-in");
    }

    public void transitionOut() {
        transitionAnimator.Play("Transition-out");
    }
}
