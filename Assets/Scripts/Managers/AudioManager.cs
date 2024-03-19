using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
	public static AudioManager Instance { get { return _instance; } }
    public AudioSource audioSource;
    public float skipToThisTimeInSoundtrack;
    public Sprite[] audioIcons;
    public Slider musicSlider;
    public Slider sfxSlider;
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
        audioSource.time = skipToThisTimeInSoundtrack;
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

    public void PauseMusic(){
        audioSource.Pause();
    }

    public void ChangeMusicVolume(){
        audioSource.volume = musicSlider.value;
        if (musicSlider.value == 0){
            musicIcon.sprite = audioIcons[0];
        } else {
            musicIcon.sprite = audioIcons[1];
        }
    }

    public void ChangeSFXVolume(){
        audioSource.volume = sfxSlider.value;
        if (sfxSlider.value == 0){
            sfxIcon.sprite = audioIcons[0];
        } else {
            sfxIcon.sprite = audioIcons[1];
        }
    }
}
