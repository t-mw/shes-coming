using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneSfx : MonoBehaviour
{
    public AudioSource audioS;
    public AudioClip[] PleasePlaySounds;

    public float minTime;
    public float maxTime;

    private void Start()
    {
        float rand = Random.Range(minTime, maxTime);
        StartCoroutine(waitToPlaySfx(rand));
    }


    public IEnumerator waitToPlaySfx(float t)
    {
        yield return new WaitForSeconds(t);
        Play();
    }

    public void Play()
    {
        int rand = Random.Range(0, PleasePlaySounds.Length);
        audioS.clip = PleasePlaySounds[rand];
        audioS.Play();
    }
}
