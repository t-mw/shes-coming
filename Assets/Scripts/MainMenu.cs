using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class MainMenu : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioMixer audioMixer;


    private void Start()
    {

    }


    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }


    public void SetMusicVolume(float musicVolume)
    {
        audioMixer.SetFloat("_musicVol", musicVolume);
    }
    public void SetSFXVolume(float sfxVolume)
    {
        audioMixer.SetFloat("_sfxVol", sfxVolume);
    }
}
