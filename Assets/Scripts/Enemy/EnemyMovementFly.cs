using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovementFly : MonoBehaviour
{
    enum State {PATROL, CHASE, RAGE, ALERT, RETURN};
    [SerializeField] State enemyState = State.PATROL;

    public float speed = 5f;
    public float closeSpeed = 10f;
    public bool chase = false;
    public Transform startingPosition;
    private GameObject player;
    [SerializeField] GameObject perimeter;
    [SerializeField] GameObject perimeterRef;

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
            Chase();
        } else {
            returnPosition(); 
        }
            
        Flip();
    }


    void Chase() {
        if (Vector2.Distance(this.transform.position, player.transform.position) <= 10f) {
            speed = closeSpeed;
            //PlayState
        } else {
            speed = 5;
        }
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        
    }

    void returnPosition() {
        transform.position = Vector2.MoveTowards(transform.position, startingPosition.position, speed * Time.deltaTime);
    }

    void Flip() {
        // Only works if the original sprite is facing left
        if (transform.position.x > player.transform.position.x) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else  {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }


}

