using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameDisplay : MonoBehaviour
{
    public GameObject TargetCanvas;
    public GameObject DotPrefab;
    public GameObject DotBackgroundPrefab;

    public List<KeyCode> KeyCodes
    {
        get => _keyCodes;
        set
        {
            _keyCodes = value;
        }
    }
    private List<KeyCode> _keyCodes = new List<KeyCode>();

    public int CompletedCount
    {
        get => _completedCount;
        set
        {
            _completedCount = value;
        }
    }
    private int _completedCount = 0;

    public bool regenerate = false;

    List<GameObject> dotObjects = new List<GameObject>();
    GameObject backgroundObject;

    private bool fadeIn = false;
    public float? fadeTime = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var originIndex = this.CompletedCount;

        for (var i = 0; i < this.KeyCodes.Count; i++)
        {
            var dot = this.dotObjects[i];
            var keyCode = this.KeyCodes[i];
            var minigameDot = dot.GetComponent<MinigameDot>();

            if (i == originIndex && !minigameDot.focusTime.HasValue)
            {
                minigameDot.focusTime = Time.time;
            }

            var alpha = 1.0f - Mathf.Clamp(Mathf.Abs(i - originIndex) / 3.0f, 0.0f, 1.0f);
            minigameDot.alpha = alpha;

            switch (keyCode)
            {
                case KeyCode.W:
                    minigameDot.arrowOrientation = ArrowOrientation.Up;
                    break;
                case KeyCode.A:
                    minigameDot.arrowOrientation = ArrowOrientation.Left;
                    break;
                case KeyCode.S:
                    minigameDot.arrowOrientation = ArrowOrientation.Down;
                    break;
                case KeyCode.D:
                    minigameDot.arrowOrientation = ArrowOrientation.Right;
                    break;
                default:
                    break;
            }

            if (i < this.CompletedCount)
            {
                minigameDot.state = MinigameDotState.Passed;
            }
            else
            {
                minigameDot.state = MinigameDotState.Normal;
            }
        }

        this.UpdatePositions();

        if (this.backgroundObject != null)
        {
            var backgroundImage = this.backgroundObject.GetComponent<UnityEngine.UI.Image>();
            var alpha = 0.5f * Utility.CalculateFade(this.fadeTime, 0.3f, this.fadeIn);
            backgroundImage.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }
    }

    public void FadeIn()
    {
        this.fadeIn = true;
        this.fadeTime = Time.time;

        foreach (var obj in this.dotObjects)
        {
            obj.GetComponent<MinigameDot>().FadeIn();
        }
    }

    public void FadeOut()
    {
        this.fadeIn = false;
        this.fadeTime = Time.time;

        foreach (var obj in this.dotObjects)
        {
            obj.GetComponent<MinigameDot>().FadeOut();
        }
    }

    void Clear()
    {
        foreach (var obj in this.dotObjects)
        {
            Destroy(obj);
        }
        this.dotObjects.Clear();
    }

    void UpdatePositions()
    {
        var originIndex = this.CompletedCount;
        for (var i = 0; i < this.dotObjects.Count; i++)
        {
            var obj = this.dotObjects[i];

            var separation = 100.0f;
            var x = -((i - originIndex) * separation);

            var localPosition = obj.transform.localPosition;
            localPosition.x = (localPosition.x + x) * 0.5f;
            obj.transform.localPosition = localPosition;
        }
    }

    public void Reset()
    {
        this.CompletedCount = 0;
        for (var i = 0; i < this.dotObjects.Count; i++)
        {
            var dot = this.dotObjects[i];
            var minigameDot = dot.GetComponent<MinigameDot>();
            minigameDot.focusTime = null;
        }
    }

    public void Recreate()
    {
        var background = GameObject.Instantiate(this.DotBackgroundPrefab);
        background.transform.SetParent(this.TargetCanvas.transform, false);
        this.backgroundObject = background;

        this.Clear();

        for (var i = 0; i < this.KeyCodes.Count; i++)
        {
            var dot = GameObject.Instantiate(this.DotPrefab);
            dot.name = "dot";
            dot.transform.SetParent(this.TargetCanvas.transform, false);
            this.dotObjects.Add(dot);
        }
    }
}
