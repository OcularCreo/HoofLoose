using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--- Audio Source ---")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--- Audio Clip ---")]
    public AudioClip backgroundMusic;
    public AudioClip poseSound1;
	public AudioClip poseSound2;
	public AudioClip poseSound3;
	public AudioClip poseSound4;
	public AudioClip twerk;
	public AudioClip wtf;
	public AudioClip select;
	public AudioClip perfect;
	public AudioClip great;
	public AudioClip good;
	public AudioClip miss;


    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    { 
        SFXSource.PlayOneShot(clip);
    }

	public void PlaySong(AudioClip clip)
	{
		musicSource.clip = clip;
		musicSource.Play();
	}
}
