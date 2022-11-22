using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EnemyTakeDamage))]
public class EnemyBasic : MonoBehaviour 
{

    [SerializeField] EnemyTakeDamage damageReference;

    Rigidbody2D RB2D;
    [SerializeField] float moveSpeed = 5;

    bool _isAlive = true;


    //Left And Right Postion
    const string LEFT = "left";
    const string RIGHT = "right";

    string facingDirection;

    //Line Renderer
    [SerializeField] Transform castPos;
    [SerializeField] float baseCastDist = 0.5f;

    Vector3 baseScale;

    void Awake() {
        damageReference = GetComponent<EnemyTakeDamage>();
        RB2D = GetComponent<Rigidbody2D>();
        castPos = GameObject.Find("CastPos").GetComponent<Transform>();
    }

    void Start()
    {
        baseScale = transform.localScale;
        facingDirection = RIGHT;
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

        float vX = (facingDirection == LEFT) ? -moveSpeed: moveSpeed;

        //Movement
        RB2D.velocity = new Vector2(vX, RB2D.velocity.y);

        if (isHittingWall() || checkForEdge()) {

            if (facingDirection == LEFT) {
                ChangeFacingDirection(RIGHT);
            } else if (facingDirection == RIGHT) {
                ChangeFacingDirection(LEFT);
            }
        }
    }


    #region Enemy Take Player Damage
        
        public void takeDamageEnemy(int dmg) {
            damageReference.TakeDamage(dmg);
        }
 
        public void Kill() {
            _isAlive = false;
        }

    #endregion 


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
            //We have found a wall
            val = false;
        } else {
            val = true;
        }

        return val;
    }
    #endregion
}
