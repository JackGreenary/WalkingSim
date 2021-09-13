using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioController instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " was not found.");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " was not found.");
            return;
        }
        s.source.Stop();
    }

    public void MuffleSound(bool muffle)
    {
        if (muffle)
        {
            // Muffle all listed sounds
            //foreach (Sound s in sounds)
            //{
            //    s.source.volume = .25f;
            //    s.source.pitch = .5f;
            //}
            // Muffle all audio sources
            AudioSource[] sources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource aS in sources)
            {
                if (aS.clip != null && !aS.clip.name.Contains("Music"))
                {
                    aS.volume = .25f;
                    aS.pitch = .5f;
                }
            }
        }
        else
        {
            // Unmuffle all listed sounds
            //foreach (Sound s in sounds)
            //{
            //    s.source.volume = 1f;
            //    s.source.pitch = 1f;
            //}
            // Unmuffle all audio sources
            AudioSource[] sources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource aS in sources)
            {
                if (aS.clip != null && !aS.clip.name.Contains("Music"))
                {
                    aS.volume = 1f;
                    aS.pitch = 1f;
                }
            }
        }
    }

    public void PlaybackgroundMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " was not found.");
            return;
        }
        s.source.Play();
    }
}
