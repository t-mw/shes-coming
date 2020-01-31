using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    public MinigameDisplay display;

    Sequence sequence = new Sequence();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        this.display.Count = this.sequence.keyCodes.Count;
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
                    this.sequence.AdvanceIndex();
                    if (this.sequence.IsComplete)
                    {
                        // ...
                    }
                }
                else
                {
                    this.sequence.ResetIndex();
                }
            }
        }
    }
}

class Sequence
{
    public KeyCode? CurrentKeyCode { get => this.IsComplete ? (KeyCode?)null : this.keyCodes[this.currentIndex]; }
    public bool IsComplete { get => this.currentIndex == this.keyCodes.Count; }

    public List<KeyCode> keyCodes = new List<KeyCode> { KeyCode.A, KeyCode.A, KeyCode.B, KeyCode.B };
    public int currentIndex = 0;

    public void ResetIndex()
    {
        this.currentIndex = 0;
    }

    public void AdvanceIndex()
    {
        this.currentIndex += 1;
    }
}
