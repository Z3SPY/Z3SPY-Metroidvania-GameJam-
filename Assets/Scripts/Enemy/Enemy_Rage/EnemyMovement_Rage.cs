using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Enemy))]
public class EnemyMovement_Rage : MonoBehaviour
{   
    //States
    enum State {PATROL, RAGE, ALERT};
    [SerializeField] State enemyState = State.PATROL;
    public bool _canRage = true;


    //Rage Movement
    [SerializeField] CircleCollider2D circleAttack;
    Rigidbody2D RB2D;
    [SerializeField] float moveSpeed = 5;
    public bool _canMove = true;
    

    //Left And Right Postion
    const string LEFT = "left";
    const string RIGHT = "right";
    string facingDirection;

    //Line Renderer
    [SerializeField] Transform castPos;
    [SerializeField] Transform castPosRage;
    [SerializeField] Transform castPosRageBehind;
    [SerializeField] float baseCastDist = 0.5f;
    [SerializeField] float castDistPlayerDetection = 17f;

    //Line Renderer Detect
    [SerializeField] Vector3 playerPos;
    [SerializeField] bool _searching = false;

    //Reference to Enemy
    Enemy enemyReference; 
    GameObject Player;


    Vector3 baseScale;

    void Awake() {
        enemyReference = GetComponent<Enemy>();
        RB2D = GetComponent<Rigidbody2D>();
        castPos = this.gameObject.transform.GetChild(1);
        castPosRage = this.gameObject.transform.GetChild(2);
        castPosRageBehind = this.gameObject.transform.GetChild(2);
        Player = GameObject.Find("Player");
    }

    void Start()
    {        
        baseScale = transform.localScale;
        facingDirection = RIGHT;
    }   

    void Update() {
        _canMove =  enemyReference._canMove;

        if (enemyState == State.RAGE) {
            moveSpeed = 15f;
        } else {
            moveSpeed = 5f;
        }
    }

     void FixedUpdate() {

        Movement();
    }

    public void Movement() {
        float vX = (facingDirection == LEFT) ? -moveSpeed: moveSpeed;

        //Movement
        if (enemyState == State.PATROL) {
            if (_canMove == true) 
            RB2D.velocity = new Vector2(vX, RB2D.velocity.y);
            }   

            if (isHittingWall() || checkForEdge() ) {
                

                // Cannot Rage As soon as turn around Occurs to Prevent bug
                _canRage = false;
                if (_canRage == false) {
                    StartCoroutine(rageCoolDown(.75f));
                }
                if (facingDirection == LEFT) {
                    ChangeFacingDirection(RIGHT);
                } else if (facingDirection == RIGHT) {
                    ChangeFacingDirection(LEFT);
                }
                
                if (enemyState == State.RAGE) {
                    enemyState = State.PATROL;
                    _searching = false;

                } else {
                    circleAttack.enabled = false;
                }   

            }
            

        // Player Detection
        if ((isDetectingPlayer() == true ||  checkBehind() == true) && enemyState == State.PATROL) {
            if (_canRage == true) {
                enemyState = State.ALERT;
            }
        }

        if (enemyState == State.ALERT) {
            // Waits 1.25 seconds then Rage
            StartCoroutine(rageActivate());
        }

        if (enemyState == State.RAGE) {
            PlayerChase(moveSpeed);        
        } 
    }

    #region Basic Patrol
    void ChangeFacingDirection(string newDirection) {

        Vector3 newScale = baseScale;
        
        if (newDirection == LEFT) {
            newScale.x = -baseScale.x;
        } else if (newDirection == RIGHT) {
            newScale.x = baseScale.x;
        }

        transform.localScale = newScale;
        facingDirection = newDirection;

    }

    bool isHittingWall() {
        bool val = false;

        float castDist = baseCastDist;

        if (facingDirection == LEFT) {
            castDist = -baseCastDist;
        } else {
            castDist = baseCastDist;
        }

        //Determine the target destination based on the cast distance
        Vector3 targetPos = castPos.position;
        targetPos.x += castDist;

        //Draw Line
        Debug.DrawLine(castPos.position, targetPos, Color.green);


        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Terrain"))) {
            //We have found a wall
            val = true;
        } else {
            val = false;
        }

        return val;
    }

    bool checkForEdge() {
        bool val = true;

        float castDist = baseCastDist;

        //Determine the target destination based on the cast distance
        Vector3 targetPos = castPos.position;
        targetPos.y -= castDist;

        //Draw Line
        Debug.DrawLine(castPos.position, targetPos, Color.red);


        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Terrain"))) {
            //We have found no floor
            val = false;
        } else {
            val = true;
        }

        return val;
    }
    
    
    #endregion

    #region Player Check
        bool checkBehind() {
            bool val = false;

            

            float castDistReverse = castDistPlayerDetection - 12f;
            float castDist = castDistReverse; // Returns 5;

            if (facingDirection == RIGHT) {
                castDist = -castDistReverse;
            } else {
                castDist = castDistReverse;
            }

            Vector3 targetPos = castPosRageBehind.position;
            targetPos.x += castDist;

            Debug.DrawLine(castPosRageBehind.position, targetPos, Color.blue);

            if (Physics2D.Linecast(castPosRageBehind.position, targetPos, 1 << LayerMask.NameToLayer("Character"))) {
                
                if (_searching == false) {
                    playerPos = Player.transform.position;
                    _searching = true;
                }

                //We have found a Player
                val = true;
            } else {
                val = false;
            }

            return val;

        }

        bool isDetectingPlayer() {
            bool val = false;

            float castDist = castDistPlayerDetection;

            if (facingDirection == LEFT) {
                castDist = -castDistPlayerDetection;
            } else {
                castDist = castDistPlayerDetection;
            }


            //Determine the target destination based on the cast distance
            Vector3 targetPos = castPosRage.position;
            targetPos.x += castDist;

            //Draw Line
            Debug.DrawLine(castPosRage.position, targetPos, Color.yellow);
            
            if (Physics2D.Linecast(castPosRage.position, targetPos, 1 << LayerMask.NameToLayer("Character"))) {
                
                RaycastHit2D hit = Physics2D.Linecast(castPosRage.position, targetPos, 1 << LayerMask.NameToLayer("Character"));


                if (_searching == false) {
                    playerPos = hit.collider.gameObject.transform.position;
                    _searching = true;
                }

                //We have found a Player
                val = true;
            } else {
                val = false;
            }

            return val;
        }
    #endregion 


    #region Rage 
        public void PlayerChase(float vX) {

            Debug.Log("Raging");
            circleAttack.enabled = true;

            float dist = Vector2.Distance(this.transform.position, playerPos);  

            StartCoroutine(bugCheck());

            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(playerPos.x , this.transform.position.y, 0), moveSpeed * Time.fixedDeltaTime);
            

            //Debug.Log(playerPos);
            if (dist  <= 3f) {
                Debug.Log("Reached Destination");
                StartCoroutine(rageCoolDown(1.25f));
                _searching = false;
                enemyState = State.PATROL;
            }
        }

        IEnumerator rageActivate() {
            yield return new WaitForSeconds(1.25f);
            enemyState = State.RAGE;
            _canRage = false;

        }

        IEnumerator rageCoolDown(float cooldown) {
            yield return new WaitForSeconds(cooldown);
            _canRage = true;
        }

        IEnumerator bugCheck() {
            yield return new WaitForSeconds(1.5f);
            if (_searching == true && enemyState == State.RAGE) {
                _searching = false;
                enemyState = State.PATROL;
                _canRage = false;
                Debug.Log("Bug Fixed");
            } 

        }
       
    #endregion 

}
