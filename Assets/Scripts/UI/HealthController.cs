using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public Slider healthSlider;
    public Image healthBar;

    #region Instance
        public static HealthController instance;
        void Awake() {
            instance = this;
        }
    #endregion
    
    public void updateHealth(float hp) {
        
        float hpCont = 0f;
        healthBar.color = new Color32(255,255,225,255);
        switch (hp)
        {
            case 5:
            case 4:
            case 3:
                hpCont = hp;
            break;
            case 2:
                hpCont = 1.7f;
            break;
            case 1:
                hpCont = 0.5f;
            break;
            default:
                healthBar.color = new Color32(255,255,225,0);
            break;
        }
        
        healthSlider.value = hpCont;


    }
}
