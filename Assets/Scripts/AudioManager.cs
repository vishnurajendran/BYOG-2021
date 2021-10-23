using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioDB audioDB;
    private AudioSource audioSource;
    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
            instance = this;
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start(){
        PlayAudio("First_Reset");
    }

    public void PlayAudio(string key)
    {
        audioSource.PlayOneShot(audioDB.GetAudioClip(key));
    }
}
