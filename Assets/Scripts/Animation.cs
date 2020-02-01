using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
   
    public Animator anim;

    public bool onPlayback = false;


    private void Start()
    {
        anim.SetFloat("animSpeed", 0f);
      
        onPlayback = false;
    }

    public void SpeedToZero()
    {
        if (!onPlayback)
        {
       
            anim.SetFloat("animSpeed", 0f);
            anim.enabled = false;

        }
        

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
