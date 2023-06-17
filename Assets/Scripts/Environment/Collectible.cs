using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Collectible : MonoBehaviour
{
    public string Type;     
    [SerializeField] bool _startAnimation = false;
    public bool _collected = false;
    public ParticleSystem particle;
    public string msgEvent, msgCommand;
    [SerializeField] SpriteRenderer sr; 
    public Sprite itemSprite;

    //Missing
    //Screen Shake
    //Screen Text


    public UnityEvent OnCollect;

    void Start() {
        sr.sprite = itemSprite;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            
            sr.sprite = null;
            Invoke("waitForCollect", 1f);
            switch (msgEvent)
            {
                case "OBTAINED DASH":
                    other.GetComponent<playerScript>()._obtainedDash = true;
                    break;
                case "OBTAINED DOUBLE JUMP":
                    other.GetComponent<playerScript>()._obtainedDoubleJump = true;
                    break;
                case "OBTAINED TRANSFORM":
                    other.GetComponent<playerScript>()._obtainedCrouch = true;
                    break;
                default:
                    break;
            }
            
            other.GetComponent<playerScript>().getMainCheckPoint(this.transform);
            OnCollect?.Invoke();
            particle.Stop();


            //Calls Set Text
            TextController.instance.SetText(msgEvent, msgCommand);
        }
    }

    public void waitForCollect() {
        this.gameObject.SetActive(false);
    }
    

}
