using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    #region singleton
        public static bullet instance;
        void Awake() {
            instance = this;
            Invoke("DestroyBullet", 2.5f);
        }
    #endregion

    public float Speed = 10f;
    [SerializeField] float _dir = 1f;
    Rigidbody2D RB2D;


    public void SetDirection(string plyrDir) {
        if (plyrDir == "RIGHT") {
            _dir = 1;
            transform.Rotate(0f, 0f, 270f);
        } else if (plyrDir == "LEFT") {
            _dir = 1;
            transform.Rotate(0f, 0f, 90f);
        }

    }

    void Start() {
        RB2D = this.GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate() {
        transform.Translate(new Vector3(0, _dir, 0) * Time.fixedDeltaTime * Speed);
    } 


    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Wall") || other.CompareTag("Floor")) {
            DestroyBullet();
        } 

        if (other.CompareTag("Enemy")) {
            other.GetComponent<Enemy>().takeDamageEnemy(2, this.gameObject);
            DestroyBullet();
        }
    }

    void DestroyBullet() {
        //Play Animation
        //Destroy bullet
        Destroy(this.gameObject);
        _dir = 0f;
    }   
}
