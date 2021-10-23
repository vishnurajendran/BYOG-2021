using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public class AudioInfo
    {
        [SerializeField]
        public string voName;
        [SerializeField]
        public string textOnScreen;

        public AudioInfo(string voName, string textOnScreen)
        {
            this.voName = voName;
            this.textOnScreen = textOnScreen;
        }
    }
    
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

    void Start()
    {
        PlayAudio("First_Reset");
    }

    public void PlayAudio(string key)
    {
        audioSource.PlayOneShot(GetAudioClip(key));
    }

    public Dictionary<string, AudioInfo> audioLookup = new Dictionary<string, AudioInfo>{
        {"First_Reset",new AudioInfo("First_Reset","Blah blah, blah")}
    };

    public AudioClip GetAudioClip(string key)
    {
        return Resources.Load<AudioClip>(audioLookup[key].voName);
    }
}
