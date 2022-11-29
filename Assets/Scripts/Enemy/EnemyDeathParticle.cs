using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathParticle : MonoBehaviour
{

    [Header("Particle System")]
    public ParticleSystem deathParticle;

    void Awake() {
        deathParticle.Play();
        Invoke("DestroyParticle", 1.5f);
    }

    void DestroyParticle() {
        Destroy(this.gameObject);
    }
}
