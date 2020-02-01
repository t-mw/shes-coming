using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameDisplay : MonoBehaviour
{
    public GameObject TargetCanvas;
    public GameObject DotPrefab;

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
    public int? failedIndex = null;

    List<GameObject> dotObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < this.KeyCodes.Count; i++)
        {
            var dot = this.dotObjects[i];
            var keyCode = this.KeyCodes[i];

            var minigameDot = dot.GetComponent<MinigameDot>();
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

            if (i == this.failedIndex)
            {
                minigameDot.State = MinigameDotState.Failed;
            }
            else if (i < this.CompletedCount)
            {
                minigameDot.State = MinigameDotState.Passed;
            }
            else
            {
                minigameDot.State = MinigameDotState.Normal;
            }
        }
    }

    public void FadeIn()
    {
        foreach (var obj in this.dotObjects)
        {
            obj.GetComponent<MinigameDot>().FadeIn();
        }
    }

    public void FadeOut()
    {
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

    public void Reset()
    {
        this.Clear();

        for (var i = 0; i < this.KeyCodes.Count; i++)
        {
            var dot = GameObject.Instantiate(this.DotPrefab);
            dot.name = "dot";
            dot.transform.SetParent(this.TargetCanvas.transform, false);

            var width = 700.0f;
            var x = this.KeyCodes.Count > 1 ?
                (-0.5f * width + ((float)i / (this.KeyCodes.Count - 1)) * width) :
                0.0f;
            dot.transform.localPosition = new Vector3(x, 0.0f, 0.0f);

            this.dotObjects.Add(dot);
        }
    }
}
