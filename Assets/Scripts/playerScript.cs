using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{

    [SerializeField] float x, y;
    [SerializeField] float _speed = 2f;
    [SerializeField] bool _canJump = true;

    void Start()
    {
        
    }

    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
    }
}
