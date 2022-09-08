using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioPlayerManager : MonoBehaviour
{
    [Space]
    public Sound[] sounds;

    void Awake()
    {        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * AudioManager.instance.FXVolume;

            s.source.pitch = s.pitch;
            s.source.spatialBlend = s.SpacialBlend;
            s.source.loop = s.loop;
        }
        AudioManager.SliderSoundChanged += OnValidate;

    }

    private void OnValidate() //lorsque la valleur change
    {
        foreach (Sound s in sounds)
        {
            if (s.source == null)
                return;
            s.source.volume = s.volume * AudioManager.instance.FXVolume;
            s.source.spatialBlend = s.SpacialBlend;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    
}
