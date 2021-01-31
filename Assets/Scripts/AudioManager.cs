using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] Sounds;

    public static AudioManager instance;
    
    // Start is called before the first frame update
    private void Awake()
    {

        //if (instance == null) // may need to swith to if == to null
        //{
        //    instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        DontDestroyOnLoad(gameObject);

        foreach (Sound S in Sounds)
        {
            S.source = gameObject.AddComponent<AudioSource>();
            S.source.clip = S.clip;
            S.source.volume = S.volume;
            S.source.pitch = S.pitch;
        }
    }

    public void Play (string name)
    {
        // Huh?
        Sound S = Array.Find(Sounds, sound => sound.name == name);
        S.source.Play();
    }

    public bool IsPlaying (string name)
    {
        Sound S = Array.Find(Sounds, sound => sound.name == name);
        return S.source.isPlaying;
    }
}
