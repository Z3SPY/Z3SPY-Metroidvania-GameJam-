using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] AudioSource audioSource;


    #region singleton
    public static AudioManager instance;
    void Awake() {
        instance = this;
    }
    #endregion

    void Start() {
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }
        

    public void setAudio(AudioClip sound) {

        audioSource.clip = sound;
        audioSource.Play();
    }

    public void setAudioShot(AudioClip sound, float volume) {
        audioSource.PlayOneShot(sound, volume);
    }

    void playAudio() {
        audioSource.Play();
    }


}
