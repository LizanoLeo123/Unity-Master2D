using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFX : MonoBehaviour
{
    public AudioClip[] soundFX;
    private AudioSource _audioSource; 
    //0 Crash
    //1 Game Music


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void playCrashFX()
    {
        _audioSource.clip = soundFX[0];
        _audioSource.volume = 0.7f;
        _audioSource.Play();
    }

    public void playGameMusic()
    {
        _audioSource.clip = soundFX[1];
        _audioSource.volume = 0.3f;
        _audioSource.Play();
    }

}
