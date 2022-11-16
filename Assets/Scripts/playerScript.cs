using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{

    [SerializeField] float x, y;
    [SerializeField] float _speed = 10f;
    [SerializeField] bool _canJump = false;

    [SerializeField] Rigidbody2D RB2D;
    [SerializeField] jumpCheckScript jmpChkScript;

    [SerializeField] int jmpNum = 1;


    void Awake() {

        if (RB2D == null) RB2D = this.GetComponent<Rigidbody2D>();
        if (jmpChkScript == null) jmpChkScript = GameObject.Find("bottomCheck").GetComponent<jumpCheckScript>(); 
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        //Movement
        x = Input.GetAxisRaw("Horizontal");

        if (x != 0) 
            transform.Translate(new Vector3(x, 0, 0) * Time.deltaTime * _speed);

        if (Input.GetKeyDown(KeyCode.Space) && _canJump == true) {
            Jump();

        }

        if (_canJump == false) jmpChkScript.enabled = true;
        else jmpChkScript.enabled = false;

        

    }

    //When Messing With Physics
    void FixedUpdate() {
        
    }

    public void changeJmpState(bool state) {

        _canJump = state;
        

        Debug.Log("Floored"); 

    }

    void Jump() {
        Debug.Log("Jump");
        _canJump = false;  
        RB2D.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
    }

}
