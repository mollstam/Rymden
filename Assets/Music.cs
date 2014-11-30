﻿using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioClip ChillMusic;
    public AudioClip DramaMusic;

    // Use this for initialization
    void Start()
    {
        var music = GameObject.FindGameObjectsWithTag("Music");

        foreach (var m in music)
        {
            if (m != gameObject)
            {
                m.audio.clip = ChillMusic;
                m.audio.loop = true;
                audio.Play();
                Destroy(gameObject);
                return;
            }
                
        }

        audio.clip = ChillMusic;
        audio.loop = true;
        audio.Play();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}