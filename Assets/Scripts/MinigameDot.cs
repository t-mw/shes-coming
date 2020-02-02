using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinigameDotState { Normal, Passed }
public enum ArrowOrientation { Left, Right, Up, Down }

public class MinigameDot : MonoBehaviour
{
    public GameObject Arrow;
    public GameObject Circle;

    private bool fadeIn = false;
    public float? fadeTime = null;
    public float alpha = 1.0f;

    public ArrowOrientation arrowOrientation = ArrowOrientation.Up;
    public MinigameDotState state;

    public float? focusTime = null;

    public void FadeIn()
    {
        this.fadeIn = true;
        this.fadeTime = Time.time;
    }

    public void FadeOut()
    {
        this.fadeIn = false;
        this.fadeTime = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateColors();

        float boopScaleLerp = this.focusTime.HasValue ?
            Mathf.Clamp(1.0f - (Time.time - this.focusTime.Value) / 0.2f, 0.0f, 1.0f) : 0.0f;
        boopScaleLerp *= boopScaleLerp;

        float scale = 1.0f + 0.4f * Mathf.Lerp(0.0f, 1.0f, boopScaleLerp);
        this.transform.localScale = new Vector3(scale, scale, 1.0f);

        switch (this.arrowOrientation)
        {
            case ArrowOrientation.Up:
                this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            case ArrowOrientation.Down:
                this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
                break;
            case ArrowOrientation.Left:
                this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
                break;
            case ArrowOrientation.Right:
                this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
                break;
            default:
                break;
        }
    }

    void UpdateColors()
    {
        {
            float darken = this.alpha < 1.0f ? 0.5f : 1.0f;

            var image = this.Circle.GetComponent<UnityEngine.UI.Image>();
            if (this.state == MinigameDotState.Normal)
            {
                image.color = new Color(darken * 1.0f, darken * 1.0f, darken * 1.0f, this.GetColorAlpha());
            }
            else if (this.state == MinigameDotState.Passed)
            {
                image.color = new Color(0.0f, darken * 1.0f, 0.0f, this.GetColorAlpha());
            }
        }

        {
            var image = this.Arrow.GetComponent<UnityEngine.UI.Image>();
            if (this.state == MinigameDotState.Normal)
            {
                image.color = new Color(0.0f, 0.0f, 0.0f, this.GetColorAlpha());
            }
            else
            {
                image.color = new Color(1.0f, 1.0f, 1.0f, 0.9f * this.GetColorAlpha());
            }
        }
    }

    float GetColorAlpha()
    {
        return this.alpha * Utility.CalculateFade(this.fadeTime, 0.3f, this.fadeIn);
    }
}
