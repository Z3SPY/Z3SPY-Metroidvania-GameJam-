using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    [Header ("Movement Settings")]
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] float _speed = 10f;
    [SerializeField] bool _canJump = false;
    [SerializeField] bool _isGrounded = true;

    [Header ("JumpVal Settings")]
    [SerializeField] int jmpNum;
    [SerializeField] int maxJump = 1;
    float yVelocity;
    float downVelocityMax = 2f;
    float velocity = 0.42f;

    [Header ("Health Settings")]
    public int playerHP;
    [SerializeField] int maxHp = 5;
    [SerializeField] bool _isDead = false;
    [SerializeField] bool _invulnerable = false;
    public float delayDmg = 1.25f; 


    [Header ("References")]
    [SerializeField] Rigidbody2D RB2D;
    [SerializeField] jumpCheckScript jmpChkScript;
    [SerializeField] KnockbackScript knockbackRef;


    
    void Awake() {

        if (RB2D == null) RB2D = this.GetComponent<Rigidbody2D>();
        if (jmpChkScript == null) jmpChkScript = GameObject.Find("bottomCheck").GetComponent<jumpCheckScript>(); 
        if (knockbackRef == null) knockbackRef = this.GetComponent<KnockbackScript>();
        
    }

    void Start()
    {
        jmpNum = maxJump;
        yVelocity = downVelocityMax;
        playerHP = maxHp;

    }

    void Update()
    {

        jmpCheck();
        //Movement
        x = Input.GetAxisRaw("Horizontal");
        if (x != 0) 
            transform.Translate(new Vector3(x, 0, 0) * Time.deltaTime * _speed);

        if (Input.GetKeyDown(KeyCode.Space) && _canJump) {
            Jump();
        }

    }

    //When Messing With Physics
    void FixedUpdate() {
        fixedGravity();
    }   

#region jump and gravity 
    void fixedGravity() {

        if (_isGrounded == false) {
            
            yVelocity += velocity;
            RB2D.AddForce(new Vector2(0, -yVelocity * Time.fixedDeltaTime), ForceMode2D.Impulse);

           if (Input.GetKeyDown(KeyCode.Space) && _canJump) { yVelocity = downVelocityMax; }

        } else {
            yVelocity = downVelocityMax;
            
        }
    }

    void jmpCheck() {
        if (jmpNum > 0) {
            _canJump = true;
        } else {
            _canJump = false;
        }

        //While Grounded Script Is Not Activiated Else if In air Script is Activated
        if (_isGrounded == false) jmpChkScript.enabled = true;
        else jmpChkScript.enabled = false;
    }

    public void isGrounded(bool state) {
        _isGrounded = state;
    }

    public void resetJumps() {
        jmpNum = maxJump;
    }

    void Jump() {
        Debug.Log("Jump");
        _isGrounded = false;
        jmpNum--;
        RB2D.AddForce(new Vector2(0, 15), ForceMode2D.Impulse);
    }
#endregion

#region health
    void resetHp() {
        playerHP = maxHp;
    }

    public void takeDamage(int dmg) {
        playerHP -= dmg;
        _invulnerable = true;
        StartCoroutine(hitVulnerability());
    }

    void damageHandler() {
        if (playerHP <= 0) {
            _isDead = true;
            Debug.Log("Dead");
        }
    }
#endregion

#region Collision
    void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Enemy") && _invulnerable == false) {
                takeDamage(1);
                knockbackRef.PlayFeedback(other.gameObject);
                RB2D.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            }
        }

    IEnumerator hitVulnerability () {
        //While True
        while (_invulnerable == true) {
            yield return new WaitForSeconds(delayDmg);
            _invulnerable = false;
        } 
    }

#endregion



}
