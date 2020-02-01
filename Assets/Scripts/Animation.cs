using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
   
    public Animator anim;

    private int currentStage = 0;

 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayNextAnimation();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlayBackAnimation();
        }
    }

    public void PlayNextAnimation()
    {
        switch (currentStage)
        {
            case 0:
                anim.Play("Stage 1");
                break;
            case 1:
                anim.Play("Stage 2");
                break;
            case 2:
                anim.Play("Stage 3");
                break;

            default:
                break;
        }
        currentStage++;
    }

    public void PlayBackAnimation()
    {
        if (currentStage == 2)
        {
            anim.Play("Stage Back");
        }
        else
        {
            anim.Play("Stage 0");
        }
        currentStage = 0;
    }
}
