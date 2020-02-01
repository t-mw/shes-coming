using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinigameDotState { Normal, Passed, Failed, Hidden }

public class MinigameDot : MonoBehaviour
{
    public GameObject Text;
    public GameObject Circle;

    public bool FadeOut
    {
        get => _fadeOut;
        set
        {
            if (_fadeOut != value)
            {
                this.fadeOutTime = Time.time;
                _fadeOut = value;
            }
        }
    }

    private bool _fadeOut = false;

    private float? fadeOutTime = null;

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
                    image.color = new Color(1.0f, 1.0f, 1.0f);
                }
                else if (value == MinigameDotState.Passed)
                {
                    image.color = new Color(0.0f, 1.0f, 0.0f);
                }
                else if (value == MinigameDotState.Failed)
                {
                    image.color = new Color(1.0f, 0.0f, 0.0f);
                }
                this.stateChangeTime = Time.time;
                _state = value;
            }
        }
    }
    private MinigameDotState _state = MinigameDotState.Normal;

    float? stateChangeTime = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var text = this.Text.GetComponent<UnityEngine.UI.Text>();
        text.color = this.SetColorAlpha(text.color);

        var image = this.Circle.GetComponent<UnityEngine.UI.Image>();
        image.color = this.SetColorAlpha(image.color);

        float boopScaleLerp = this.stateChangeTime.HasValue ?
            Mathf.Clamp(1.0f - (Time.time - this.stateChangeTime.Value) / 0.2f, 0.0f, 1.0f) : 0.0f;
        boopScaleLerp *= boopScaleLerp;

        float scale = 1.0f + 0.4f * Mathf.Lerp(0.0f, 1.0f, boopScaleLerp);
        this.transform.localScale = new Vector3(scale, scale, scale);
    }

    Color SetColorAlpha(Color color)
    {
        if (this._fadeOut)
        {
            color.a = 0.0f;
        }
        else
        {
            color.a = 1.0f;
        }

        return color;
    }
}
