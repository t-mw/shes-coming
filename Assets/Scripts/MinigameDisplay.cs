using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameDisplay : MonoBehaviour
{
    public GameObject TargetCanvas;
    public GameObject DotCirclePrefab;
    public GameObject DotTextPrefab;

    public List<KeyCode> KeyCodes
    {
        get => _keyCodes;
        set
        {
            regenerate = true;
            _keyCodes = value;
        }
    }
    private List<KeyCode> _keyCodes = new List<KeyCode>();

    public int CompletedCount
    {
        get => _completedCount;
        set
        {
            if (_completedCount != value)
            {
                regenerate = true;
            }
            _completedCount = value;
        }
    }
    private int _completedCount = 3;

    public int? FailedIndex = null;

    private bool regenerate = true;

    List<GameObject> dotObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        this.Regenerate();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.regenerate)
        {
            this.Regenerate();
            this.regenerate = false;
        }
    }

    void Regenerate()
    {
        foreach (var obj in this.dotObjects)
        {
            Destroy(obj);
        }
        this.dotObjects.Clear();

        for (var i = 0; i < this.KeyCodes.Count; i++)
        {
            var keyCode = this.KeyCodes[i];

            var dot = new GameObject();
            dot.name = "dot";

            dot.transform.SetParent(this.TargetCanvas.transform, false);

            var circle = GameObject.Instantiate(this.DotCirclePrefab);
            circle.transform.SetParent(dot.transform, false);

            var text = GameObject.Instantiate(this.DotTextPrefab);
            text.transform.SetParent(dot.transform, false);

            text.GetComponent<UnityEngine.UI.Text>().text = keyCode.ToString();

            var image = circle.GetComponent<UnityEngine.UI.Image>();
            if (i == this.FailedIndex)
            {
                image.color = new Color(1.0f, 0.0f, 0.0f);
            }
            else if (i < this.CompletedCount)
            {
                image.color = new Color(0.0f, 1.0f, 0.0f);
            }

            var width = 500.0f;
            var x = this.KeyCodes.Count > 1 ?
                (-0.5f * width + ((float)i / (this.KeyCodes.Count - 1)) * width) :
                0.0f;

            var rectTransform1 = circle.GetComponent<RectTransform>();
            rectTransform1.anchoredPosition = new Vector2(x, 0.0f);

            var rectTransform2 = text.GetComponent<RectTransform>();
            rectTransform2.anchoredPosition = new Vector2(x, 0.0f);

            this.dotObjects.Add(dot);
        }
    }
}
