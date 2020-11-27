using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{

    
    public AudioSource otherAudioSource;
    public AudioSource backgroundAudioSource;

    // public AudioClip backgrundMusicClip;
    public AudioClip duckDeathClip;
    public AudioClip fireClip;
    public AudioClip clickClip;
    GameManager gameManager;
    public bool isPlay;
    public static SoundManager shared;
    private void Awake()
    {
        shared = this;
        gameManager = GetComponent<GameManager>();
    }   

        void Start()
    {
       
           // backgroundAudioSource.volume = 0.3f;
          //  backgroundAudioSource.PlayOneShot(backgrundMusicClip);

       
    }

    public void PlayDuckDeathSound()
    {
        otherAudioSource.volume = 0.8f;
      otherAudioSource.PlayOneShot(duckDeathClip);
       
    }
    public void PlayFireSound(Vector3 position)
    {
       
            otherAudioSource.volume = 0.5f;
       otherAudioSource.PlayOneShot(fireClip);
      // AudioSource.PlayClipAtPoint(fireClip, position);
       // otherAudioSource.clip = fireClip;
        //otherAudioSource.Play();
       
       
       
    }
    public void PlayClickSound()
    {
        otherAudioSource.volume = 0.9f; 
        otherAudioSource.PlayOneShot(clickClip);
    }
}
