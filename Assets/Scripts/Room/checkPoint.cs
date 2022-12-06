using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    enum checkPointType {FALL, DEATH};
    [SerializeField] checkPointType checkType = checkPointType.FALL;
    Animator anim;
    ParticleSystem spark;
    bool _activated = false;

    public Transform mainCheckPoint;

    void Start() {
        if (checkType == checkPointType.FALL) {
            anim = null;
        } else {
            anim = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
            spark = this.gameObject.transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>(); 
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !other.isTrigger) {
            //CheckPoint Handler
            if (mainCheckPoint != null) {

                if (checkType == checkPointType.FALL) {
                    other.GetComponent<playerScript>().getCheckPoint(mainCheckPoint.transform);
                } else if (checkType == checkPointType.DEATH) {
                    other.GetComponent<playerScript>().getMainCheckPoint(mainCheckPoint.transform);
                    other.GetComponent<playerScript>().getCheckPointObject(this.gameObject);
                    if (_activated == false) {
                        spark.Play();
                        _activated = true;
                    }
                    anim.Play("Check-On");
                }
            }
        }
    }

  

    public void sparking() {
        spark.Play();
    }
}
