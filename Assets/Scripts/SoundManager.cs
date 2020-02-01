using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public AudioSource audioS;
    public AudioClip[] taping;

    public void Taping()
    {
        int rand = Random.Range(0, taping.Length);
        audioS.clip = taping[rand];
        audioS.Play();
    }
  
}
