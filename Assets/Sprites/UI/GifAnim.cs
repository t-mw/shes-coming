using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GifAnim : MonoBehaviour
{

    public GameObject Im1, Im2, Im3;

    public float time, switchSpriteTime;


    void Update()
    {
        time += Time.deltaTime;

        if (time < switchSpriteTime){
            Im1.SetActive(true);
            Im2.SetActive(false);
            Im3.SetActive(false);

        }

        if (time > switchSpriteTime && time < switchSpriteTime*2){
            Im1.SetActive(false);
            Im2.SetActive(true);
            Im3.SetActive(false);

        }

        if (time > switchSpriteTime*2 && time < switchSpriteTime*3){
            Im1.SetActive(false);
            Im2.SetActive(false);
            Im3.SetActive(true);
        }
        if (time > switchSpriteTime*3){
            time = 0;
        }
    }
}
