using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class playerScript : MonoBehaviour
{

    public string[] playerStates = {"Player-Idle", "Player-Attack", "Player-Walk", "Player-Jump", "Player-Air"};

    [Header ("Movement Settings")]
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] float _speed = 10f;
    [SerializeField] bool _canJump = false;
    [SerializeField] bool _isGrounded = true;
    public bool autoWalkBool = false;
    [SerializeField] bool _inTheAir = false;
    public float walk = 0f;

    [Header ("JumpVal Settings")]
    [SerializeField] int jmpNum;
    [SerializeField] int maxJump = 1;
    [SerializeField] bool _pressedJump = false;
    [SerializeField] bool _jumpAnimStart = false;
    [SerializeField] float jumpForce = 15f;

    float yVelocity;
    float downVelocityMax = 2f;
    float velocity = 0.42f;
  

    private float jumpTimeCounter;
    public float jumpTime;

    [Header ("Health Settings")]
    public int playerHP;
    [SerializeField] int maxHp = 5;
    [SerializeField] bool _isDead = false;
    [SerializeField] bool _invulnerable = false;
    public float delayDmg = 1.25f; 


    [Header ("Attack Settings")]
    [SerializeField] Transform attackOrigin;
    [SerializeField] Transform attackPos;
    [SerializeField] bool _canAttack = true;
    

    float attackRadius = 4f;
    [SerializeField] float attackDelay = .2f;

    public int attackDmg = 1;

    

    [Header ("References")]
    [SerializeField] Rigidbody2D RB2D;
    [SerializeField] jumpCheckScript jmpChkScript;
    [SerializeField] KnockbackScript knockbackRef;
    [SerializeField] SpriteRenderer srPlayer;
    public playerAnimatorController animatorController;



    
    void Awake() {

        if (RB2D == null) RB2D = this.GetComponent<Rigidbody2D>();
        if (jmpChkScript == null) jmpChkScript = GameObject.Find("bottomCheck").GetComponent<jumpCheckScript>(); 
        if (knockbackRef == null) knockbackRef = this.GetComponent<KnockbackScript>();
        if (attackOrigin == null) attackOrigin = GameObject.Find("AttackOrigin").GetComponent<Transform>();
        if (attackPos == null) attackPos = GameObject.Find("AttackPoint").GetComponent<Transform>();
        if (srPlayer == null) srPlayer = GameObject.Find("Player_visual").GetComponent<SpriteRenderer>();
        if (animatorController == null) animatorController = this.GetComponent<playerAnimatorController>();

    }

    void Start()
    {
        jmpNum = maxJump;
        yVelocity = downVelocityMax;
        playerHP = maxHp;

    }

    void Update()
    {   

        if (autoWalkBool == true) {
            autoWalk(walk);
        }

        jmpCheck();
        //Movement
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");


        if (autoWalkBool == false) {


            //Walk Handler
            if (_canAttack == true) {
                if (x != 0) {
                //Walk Animation
                    if (_isGrounded == true ) { animatorController.ChangeAnimationState(playerStates[2]); } 
                    transform.Translate(new Vector3(x, 0, 0) * Time.deltaTime * _speed);
                    PlayerFlip(x);
                } else {
                //Idle Animation
                    if (_isGrounded == true)
                    animatorController.ChangeAnimationState(playerStates[0]);
                }
            } else {
                if (x != 0) {
                    transform.Translate(new Vector3(x, 0, 0) * Time.deltaTime * _speed);
                }
                animatorController.ChangeAnimationState(playerStates[1]);
            }

            if (_canAttack == true && _isGrounded == false) {

                if (_jumpAnimStart == false) {
                    animatorController.ChangeAnimationState(playerStates[4]);
                } else {
                    animatorController.ChangeAnimationState(playerStates[3]);
                }
            }

                
            if (Input.GetKeyDown(KeyCode.Space) && _canJump) {

                if (Input.GetKey(KeyCode.S) == false) {
                    Jump();
                }
            }

            if (Input.GetKey(KeyCode.Space) && _pressedJump == true) {

                HoldJump();
                
            }


            if (y == 1) { attackOrigin.rotation = Quaternion.Euler(0, 0, 90); }
            if (y == -1) { attackOrigin.rotation = Quaternion.Euler(0, 0, 270); }
            


            if (Input.GetKeyDown(KeyCode.J)){
                if (_canAttack) {
                    attack();
                }
            }
       
        }
        

    }

    void canAttackChange() {
        _canAttack = false;
    }

    //When Messing With Physics
    void FixedUpdate() {
        fixedGravity();
    }   

    void PlayerFlip(float x) {
        float flipVal = x;
        if (flipVal == 1) { attackOrigin.rotation = Quaternion.Euler(0, 0, 0); srPlayer.flipX = false;} 
        if (flipVal == -1) { attackOrigin.rotation = Quaternion.Euler(0, 0, 180); srPlayer.flipX = true;}
    }
#region jump and gravity 
    void fixedGravity() {

        if (_isGrounded == false ) {
            
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


    void Jump() {
        if (_jumpAnimStart == false) {
            StartCoroutine(jumpDelay());
        }
        _pressedJump = true;
        Debug.Log("Jump");
        _isGrounded = false;
        jmpNum--;
        jumpTimeCounter = jumpTime;
        RB2D.velocity = Vector2.up * jumpForce;
        //RB2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    void HoldJump() {
        
        if (jumpTimeCounter > 0) {
            RB2D.velocity = Vector2.up * jumpForce;
            jumpTimeCounter -= Time.deltaTime;
        } else {
            _pressedJump = false;
        }
    }

    IEnumerator jumpDelay() {
        _jumpAnimStart = true;
        yield return new WaitForSeconds(0.4f);
        _jumpAnimStart = false;
    }


    //JUmp Checks
      public void isGrounded(bool state) {
        _isGrounded = state;
    }

    public void isAir(bool state) {
        _inTheAir = state;
    }

    public void resetJumps() {
        jmpNum = maxJump;
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

#region Attack
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPos.position, attackRadius);
    }

    void attack() {
        _canAttack = false;

        foreach ( Collider2D collider in Physics2D.OverlapCircleAll(attackPos.position, attackRadius))
        {
            if (collider.CompareTag("Enemy")) {
                collider.GetComponent<EnemyBasic>().takeDamageEnemy(attackDmg);
            }
            Debug.Log(collider.name);
        } 

        Invoke("changeAttack", attackDelay);

    }

    void changeAttack() {
        _canAttack = true;
    }

    
#endregion



#region Collision
    void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Enemy") && _invulnerable == false) {
                takeDamage(1);
                knockbackRef.PlayFeedback(other.gameObject);
                RB2D.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            }

            if (other.CompareTag("Room")) {
                autoWalkBool = false;
                walk = 0;
                transitionHandler.instance.transitionOut();
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


#region room State 
    public void setAutoWalk() {

        if (walk == 0 && autoWalkBool == false) {
            walk = x;
            autoWalkBool = true;
        } 
    }

    public void autoWalk(float dir) {
        animatorController.ChangeAnimationState(playerStates[2]);
        transform.Translate(new Vector3(dir, 0, 0) * Time.deltaTime * _speed);
    }
#endregion

#region platform interaction

    public void Down() {
            RB2D.AddForce(new Vector2(0, -15), ForceMode2D.Impulse);
    }
#endregion


}
