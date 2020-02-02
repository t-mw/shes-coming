using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public AudioSource audioS;
    public AudioClip[] taping;
    public AudioClip[] crashing;
    public AudioClip[] happyEnding;
    public AudioClip[] badEnding;

    public void Taping()
    {
        int rand = Random.Range(0, taping.Length);
        audioS.clip = taping[rand];
        audioS.Play();
    }

    public void Crashing()
    {
        int rand = Random.Range(0, taping.Length);
        audioS.clip = crashing[rand];
        audioS.Play();
        }
    }

