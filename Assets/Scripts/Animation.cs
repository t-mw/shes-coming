using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
   
    public Animator anim;

    public bool onPlayback = false;
    public bool repaired = false;

    private void Start()
    {
        anim.SetFloat("animSpeed", 1f);
        anim.enabled = false;
        onPlayback = false;
    }

    public void Repaired()
    {
        repaired = true;
        anim.SetFloat("animSpeed", 1f);
        anim.enabled = true;
    }


    public void SpeedToZero()
    {
        if (!onPlayback && !repaired)
        {
       
            anim.SetFloat("animSpeed", 0f);
            anim.enabled = false;

        }
        Debug.Log("zero");

    }
    public void PlaybackOff()
    {
        onPlayback = false;
        anim.SetFloat("animSpeed", 1f);
        anim.enabled = false;
    }



    public void NextAnimation()
    {

        if (onPlayback)
        {
            anim.enabled = true;
            onPlayback = false;
            anim.Play("Animation", 0, 0f);
            
        }
        else
        {
            anim.enabled = true;
            anim.SetFloat("animSpeed", 1f);
           // anim.Play("Animation", 0, startTime);
        }

    }


    public void PlayBackAnimation()
    {
        Debug.Log("playback checker");
        onPlayback = true;
        anim.enabled = true;
        anim.SetFloat("animSpeed", -2f);
    }
}
