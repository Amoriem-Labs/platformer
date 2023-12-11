using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public float _time;
    void Start()
    {
        audioSource.time = _time;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
