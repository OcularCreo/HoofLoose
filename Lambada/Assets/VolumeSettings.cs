using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume")) 
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    private void Update()
    {
        if (musicSlider.value != PlayerPrefs.GetFloat("musicVolume")) 
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }

        if (sfxSlider.value != PlayerPrefs.GetFloat("sfxVolume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        }
    }

    public void SetMusicVolume() 
    {
        float volume = musicSlider.value;
        mixer.SetFloat("music", Mathf.Log10(volume) *20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        mixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadVolume() 
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        musicSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        SetMusicVolume();
        SetSFXVolume();
    }
}
