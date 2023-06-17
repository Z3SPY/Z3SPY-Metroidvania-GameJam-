using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextController : MonoBehaviour
{
    #region singleton
    public static TextController instance;
    void Awake() {
        instance = this;
    }
    #endregion

    public Text textEvent, textCommand;
    public Animator anim;

    void Start() {
    }

    void Update() {
        if (GameManager.instance.winGame == true) {
            textEvent.color = Color.black;
            textCommand.color = Color.black;
        } else {
            textEvent.color = Color.white;
            textCommand.color = Color.white;
        }
    }

    public void SetText(string msgEvent, string msgCommand) {
        textEvent.text = msgEvent;
        textCommand.text = msgCommand;

        PlayAnimation();
    }



    private void PlayAnimation() {
        anim.Play("text");
        if (GameManager.instance.winGame == false)
        StartCoroutine(waitAnim());

    }

    IEnumerator waitAnim() {
        yield return new WaitForSeconds(2f);
        anim.Play("textExit");
    }


    

}
