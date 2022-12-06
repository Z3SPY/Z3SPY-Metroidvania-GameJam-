using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovementFly : MonoBehaviour
{
    enum FlyState {PATROL, CHASE, RAGE, ALERT, RETURN};
    [SerializeField] FlyState enemyState = FlyState.PATROL;

    public float speed = 5f;
    public float closeSpeed = 10f;
    public bool chase = false;
    public Transform startingPosition;
    private GameObject player;
    [SerializeField] GameObject perimeter;
    [SerializeField] GameObject perimeterRef;

    [Header("Damage")]
    public UnityEvent onAttack;
    [SerializeField] bool _canDamage = true;
    
    [Header("Line Renderer")]
    [SerializeField] Transform castPos;
    [SerializeField] float castDistPlayerDetection = 8f;
    const string LEFT = "left";
    const string RIGHT = "right";
    string facingDirection;


        [Header("Collision")]
    [SerializeField] Rigidbody2D RB2D;

    [SerializeField] 
    private float strength = 12, delay = 0.15f;



    void Awake() {
        perimeterRef = Instantiate(perimeter, this.transform.position, Quaternion.identity);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startingPosition = transform.parent;

    }

    void Update()
    {
        chase = perimeterRef.GetComponent<flyPerimeter>().getBool();

        if (player == null) 
            return;        
        if (startingPosition == null)
            return;

        if (chase == true)  {

            if (_canDamage == true) {
                Chase();
            }
            
        } else {

            returnPosition();
        }

        Flip();


        
    }


    void Chase() {

        // If within distance do osomething
        if (Vector2.Distance(this.transform.position, player.transform.position) <= 10f) {
            StartCoroutine(goingToAttack());
        } else {
            speed = 5;
        }
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        
    }

    void returnPosition() {        
        

        //Do something about this
        if (Vector2.Distance(this.transform.position, startingPosition.position) <= 0.5f) {
            StartCoroutine(waitAndReturn());
        }
        
        transform.position = Vector2.MoveTowards(transform.position, startingPosition.position, speed * Time.deltaTime);
    }

    IEnumerator goingToAttack() {
        yield return new WaitForSeconds(1.25f);
        speed = closeSpeed;
        yield return new WaitForSeconds(1f);
        speed = 5f;
    }

    IEnumerator waitAndReturn() {
        yield return new WaitForSeconds(1.5f);
    }
    
    void Flip() {
        // Only works if the original sprite is facing left
        if (transform.position.x > player.transform.position.x) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            facingDirection = LEFT;
        } else  {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            facingDirection = RIGHT;
        }
    }

    #region playerDetect
    bool isDetectingPlayer() {
            bool val = false;

            float castDist = castDistPlayerDetection;

            if (facingDirection == LEFT) {
                castDist = -castDistPlayerDetection;
            } else {
                castDist = castDistPlayerDetection;
            }


            //Determine the target destination based on the cast distance
            Vector3 targetPos = castPos.position;
            targetPos.x += castDist;

            //Draw Line
            Debug.DrawLine(castPos.position, targetPos, Color.yellow);
            
            if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Character"))) {
                
                RaycastHit2D hit = Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Character"));

                //We have found a Player
                val = true;
            } else {
                val = false;
            }

            return val;
        }

    #endregion

    #region Collision

        //Player Collsion Knock Back
        void PlayerCollision(GameObject sender) {
            StopAllCoroutines();
            Vector2 direction = (transform.position - sender.transform.position).normalized;
            RB2D.AddForce(direction*strength, ForceMode2D.Impulse);
            StartCoroutine(Reset());
        }

         private IEnumerator Reset() {
            yield return new WaitForSeconds(delay);
            RB2D.velocity = Vector3.zero;
        }

        //Attack Delay
        IEnumerator waitAttack() {
            _canDamage = false;
            yield return new WaitForSeconds(0.2f);
            _canDamage = true;
        }

        void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player") && _canDamage == true) {
                PlayerCollision(other.gameObject);
                StartCoroutine(waitAttack());
            }
        }

        void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("Perimeter")) {
                this.transform.position = this.transform.position;
            }
        }

    #endregion
}

