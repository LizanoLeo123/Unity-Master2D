using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Chronometer _chronometer;

    private AudioFX _audioFXScript;

    private void Start()
    {
        _chronometer = GameObject.FindObjectOfType<Chronometer>()
            .GetComponent<Chronometer>();

        _audioFXScript = GameObject.Find("SoundFX").GetComponent<AudioFX>();


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _audioFXScript.playCrashFX();
        _chronometer.myTime -= 20;
        if (collision.GetComponent<Car>() != null)
            Destroy(gameObject);
    }
}
