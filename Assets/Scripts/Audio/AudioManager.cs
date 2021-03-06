using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Range(0f, 1f)]
    public float MusicVolume =1f;
    
    [Range(0f, 1f)]
    public float FXVolume;

    [Space]
    public Slider volumeMusicSlider;
    public TextMeshProUGUI volumeMusicValue;
    public TextMeshProUGUI volumeMusicText;

    [Space]
    public Slider volumeFXSlider;
    public TextMeshProUGUI volumeFXValue;
    public TextMeshProUGUI volumeFXText;

    [Space]
    public Sound[] sounds;
    


    void Awake()
    {

        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume =   s.soundType==SoundType.Music ?    s.volume * MusicVolume   :   s.volume * FXVolume  ;
            
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

        }
        volumeMusicSlider.value = MusicVolume;
        volumeMusicValue.text = Math.Round(volumeMusicSlider.value ,  1 ).ToString();
        volumeMusicSlider.onValueChanged.AddListener(delegate { ValueChangeSliderCheck(); });

        volumeFXSlider.value = FXVolume;
        volumeFXValue.text = Math.Round(volumeFXSlider.value, 1).ToString();
        volumeFXSlider.onValueChanged.AddListener(delegate { ValueChangeSliderCheck(); });
    }
    public void Start()
    {
        #if !UNITY_SERVER
            Play("Theme");
        #endif
    }

    public void ValueChangeSliderCheck()
    {
        MusicVolume = volumeMusicSlider.value ;
        volumeMusicValue.text = Math.Round(volumeMusicSlider.value, 1).ToString();

        FXVolume = volumeFXSlider.value;
        volumeFXValue.text = Math.Round(volumeFXSlider.value, 1).ToString();

        foreach (Sound s in sounds)
        {
            if (s.source == null)
                return;
            s.source.volume = s.soundType == SoundType.Music ? s.volume * MusicVolume : s.volume * FXVolume;
            s.source.pitch = s.pitch;
        }
    }

    private void OnValidate() //lorsque la valleur change
    {
        foreach (Sound s in sounds)
        {
            if (s.source ==null)
                return;
            s.source.volume = s.soundType == SoundType.Music ? s.volume * MusicVolume : s.volume * FXVolume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s==null)
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