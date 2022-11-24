using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Enemy))]
public class EnemyMovement_Rage : MonoBehaviour
{   

    enum State {PATROL, RAGE};
    [SerializeField] State enemyState = State.PATROL;


    Rigidbody2D RB2D;
    [SerializeField] float moveSpeed = 5;
    
    public bool _canMove = true;
    bool _hitEnemy = false;


    //Left And Right Postion
    const string LEFT = "left";
    const string RIGHT = "right";

    string facingDirection;

    //Line Renderer
    [SerializeField] Transform castPos;
    [SerializeField] Transform castPosRage;
    [SerializeField] float baseCastDist = 0.5f;
    [SerializeField] float castDistPlayerDetection = 17f;

    //Reference to Enemy
    Enemy enemyReference; 


    Vector3 baseScale;

    void Awake() {
        enemyReference = GetComponent<Enemy>();
        RB2D = GetComponent<Rigidbody2D>();
        castPos = this.gameObject.transform.GetChild(1);
        castPosRage = this.gameObject.transform.GetChild(2);
    }

    void Start()
    {        
        baseScale = transform.localScale;
        facingDirection = RIGHT;
    }   

    void Update() {
        _canMove =  enemyReference._canMove;
    }

     void FixedUpdate() {

        if (enemyState == State.PATROL) {
            Movement();
        }
    }

    public void Movement() {
        float vX = (facingDirection == LEFT) ? -moveSpeed: moveSpeed;

        //Movement
        if (_canMove == true) 
        RB2D.velocity = new Vector2(vX, RB2D.velocity.y);

        if (isHittingWall() || checkForEdge() || _hitEnemy == true ) {

            if (facingDirection == LEFT) {
                ChangeFacingDirection(RIGHT);
            } else if (facingDirection == RIGHT) {
                ChangeFacingDirection(LEFT);
            }
        }


        // Player Detection
        if (isDetectingPlayer() == true) {enemyState = State.RAGE; };
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
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            _hitEnemy = true;
        }
        else _hitEnemy = false;
    }
    #endregion

    #region Player Check
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
                //We have found a Player
                val = true;
            } else {
                val = false;
            }

            return val;
        }
    #endregion 


}
