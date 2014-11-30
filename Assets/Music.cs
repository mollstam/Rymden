﻿using System;
﻿using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioClip ChillMusic;
    public AudioClip DramaMusic;
    private AudioSource _chillSource;
    private float _chillVolume;
    private AudioSource _dramaSource;

    // Use this for initialization
    void Start()
    {
        var music = GameObject.FindGameObjectsWithTag("Music");

        foreach (var m in music)
        {
            if (m != gameObject)
            {
                Destroy(gameObject);
                return;
            }
        }

        _chillSource = gameObject.AddComponent<AudioSource>();
        _chillSource.loop = true;
        _chillSource.clip = ChillMusic;
        _chillVolume = 1.0f;
        _chillSource.Play();
        
        _dramaSource = gameObject.AddComponent<AudioSource>();
        _dramaSource.loop = false;
        _dramaSource.clip = DramaMusic;
        _dramaSource.Stop();

        DontDestroyOnLoad(gameObject);
    }

    public void SetDrama()
    {
        _dramaSource.Stop();
        _dramaSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (_dramaSource.isPlaying)
        {
            _chillVolume -= Time.time;
            _chillVolume = Math.Max(_chillVolume, 0);
        }

        _chillSource.volume = _chillVolume;
    }
}