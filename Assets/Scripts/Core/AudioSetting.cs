using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sFXSlider;
    void Start()
    {
        if(PlayerPrefs.HasKey("MusicVolume"))
            loadVolume();
        else
        {
            setMusicVolume();
            setSFXVolume();
        }
    }
    public void setMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("Music",Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("MusicVolume",volume);
    }
    public void setSFXVolume()
    {
        float volume = sFXSlider.value;
        audioMixer.SetFloat("SFX",Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("SFXVolume",volume);
    }
    public void loadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        setMusicVolume();
        setSFXVolume();
    }
    public void Exit()
    {
        gameObject.SetActive(false);
    }
}
