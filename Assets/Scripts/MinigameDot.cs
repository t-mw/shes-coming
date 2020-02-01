using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinigameDotState { Normal, Passed, Failed, Hidden }

public class MinigameDot : MonoBehaviour
{
    public GameObject Text;
    public GameObject Circle;

    private bool fadeIn = false;
    public float? fadeTime = null;

    public string text
    {
        get => _text;
        set
        {
            if (_text != value)
            {
                this.Text.GetComponent<UnityEngine.UI.Text>().text = value;
                _text = value;
            }
        }
    }
    private string _text = "";

    public MinigameDotState State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                var image = this.Circle.GetComponent<UnityEngine.UI.Image>();
                if (value == MinigameDotState.Normal)
                {
                    image.color = new Color(1.0f, 1.0f, 1.0f, this.GetColorAlpha());
                }
                else if (value == MinigameDotState.Passed)
                {
                    image.color = new Color(0.0f, 1.0f, 0.0f, this.GetColorAlpha());
                }
                else if (value == MinigameDotState.Failed)
                {
                    image.color = new Color(1.0f, 0.0f, 0.0f, this.GetColorAlpha());
                }
                this.stateChangeTime = Time.time;
                _state = value;
            }
        }
    }
    private MinigameDotState _state = MinigameDotState.Normal;

    float? stateChangeTime = null;

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

        float boopScaleLerp = this.stateChangeTime.HasValue ?
            Mathf.Clamp(1.0f - (Time.time - this.stateChangeTime.Value) / 0.2f, 0.0f, 1.0f) : 0.0f;
        boopScaleLerp *= boopScaleLerp;

        float scale = 1.0f + 0.4f * Mathf.Lerp(0.0f, 1.0f, boopScaleLerp);
        this.transform.localScale = new Vector3(scale, scale, scale);
    }

    void UpdateColors()
    {
        var text = this.Text.GetComponent<UnityEngine.UI.Text>();
        {
            var color = text.color;
            color.a = this.GetColorAlpha();
            text.color = color;
        }

        var image = this.Circle.GetComponent<UnityEngine.UI.Image>();
        {
            var color = image.color;
            color.a = this.GetColorAlpha();
            image.color = color;
        }
    }

    float GetColorAlpha()
    {
        float? lerp = null;
        if (this.fadeTime.HasValue)
        {
            lerp = Mathf.Clamp((Time.time - this.fadeTime.Value) / 0.3f, 0.0f, 1.0f);
        }

        if (this.fadeIn)
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

                return 1.0f - Mathf.Lerp(0.0f, 1.0f, lerp.Value);
            }
            else
            {
                return 0.0f;
            }
        }
    }
}
