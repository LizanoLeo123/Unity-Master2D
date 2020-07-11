using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSounds : MonoBehaviour
{
    public AudioClip bloop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reproducirSonido()
    {
        AudioSource.PlayClipAtPoint(bloop,transform.position);
    }
}
