using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EnemyTakeDamage))]
public class Enemy : MonoBehaviour 
{
    [SerializeField] EnemyTakeDamage damageReference;

    bool _isAlive = true;
    bool _exitRoom = false;
    public bool _canMove = true;

    //Reference To Spawner
    [SerializeField] enemySpawnControl spawner;
    [SerializeField] SpriteRenderer sr;
 

    void Awake() {
        damageReference = GetComponent<EnemyTakeDamage>();
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
        }
 
        public void Kill() {
            _isAlive = false;

            if (spawner != null) {
                spawner.enemyDied();
            }
        }

        public void Alive() {
            _isAlive = true;
            _canMove = true;
            damageReference.healthReset(); //damage Reference Calls Health Reference in its Code
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


    #endregion 


}
