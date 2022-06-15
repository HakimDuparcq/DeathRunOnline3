using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Range(0f, 1f)]
    public float MainVolume =1f;
    /*
    [Range(0f, 1f)]
    public float MusicVolume;

    [Range(0f, 1f)]
    public float FXVolume;
    */

    [Space]
    [Header("Echap")]
    public Slider volumeSlider;
    public TextMeshProUGUI volumeValue;
    public TextMeshProUGUI volumeText;

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
            s.source.volume = s.volume  * MainVolume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

        }
        volumeSlider.value = MainVolume;
        volumeValue.text = Math.Round(volumeSlider.value ,  1 ).ToString();
        volumeSlider.onValueChanged.AddListener(delegate { ValueChangeSliderCheck(); });
    }
    public void Start()
    {
        Play("Theme");
    }

    public void ValueChangeSliderCheck()
    {
        MainVolume = volumeSlider.value ;
        volumeValue.text = Math.Round(volumeSlider.value, 1).ToString();

        foreach (Sound s in sounds)
        {
            if (s.source == null)
                return;
            s.source.volume = s.volume * MainVolume;
            s.source.pitch = s.pitch;
        }
    }

    private void OnValidate() //lorsque la valleur change
    {
        foreach (Sound s in sounds)
        {
            if (s.source ==null)
                return;
            s.source.volume = s.volume * MainVolume;
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
    

    public void HasHideUI(bool hide)
    {
        volumeSlider.gameObject.SetActive(!hide);
        volumeValue.gameObject.SetActive(!hide);
        volumeText.gameObject.SetActive(!hide);
    }

}