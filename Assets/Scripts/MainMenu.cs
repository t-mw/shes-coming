using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    public AudioSource audioSource;


    private void Start()
    {

    }


    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }


    public void SetMusicVolume(float musicVolume)
    {      
        
    }
    public void SetSFXVolume(float sfxVolume)
    {
       
    }
}
