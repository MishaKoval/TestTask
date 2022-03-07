using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{

    //public AudioClip clip;

    public UnityEvent onSoundPlay;

    public void PlaySound(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip,Vector3.zero);
        
    }
/*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
