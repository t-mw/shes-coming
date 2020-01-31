using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameDisplay : MonoBehaviour
{
    public GameObject TargetCanvas;
    public GameObject DotPrefab;

    public int Count
    {
        get => _count;
        set
        {
            if (_count != value)
            {
                regenerate = true;
            }
            _count = value;
        }
    }
    private int _count = 5;

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

        for (var i = 0; i < this.Count; i++)
        {
            var dot = GameObject.Instantiate(this.DotPrefab);
            dot.transform.SetParent(this.TargetCanvas.transform);

            var rectTransform = dot.GetComponent<RectTransform>();
            var image = dot.GetComponent<UnityEngine.UI.Image>();

            if (i < this.CompletedCount)
            {
                image.color = new Color(0.0f, 1.0f, 0.0f);
            }

            var width = 500.0f;
            var x = this.Count > 1 ? (-0.5f * width + ((float)i / (this.Count - 1)) * width) : 0.0f;
            rectTransform.anchoredPosition = new Vector2(x, 0.0f);

            this.dotObjects.Add(dot);
        }
    }
}
