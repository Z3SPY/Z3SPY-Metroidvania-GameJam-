using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockbackScript : MonoBehaviour
{

    [SerializeField] Rigidbody2D RB2D;

    [SerializeField] 
    private float strength = 16, delay = 0.15f;

    public UnityEvent OnBegin, OnDone;


    public void PlayFeedback(GameObject sender) {
        StopAllCoroutines();
        OnBegin?.Invoke();
        Vector2 direction = (transform.position - sender.transform.position).normalized;
        RB2D.AddForce(direction*strength, ForceMode2D.Impulse);
        StartCoroutine(Reset());
    }


    private IEnumerator Reset() {
        yield return new WaitForSeconds(delay);
        RB2D.velocity = Vector3.zero;
        OnDone?.Invoke();
    }
}
