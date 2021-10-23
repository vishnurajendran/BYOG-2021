using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AudioKeyPair{
    [SerializeField]
    public string fileKey;
    [SerializeField]
    public AudioClip soundFile;
}

[CreateAssetMenu(fileName = "AudioDB", menuName = "ScriptableObjects/AudioDB", order = 1)]
public class AudioDB : ScriptableObject
{
    public List<AudioKeyPair> audioKeyPairList;

    public AudioClip GetAudioClip(string key)
    {
        foreach(var e in audioKeyPairList)
        {
            if(e.fileKey == key)
            {
                return e.soundFile;
            }
        }

        return null;
    }
}