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

    //Missing
    //Screen Shake
    //Screen Text


    public UnityEvent OnCollect;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            this.gameObject.SetActive(false);
            other.GetComponent<playerScript>()._obtainedDash = true;
            other.GetComponent<playerScript>().getMainCheckPoint(this.transform);
            OnCollect?.Invoke();
            particle.Stop();


            //Calls Set Text
            TextController.instance.SetText(msgEvent, msgCommand);
        }
    }
    

}
