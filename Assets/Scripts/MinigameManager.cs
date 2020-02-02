using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public ScoreDisplay scoreDisplay;
    public MinigameDisplay gameDisplay;
    public bool IsComplete { get => this.sequence.IsComplete && !this.isTransitioning; }
    public float CompleteFraction { get => (float)this.sequence.currentIndex / this.sequence.keyCodes.Count; }
    public int RemainingCount { get => this.sequence.keyCodes.Count - this.sequence.currentIndex; }

    int stage = 0;
    Sequence sequence = new Sequence();
    bool isTransitioning = false;
    MinigameActions minigameActions;

    // Start is called before the first frame update
    void Start()
    {
        minigameActions = new MinigameActions();

        minigameActions.Left.AddDefaultBinding(Key.LeftArrow);
        minigameActions.Left.AddDefaultBinding(Key.A);
        minigameActions.Left.AddDefaultBinding(InputControlType.DPadLeft);

        minigameActions.Right.AddDefaultBinding(Key.RightArrow);
        minigameActions.Right.AddDefaultBinding(Key.D);
        minigameActions.Right.AddDefaultBinding(InputControlType.DPadRight);

        minigameActions.Up.AddDefaultBinding(Key.UpArrow);
        minigameActions.Up.AddDefaultBinding(Key.W);
        minigameActions.Up.AddDefaultBinding(InputControlType.DPadUp);

        minigameActions.Down.AddDefaultBinding(Key.DownArrow);
        minigameActions.Down.AddDefaultBinding(Key.S);
        minigameActions.Down.AddDefaultBinding(InputControlType.DPadDown);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameDisplay.CompletedCount = this.sequence.currentIndex;

        if (this.isTransitioning)
        {
            return;
        }

        bool leftPressed = this.minigameActions.Left.WasPressed;
        bool rightPressed = this.minigameActions.Right.WasPressed;
        bool upPressed = this.minigameActions.Up.WasPressed;
        bool downPressed = this.minigameActions.Down.WasPressed;

        if (leftPressed || rightPressed || upPressed || downPressed)
        {
            if ((leftPressed && this.sequence.CurrentKeyCode == KeyCode.A) ||
                (rightPressed && this.sequence.CurrentKeyCode == KeyCode.D) ||
                (upPressed && this.sequence.CurrentKeyCode == KeyCode.W) ||
                (downPressed && this.sequence.CurrentKeyCode == KeyCode.S))
            {
                this.sequence.AdvanceIndex();
            }
            else
            {
                this.scoreDisplay.score -= 2;
                this.gameDisplay.Reset();

                this.sequence.ResetIndex();
            }
        }
    }

    public void EndGame()
    {
        this.scoreDisplay.score += 10;

        this.isTransitioning = true;
        this.gameDisplay.FadeOut();
    }

    public void BeginGame()
    {
        this.stage += 1;
        int sequenceLength = 4 + 2 * (this.stage - 1);

        this.sequence.ResetIndex();
        this.sequence.Randomize(sequenceLength);

        this.gameDisplay.KeyCodes = new List<KeyCode>(this.sequence.keyCodes);
        this.gameDisplay.Recreate();

        this.isTransitioning = false;
        this.gameDisplay.FadeIn();
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

    public void Randomize(int count)
    {
        this.keyCodes.Clear();

        KeyCode[] keyCodes = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

        for (var i = 0; i < count; i++)
        {
            KeyCode keyCode = keyCodes[Random.Range(0, keyCodes.Length - 1)];
            this.keyCodes.Add(keyCode);
        }
    }
}

public class MinigameActions : PlayerActionSet
{
    public PlayerAction Left;
    public PlayerAction Right;
    public PlayerAction Up;
    public PlayerAction Down;

    public MinigameActions()
    {
        Left = CreatePlayerAction("Left");
        Right = CreatePlayerAction("Right");
        Up = CreatePlayerAction("Up");
        Down = CreatePlayerAction("Down");
    }
}