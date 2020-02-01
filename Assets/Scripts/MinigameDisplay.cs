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

        this.UpdatePositions();
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
        this.Clear();

        for (var i = 0; i < this.KeyCodes.Count; i++)
        {
            var obj = GameObject.Instantiate(this.DotPrefab);
            obj.name = "dot";
            obj.transform.SetParent(this.TargetCanvas.transform, false);
            this.dotObjects.Add(obj);
        }
    }
}
