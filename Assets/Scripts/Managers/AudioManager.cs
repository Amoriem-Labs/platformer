using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
	public static AudioManager Instance { get { return _instance; } }

    [Tooltip("The time in the Science Hill soundtrack to skip to when the game starts. This variable is needed because the first few seconds of the soundtrack doesn't contain audio.")]
    public float soundtrackBeginTime;

    [Header("Pause Menu Music UI Icons")]
    public Sprite[] audioIcons;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Image musicIcon;
    public Image sfxIcon;

    // Hack to serialize a Dictionary in Inspector: Use a list of structures and then populate a dictionary in Awake with the variables of the structure.
    // The reason why we do this over serializing a list in the Inspector is because now we don't have to worry about matching indices when referencing the list.
    [Serializable]
    public struct SoundtrackStruct {
        public string name;
        public AudioSource soundtrack;
    }
    public SoundtrackStruct[] soundtracksInit;

    [Serializable]
    public struct SFXStruct {
        public string name;
        public AudioSource SFX;
    }
    public SFXStruct[] sfxsInit;

    public static Dictionary<string, AudioSource> soundtracks;
    public static Dictionary<string, AudioSource> SFXs;

    private List<AudioSource> currentlyPlayingSoundsAtPause; // Used to store the sounds that were playing when the game was paused.

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(_instance);

            musicIcon.sprite = audioIcons[1];
            sfxIcon.sprite = audioIcons[1];

            soundtracks = new Dictionary<string, AudioSource>();
            SFXs = new Dictionary<string, AudioSource>();
            
            // Populating soundtrack and SFX dictionaries according to hack
            foreach (SoundtrackStruct soundtrackStruct in soundtracksInit){
                soundtracks[soundtrackStruct.name] = soundtrackStruct.soundtrack;
            }
            foreach (SFXStruct sfxStruct in sfxsInit){
                SFXs[sfxStruct.name] = sfxStruct.SFX;
            }
        }
    }

    #region Grab soundtracks
    public AudioSource GetSoundtrack(string soundtrackName){
        return soundtracks[soundtrackName];
    }

    public AudioSource GetSFX(string SFXName)
    {
        return SFXs[SFXName];
    } 
    #endregion

    #region Pause, resume, and stop music
    public void PauseSound(){
        foreach (KeyValuePair<string, AudioSource> entry in soundtracks){
            if (entry.Value.isPlaying){
                entry.Value.Pause();
                currentlyPlayingSoundsAtPause.Add(entry.Value);
            }
        }
        foreach (KeyValuePair<string, AudioSource> entry in SFXs){
            if (entry.Value.isPlaying){
                entry.Value.Pause();
                currentlyPlayingSoundsAtPause.Add(entry.Value);
            }
        }
    }

    public void ResumeSound(){
        currentlyPlayingSoundsAtPause.ForEach(sound => sound.Play());
        currentlyPlayingSoundsAtPause.Clear();
    }

    public void StopSound(){
        foreach (KeyValuePair<string, AudioSource> entry in soundtracks){
            if (entry.Value.isPlaying){
                entry.Value.Stop();
            }
        }
        foreach (KeyValuePair<string, AudioSource> entry in SFXs){
            if (entry.Value.isPlaying){
                entry.Value.Stop();
            }
        }
    }
    #endregion

    #region Change volumes of music and SFX
    // Changes music soundtracks' volumes. Is triggered by music slider in settings.
    public void ChangeMusicVolume(){
        foreach (KeyValuePair<string, AudioSource> entry in soundtracks){
            entry.Value.volume = musicSlider.value;
        }
        if (musicSlider.value == 0){
            musicIcon.sprite = audioIcons[0];
        } else {
            musicIcon.sprite = audioIcons[1];
        }
    }

    // Changes SFX volumes. Is triggered by SFX slider in settings.
    public void ChangeSFXVolume(){
        foreach (KeyValuePair<string, AudioSource> entry in SFXs){
            entry.Value.volume = sfxSlider.value;
        }
        if (sfxSlider.value == 0){
            sfxIcon.sprite = audioIcons[0];
        } else {
            sfxIcon.sprite = audioIcons[1];
        }
    }
    #endregion
}