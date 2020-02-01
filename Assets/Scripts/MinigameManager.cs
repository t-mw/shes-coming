using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public MinigameDisplay display;
    public bool IsComplete { get => this.sequence.IsComplete && !this.isTransitioning; }

    Sequence sequence = new Sequence();
    bool isTransitioning = false;

    // Start is called before the first frame update
    void Start()
    {
        this.BeginGame();
    }

    // Update is called once per frame
    void Update()
    {
        this.display.CompletedCount = this.sequence.currentIndex;

        if (!Input.anyKeyDown)
        {
            return;
        }

        foreach (KeyCode code in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(code))
            {
                if (code == this.sequence.CurrentKeyCode)
                {
                    this.display.failedIndex = null;
                    this.sequence.AdvanceIndex();
                    if (this.sequence.IsComplete)
                    {
                        // ...
                    }
                }
                else
                {
                    this.display.failedIndex = this.sequence.currentIndex;
                    this.display.CompletedCount = 0;

                    this.sequence.ResetIndex();
                }
            }
        }
    }

    public void EndGame()
    {
        this.isTransitioning = true;
        this.display.FadeOut();
    }

    public void BeginGame()
    {
        this.sequence.ResetIndex();
        this.sequence.Randomize();

        this.display.KeyCodes = new List<KeyCode>(this.sequence.keyCodes);
        this.display.Reset();

        this.isTransitioning = false;
        this.display.FadeIn();
    }
}

class Sequence
{
    public KeyCode? CurrentKeyCode { get => this.IsComplete ? (KeyCode?)null : this.keyCodes[this.currentIndex]; }
    public bool IsComplete { get => this.currentIndex == this.keyCodes.Count; }

    public List<KeyCode> keyCodes = new List<KeyCode> { KeyCode.W, KeyCode.W, KeyCode.S, KeyCode.S, KeyCode.D };
    public int currentIndex = 0;

    public void ResetIndex()
    {
        this.currentIndex = 0;
    }

    public void AdvanceIndex()
    {
        this.currentIndex += 1;
    }

    public void Randomize()
    {
        this.keyCodes.Clear();

        KeyCode[] keyCodes = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

        for (var i = 0; i < Random.Range(5, 10); i++)
        {
            KeyCode keyCode = keyCodes[Random.Range(0, keyCodes.Length - 1)];
            this.keyCodes.Add(keyCode);
        }
    }
}
