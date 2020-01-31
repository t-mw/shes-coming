﻿using System.Collections;
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

    public int? FailedIndex = null;

    List<GameObject> dotObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < this.KeyCodes.Count; i++)
        {
            var dot = GameObject.Instantiate(this.DotPrefab);
            dot.name = "dot";
            dot.transform.SetParent(this.TargetCanvas.transform, false);

            var width = 500.0f;
            var x = this.KeyCodes.Count > 1 ?
                (-0.5f * width + ((float)i / (this.KeyCodes.Count - 1)) * width) :
                0.0f;
            dot.transform.localPosition = new Vector3(x, 0.0f, 0.0f);

            this.dotObjects.Add(dot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < this.KeyCodes.Count; i++)
        {
            var dot = this.dotObjects[i];
            var keyCode = this.KeyCodes[i];

            var minigameDot = dot.GetComponent<MinigameDot>();
            minigameDot.text = keyCode.ToString();
            if (i == this.FailedIndex)
            {
                minigameDot.state = MinigameDotState.Failed;
            }
            else if (i < this.CompletedCount)
            {
                minigameDot.state = MinigameDotState.Passed;
            }
            else
            {
                minigameDot.state = MinigameDotState.Normal;
            }
        }
    }
}
