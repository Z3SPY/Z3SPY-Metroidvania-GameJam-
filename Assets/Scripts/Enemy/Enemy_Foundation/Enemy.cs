using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EnemyTakeDamage))]
public class Enemy : MonoBehaviour 
{

    [SerializeField] EnemyTakeDamage damageReference;
    [SerializeField] GameObject deathParticleObject;
    [SerializeField] AudioSource enemyAudio;
    public AudioClip deathClip;

    bool _isAlive = true;
    public bool _canMove = true;

    //Reference To Spawner
    [SerializeField] enemySpawnControl spawner;
    [SerializeField] SpriteRenderer sr;
 

    void Awake() {
        damageReference = GetComponent<EnemyTakeDamage>();
        enemyAudio = GetComponent<AudioSource>();
        spawner = null;
    }

    public void setSpawner(GameObject sender) {
        spawner = sender.GetComponent<enemySpawnControl>();
       
    }

    void Start()
    {
    }

    void Update()
    {
        if (_isAlive == true) {
            this.gameObject.SetActive(true);
        } else {
            this.gameObject.SetActive(false);
        }
    }

    void FixedUpdate() {

    }


    #region Enemy Take Player Damage
        
        public void takeDamageEnemy(int dmg, GameObject sender) {
            damageReference.TakeDamage(dmg, sender);
            GameManager.instance.increaseToast();
        }
 
        public void Kill() {
            Instantiate(deathParticleObject, this.transform.position, Quaternion.identity);
            _isAlive = false;
            if (spawner != null) {
                spawner.enemyDied();
            }
            

            AudioManager.instance.setAudioShot(deathClip, 1f);
        }

        public void Alive() {
            _isAlive = true;
            _canMove = true;
            damageReference.healthReset(); //damage Reference Calls Health Reference in its Code
            sr.color = new Color(1f, 1f, 1f);
        }

        // Called in Events in Insepctor
        public void checkHit(bool state) {
            _canMove = state;
            if (_canMove == true) {
                sr.color = new Color(1f, 1f, 1f);
            } else {
                sr.color = new Color(1f, 0.30196078f, 0.30196078f);
            }
        }

        void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("FallDetection")) {
                Kill();
            }
        }


    #endregion 


}
