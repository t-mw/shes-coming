using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public GameObject scoreText;

    public int score
    {
        get => this._score;
        set
        {
            int newScore = Mathf.Max(value, 0);
            if (this._score != newScore)
            {
                this.latestDiff = newScore - this._score;
                this.latestDiffTime = Time.time;
                this._score = newScore;
            }
        }
    }
    int _score = 0;
    int latestDiff = 0;
    float? latestDiffTime = null;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var text = this.scoreText.GetComponent<UnityEngine.UI.Text>();
        text.text = this._score.ToString();

        if (this.latestDiffTime.HasValue)
        {
            float lerp = Mathf.Clamp(((Time.time - this.latestDiffTime.Value) / 0.4f), 0.0f, 1.0f);
            lerp *= lerp * lerp;

            float scale = 1.0f + 2.0f * Mathf.Lerp(0.0f, 1.0f, 1.0f - lerp);
            text.fontSize = (int)(83 * scale);

            if (this.latestDiff > 0)
            {
                text.color = Color.Lerp(Color.white, Color.green, 1.0f - lerp);
            }
            else
            {
                text.color = Color.Lerp(Color.white, Color.red, 1.0f - lerp);
            }
        }
    }
}
