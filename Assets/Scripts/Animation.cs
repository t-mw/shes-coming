using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{

    public Animator anim;

    public bool onPlayback = false;
    public bool repaired = false;
    public GameObject tapedEffect;
    public bool ignoreEvents = false;

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
        if (tapedEffect != null)
        {
            tapedEffect.SetActive(true);
        }
    }


    public void SpeedToZero()
    {
        if (!ignoreEvents && !onPlayback && !repaired)
        {

            anim.SetFloat("animSpeed", 0f);
            anim.enabled = false;
            Debug.Log("zero");
        }


    }

    private void Update()
    {
        if (onPlayback && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.1f)
        {
            anim.SetFloat("animSpeed", 0f);
            anim.enabled = false;
            onPlayback = false;
        }
        if (!onPlayback && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f)
        {
            if (tapedEffect != null)
            {
                tapedEffect.SetActive(true);
            }
        }
    }

    public void PlayToEnd()
    {
        ignoreEvents = true;
        this.NextAnimation();
    }

    public void PlaybackOff()
    {
        onPlayback = false;
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
        ignoreEvents = false;
        onPlayback = true;
        anim.enabled = true;
        anim.SetFloat("animSpeed", -2f);
        if (tapedEffect != null)
        {
            tapedEffect.SetActive(false);
        }
    }
}
