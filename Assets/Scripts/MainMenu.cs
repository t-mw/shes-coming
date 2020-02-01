using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD;

public class MainMenu : MonoBehaviour
{

    FMOD.Studio.Bus musicBus;
    FMOD.Studio.Bus sfxBus;


    private void Start()
    {
        string musicBusString = "Bus:/Music";
        musicBus = FMODUnity.RuntimeManager.GetBus(musicBusString);
        string sfxBusString = "Bus:/SFX";    
        sfxBus = FMODUnity.RuntimeManager.GetBus(sfxBusString);
    }


    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }


    public void SetMusicVolume(float musicVolume)
    {      
        musicBus.setVolume(musicVolume);
    }
    public void SetSFXVolume(float sfxVolume)
    {
        musicBus.setVolume(sfxVolume);
    }
}
