using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GifMaker : MonoBehaviour
{
    public Image yourFace;

    public float time, switchSpriteTime, rot1, rot2;


    void Update()
    {
        time += Time.deltaTime;

        if (time < switchSpriteTime){
           yourFace.rectTransform.Rotate(new Vector3( 0, 0, rot1));
        }

        if (time > switchSpriteTime && time < switchSpriteTime*2){
            yourFace.rectTransform.Rotate(new Vector3( 0, 0, rot2));
        }

        if (time > switchSpriteTime*2){
            time = 0;
        }
    }
}
