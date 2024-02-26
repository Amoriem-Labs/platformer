using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
	public static AudioManager Instance { get { return _instance; } }
    public AudioSource audioSource;
    public float _time;

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
    }
    
    void Start()
    {
        audioSource.time = _time;
        audioSource.Play();
    }

    public void StopMusic(){
        audioSource.Stop();
    }

    public void StartMusic(){
        audioSource.Play();
    }
}
