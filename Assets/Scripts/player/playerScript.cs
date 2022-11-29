using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class playerScript : MonoBehaviour
{

    public string[] playerStates = {"Player-Idle", "Player-Attack", "Player-Walk", "Player-Jump", "Player-Air", "Player-Dash", "Player-Transform", "Player-Transform-Move", "Player-Shoot", "Player-Transform-Idle"};

    [Header ("Movement Settings")]
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] float _speed = 10f;
    [SerializeField] bool _canJump = false;
    [SerializeField] bool _isGrounded = true;
    public bool autoWalkBool = false;
    [SerializeField] bool _inTheAir = false;
    public float walk = 0f;
    public string direction = "RIGHT";
    private float prevX;

    [Header ("Metroid Vania Boot Components")]
    [SerializeField] bool _obtainedDash = false;
    [SerializeField] bool _obtainedCrouch = false;
    [SerializeField] bool _obtainedShoot = false;

    [Header("Dashing")]
    [SerializeField] float _dashingVelocity = 14f;
    [SerializeField] float _dashingTime = 0.1f;
    Vector2 _dashingDir;
    bool _isDashing;
    bool _canDash = true;

    
    [Header("Crouch Values")]
    [SerializeField] Transform castPos;
    float baseCastDist = 1f;
    [SerializeField] bool _canPressCrouch = true;
    [SerializeField] bool _isCrouching = false;

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
    public bool pressingHeal = false;
    [SerializeField] float healTime = 0f;


    [Header("Particle System")]
    public ParticleSystem dust;
    public ParticleSystem verticalDust;

    [Header ("Attack Settings")]
    [SerializeField] Transform attackOrigin;
    [SerializeField] Transform attackPos;
    [SerializeField] bool _canAttack = true;
    bool _isShooting = false;
    
    float attackRadius = 4f;
    [SerializeField] float attackDelay = .2f;
    public int attackDmg = 1;
    
    [Header("Shoot")]
    public GameObject bulletPreFab;


    [Header ("Misc")]
    [SerializeField] Transform checkPoint; // Room Check Point
    [SerializeField] BoxCollider2D playerHitBox;

    [Header ("References")]
    Rigidbody2D RB2D;
    jumpCheckScript jmpChkScript;
    KnockbackScript knockbackRef;
    SpriteRenderer srPlayer;
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

        //Reset Please Delete Later
        if (Input.GetKeyDown(KeyCode.R)) {
            this.transform.position = checkPoint.position;
        }

        if (_obtainedDash == true) {
            dashFunc();
        }

        if (_obtainedCrouch == true) {
            Crouch();
        }


        //Health
        if (playerHP <= 0) {
            _isDead = true;
            Debug.Log("Dead");
        }

        if (Input.GetKeyDown(KeyCode.K)) {
            pressingHeal = true;
        } 

        if (pressingHeal == true) {
            healTime += Time.deltaTime;

            if (healTime >= 3) {
                Heal();
                healTime = 0f;
            }
        }

        if (Input.GetKeyUp(KeyCode.K)) {
            if (healTime <= 0.75f && _obtainedShoot) {
                Shoot();
            }

            pressingHeal = false;
            healTime = 0f;
        }
        
       
    
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
                    if (_isGrounded ) {
                        if (_isDashing == false) {
                            if (_isCrouching == false ) {
                            animatorController.ChangeAnimationState(playerStates[2]);
                            } else {
                                animatorController.ChangeAnimationState(playerStates[7]);
                            }
                        }  
                    }

                    transform.Translate(new Vector3(x, 0, 0) * Time.deltaTime * _speed);
                    PlayerFlip(x);

                    //Set Previous X

                    if (_isDashing == false) {
                        if (x == 1f) {
                        prevX = 1f;
                        } else if (x == -1f) {
                            prevX = -1f;
                        }
                    }
                    

                } else {
                //Idle Animation
                    if (_isGrounded == true && _isDashing == false) {
                        if (_isCrouching == false) {
                            animatorController.ChangeAnimationState(playerStates[0]);
                        } else {
                            animatorController.ChangeAnimationState(playerStates[9]); //Crouching Idle
                        }
                    }
                }
            } else {
                if (x != 0) {
                    transform.Translate(new Vector3(x, 0, 0) * Time.deltaTime * _speed);
                }

                animatorController.ChangeAnimationState(playerStates[1]);
            }

            if (_canAttack == true && _isGrounded == false) {
                
                if (_isDashing == false) {
                    if (_jumpAnimStart == false) {
                        animatorController.ChangeAnimationState(playerStates[4]);
                    } else {
                        animatorController.ChangeAnimationState(playerStates[3]);
                    }
                }
                
            }

                
            if (Input.GetKeyDown(KeyCode.Space) && _canJump) {

                if (Input.GetKey(KeyCode.S) == false && _isGrounded == true) {
                    Jump();
                }
            }

            if (Input.GetKey(KeyCode.Space) && _pressedJump == true ) {

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
        if (_isDashing == false) {
            if (flipVal == 1) { attackOrigin.rotation = Quaternion.Euler(0, 0, 0); srPlayer.flipX = false; direction = "RIGHT";} 
            if (flipVal == -1) { attackOrigin.rotation = Quaternion.Euler(0, 0, 180); srPlayer.flipX = true; direction = "LEFT";}
        }   
        
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
        dust.Play(); // Particle Jump
        verticalDust.Play();

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
        if (state == true) {
            _canDash = true;
        } 
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
        HealthController.instance.updateHealth(playerHP);
    }

    public void Heal() {
        if (GameManager.instance.toastFloat >= 1f && playerHP < maxHp) {
            playerHP++;
            GameManager.instance.resetToast();
            HealthController.instance.updateHealth(playerHP);
        }
    }

    public void takeDamage(int dmg) {
        playerHP -= dmg;
        _invulnerable = true;
        HealthController.instance.updateHealth(playerHP);
        StartCoroutine(hitVulnerability());
    }

    void damageHandler() {
        if (playerHP <= 0) {
            _isDead = true;
            Debug.Log("Dead");
        }
    }

    public void Fall() {
        playerHP -= 1;
        StartCoroutine(FallingWait());
    }

    IEnumerator FallingWait() {
        yield return new WaitForSeconds(1.5f);
        this.transform.position = checkPoint.position;
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
                collider.GetComponent<Enemy>().takeDamageEnemy(attackDmg, this.gameObject);
            }
            //Debug.Log(collider.name);
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

            if (other.CompareTag("FallDetection")) {
                Fall();
                Debug.Log("Falling");
            }
        }

    IEnumerator hitVulnerability () {
        //While True
        while (_invulnerable == true) {
            StartCoroutine(hitVisual());
            yield return new WaitForSeconds(delayDmg);
            _invulnerable = false;
        } 
    }

    IEnumerator hitVisual() {
        srPlayer.color = new Color(1f, 0.30196078f, 0.30196078f);
        yield return new WaitForSeconds(delayDmg/3);
        srPlayer.color = new Color(1f, 1f, 1f);
        yield return new WaitForSeconds(delayDmg/3);
        srPlayer.color = new Color(1f, 0.30196078f, 0.30196078f);
        yield return new WaitForSeconds(delayDmg/3);
        srPlayer.color = new Color(1f, 1f, 1f);
    }

#endregion

#region room State 

    public void getCheckPoint(Transform roomCheck) {
        checkPoint = roomCheck;
    } 

    public void setAutoWalk() {

        if (walk == 0 && autoWalkBool == false) {
            walk = x;
            autoWalkBool = true;
        } 
    }

    public void autoWalk(float dir) {
        _isCrouching = false;
        animatorController.ChangeAnimationState(playerStates[2]);
        transform.Translate(new Vector3(dir, 0, 0) * Time.deltaTime * _speed);
    }
#endregion

#region platform interaction

    public void Down() {
            RB2D.AddForce(new Vector2(0, -5), ForceMode2D.Impulse);
    }
#endregion


#region metroid vania items
    #region dashing
        void dashFunc() {
            
            //Dashing 
            if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash) {
                _isDashing = true;
                _canDash = false;

                float dashX = (_isGrounded == true) ? prevX * 1.2f : prevX;

                
                _dashingDir = new Vector2(dashX, 0);

                if (_dashingDir == Vector2.zero) {
                    _dashingDir = new Vector2(transform.localScale.x, 0);
                }

                //Add Stopping Dash
                StartCoroutine(stopDashing());
            }

            if (_isDashing) {
                animatorController.ChangeAnimationState(playerStates[5]);
                RB2D.velocity = _dashingDir.normalized * _dashingVelocity;
                return;
            }

        }
    
        private IEnumerator stopDashing() {
            yield return new WaitForSeconds(_dashingTime);     
            _isDashing = false;
            RB2D.velocity = new Vector2 (0, RB2D.velocity.y);
        }

    #endregion

    #region crouching

        bool checkForWall() {
            bool val = false;

            float castDist = baseCastDist;

        
            //Determine the target destination based on the cast distance
            Vector3 targetPos = castPos.position;
            targetPos.y += castDist;

            //Draw Line
            Debug.DrawLine(castPos.position, targetPos, Color.green);

            if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Terrain"))) {
                //if find wall return false
                val = false;
            } else {
                val = true;
            }

            return val;
        }

        void Crouch() {


            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.J) ) {

                    if (_isCrouching == true && checkForWall()) {
                        _isCrouching = false;
                        playerHitBox.size = new Vector2(2.5f, 4f);
                        playerHitBox.offset = new Vector2(-0.2f, -0.65f);
                    }
                        
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && _canPressCrouch == true) {
                if (_isCrouching == false) {
                    animatorController.ChangeAnimationState(playerStates[6]);
                    _isCrouching = true;
                    playerHitBox.size = new Vector2(2.5f, 2f);
                    playerHitBox.offset = new Vector2(-0.2f, -1.5f);
                } else if (_isCrouching == true && checkForWall()) {
                    
                    _isCrouching = false;
                    playerHitBox.size = new Vector2(2.5f, 4f);
                    playerHitBox.offset = new Vector2(-0.2f, -0.65f);
                    
                }

                

                

                StartCoroutine(crouchCoolDown());
                
            } 
        }

        IEnumerator crouchCoolDown() {
            _canPressCrouch = false;
            yield return new WaitForSeconds(1.2f);
            _canPressCrouch = true;
        }
        
    #endregion

    #region shooting
        void Shoot() {

            if (GameManager.instance.toastFloat >= 1f) {
            
                animatorController.ChangeAnimationState(playerStates[8]);
                Instantiate(bulletPreFab, attackPos.transform.position, Quaternion.identity);
                GameManager.instance.resetToast();
                bullet.instance.SetDirection(direction);

                StartCoroutine(shootCoolDown());
            }
            
        }

        IEnumerator shootCoolDown() {
            _isShooting = true;
            yield return new WaitForSeconds(1.5f);
            _canPressCrouch = true;
        }
    #endregion
#endregion



}
