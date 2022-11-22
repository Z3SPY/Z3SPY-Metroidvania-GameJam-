using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimatorController : MonoBehaviour
{
   Animator animator;  
   private string currentState;


   void Start() {
        animator = GameObject.Find("Player_visual").GetComponent<Animator>();
   }

   public void ChangeAnimationState(string newState) {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
   }
}
