using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
	public static AudioManager Instance { get { return _instance; } }
    public AudioSource audioSource;
    public float _time;
    public Sprite[] audioIcons;
    public Image musicIcon;
    public Image sfxIcon;

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
        musicIcon.sprite = audioIcons[1];
        sfxIcon.sprite = audioIcons[1];
    }

    public void StopMusic(){
        audioSource.Stop();
    }

    public void StartMusic(){
        audioSource.Play();
    }

    public void ChangeMusicVolume(float volume){
        audioSource.volume = volume;
        if (volume == 0){
            musicIcon.sprite = audioIcons[0];
        } else if (volume > 0 && volume < 1f){
            musicIcon.sprite = audioIcons[1];
        } else {
            musicIcon.sprite = audioIcons[2];
        }
    }

    public void ChangeSFXVolume(float volume){
        audioSource.volume = volume;
        if (volume == 0){
            sfxIcon.sprite = audioIcons[0];
        } else if (volume > 0 && volume < 1f){
            sfxIcon.sprite = audioIcons[1];
        } else {
            sfxIcon.sprite = audioIcons[2];
        }
    }
}
