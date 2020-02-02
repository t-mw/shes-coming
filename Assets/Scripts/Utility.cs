using System;
using System.Collections;
using UnityEngine;

class Utility
{
    public static float CalculateFade(float? startTime, float duration, bool fadeIn)
    {
        float? lerp = null;
        if (startTime.HasValue)
        {
            lerp = Mathf.Clamp((Time.time - startTime.Value) / 0.3f, 0.0f, 1.0f);
        }

        if (fadeIn)
        {
            if (lerp.HasValue)
            {
                return Mathf.Lerp(0.0f, 1.0f, lerp.Value);
            }
            else
            {
                return 1.0f;
            }
        }
        else
        {
            if (lerp.HasValue)
            {

                return (1.0f - Mathf.Lerp(0.0f, 1.0f, lerp.Value));
            }
            else
            {
                return 0.0f;
            }
        }
    }

    public static IEnumerator DelaySeconds(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}