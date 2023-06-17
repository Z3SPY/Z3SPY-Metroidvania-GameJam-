using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndSwitch : MonoBehaviour
{

    public ParticleSystem particle, explodeOne, explodeTwo;
    public UnityEvent onActivate;

    public Sprite OnSprite;
    public SpriteRenderer sr; 


    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E)) {
            // Play End Game Animation 
            Invoke("playExplosion", 2f);
            particle.Stop();
            explodeOne.Play();
            explodeTwo.Play();
            sr.sprite = OnSprite;
        }
    }

    public void playExplosion() {
        onActivate?.Invoke();
        GameManager.instance.WinGame();
    }
}
