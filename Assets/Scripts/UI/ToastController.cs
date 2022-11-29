using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastController : MonoBehaviour
{
    public Slider toastSlider;
    public Image toastBar;
    public Image toastImgComp;
    public Sprite[] toastImg;

    void Update() {
        toastSlider.value = GameManager.instance.GetToast();

        if (toastSlider.value >= 1f) {
            toastImgComp.sprite = toastImg[1];
        } else {
            toastImgComp.sprite = toastImg[0];
        }
    }
}
