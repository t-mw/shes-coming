using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using UnityEngine.Playables;

public class CameraLogic : MonoBehaviour
{

    [System.Serializable]
    public struct PlayableObject
    {
        public string objectID;
        public CinemachineVirtualCamera virtualCamera;
        public Animation animationScript;
        public Image repairImage;
    }

    public List<PlayableObject> playableObjects;
    private PlayableObject currentObject;

    [Header("SOUND STUFF")]
    public SoundManager soundManager;
    private bool wasOnTapeSFX = false;
    public AudioSource music;

    [Header("CAMERAS STUFF")]
    public CinemachineBrain cameraBrain;
    public CinemachineVirtualCamera startCamera;

    private CinemachineVirtualCamera currentCamera;
    private CinemachineBasicMultiChannelPerlin currentCamerasNoise;
    private CinemachineBlend activeBlend = null;

    [Range(0, 30)]
    public float amplitudeGain;
    public float amplitudeSpeed = 0.5f;
    private float amplFraction = 0f;
    private float amplStart = 0f;

    [Range(0, 30)]
    public float frequencyGain;
    public float frequencySpeed = 0.5f;
    private float freqFraction = 0f;
    private float freqStart = 0f;

    private float timeOnStart = 0f;
    private float timeFromStart;

    [Header("TIMER STUFF")]
    public Image TimerImage;
    public PlayableDirector EndTimeline;

    [Header("MINIGAME STUFF")]
    public MinigameManager minigameManager;

    [Header("GRAND FINALE")]
    public GameObject FinalScreen;
    public Text FinalScreenDescriptionTextObject;
    public Text FinalScreenAdvanceTextObject;
    public string badFinalText;
    public string goodFinalText;
    MenuActions menuActions;

    private int objectsToSolveCount;
    private int solvedObjectsCount = 0;

    private bool onFinalScreen = false;
    private bool isBlendComplete = false;

    public int repairStage = 0;
    public bool startGame = false;

    private void Start()
    {
        currentObject = playableObjects[0];


        objectsToSolveCount = playableObjects.Count;

        TimerImage.fillAmount = 0f;
        FinalScreen.SetActive(false);
        currentCamera = startCamera;
        currentCamerasNoise = currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        this.menuActions = new MenuActions();
        this.menuActions.AdvanceMenu.AddDefaultBinding(Key.Space);
        this.menuActions.AdvanceMenu.AddDefaultBinding(InputControlType.DPadX);

        int nextObjectIndex = Random.Range(0, playableObjects.Count);
        playableObjects[nextObjectIndex].virtualCamera.Priority = 10;

        currentCamera = playableObjects[nextObjectIndex].virtualCamera;
        currentCamerasNoise = currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        currentObject = playableObjects[nextObjectIndex];
        currentObject.virtualCamera.Priority = 10;
    }

    public void StartGame()
    {
        startGame = true;
        isBlendComplete = true;
        this.music.Play();
    }

    public void GoNextObject()
    {

        currentObject.animationScript.Repaired();
        if (playableObjects.Count > 1)
        {
            playableObjects.Remove(currentObject);
            cameraBrain.ActiveVirtualCamera.Priority = 0;

            int nextObjectIndex = Random.Range(0, playableObjects.Count);
            playableObjects[nextObjectIndex].virtualCamera.Priority = 10;

            currentCamera = playableObjects[nextObjectIndex].virtualCamera;
            currentCamerasNoise = currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            currentObject = playableObjects[nextObjectIndex];
        }
        else
        {
            onFinalScreen = true;
            FinalScreenOn();
        }
    }

    private void Update()
    {
        if (startGame)
        {
            if (timeOnStart == 0)
            {
                timeOnStart = Time.time;
                minigameManager.BeginGame();
            }
            timeFromStart = Time.time - timeOnStart;



            if (this.minigameManager.IsComplete)
            {
                solvedObjectsCount++;
            }

            if (!onFinalScreen)
            {
                timeFromStart = Time.time - timeOnStart;

                if (this.cameraBrain.ActiveBlend == null) TapingSound();

                if (timeFromStart <= 20f)
                {
                    TimerImage.fillAmount = timeFromStart / 20f;
                }
                else
                {
                    onFinalScreen = true;
                    FinalScreenOn();
                }
            }

            if (this.cameraBrain.ActiveBlend != null)
            {
                this.activeBlend = this.cameraBrain.ActiveBlend;
            }
            var isBlendComplete = this.activeBlend != null && this.activeBlend.IsComplete;
            if (isBlendComplete && !this.isBlendComplete && !this.onFinalScreen)
            {
                this.NextObjectReachedOn();
            }
            this.isBlendComplete = isBlendComplete;

            if (this.minigameManager.IsComplete && !onFinalScreen)
            {
                this.EndGame();
                GoNextObject();
            }

            if (this.onFinalScreen)
            {
                currentCamerasNoise.m_FrequencyGain = freqStart;
                currentCamerasNoise.m_AmplitudeGain = amplStart;

            }
            else
            {
                if (freqFraction < 1)
                {
                    freqFraction = timeFromStart / 20f;
                    currentCamerasNoise.m_FrequencyGain = Mathf.Lerp(freqStart, frequencyGain, freqFraction);
                }
                if (amplFraction < 1)
                {
                    amplFraction = timeFromStart / 20f;
                    currentCamerasNoise.m_AmplitudeGain = Mathf.Lerp(amplStart, amplitudeGain, amplFraction);
                }
            }

            if (this.onFinalScreen && this.menuActions.AdvanceMenu.WasPressed)
            {
                SceneManager.LoadScene(0);
            }

        }
    }

    public void NextObjectReachedOn()
    {
        this.minigameManager.BeginGame();
    }

    void EndGame()
    {
        repairStage = 0;
        this.minigameManager.EndGame();
        this.currentObject.repairImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    public void FinalScreenOn()
    {
        this.EndTimeline.Play();

        this.EndGame();
        if (solvedObjectsCount == objectsToSolveCount)
        {
            StartCoroutine(Utility.DelaySeconds(() => this.soundManager.GoodEnd(), 1.5f));
            FinalScreenDescriptionTextObject.text = goodFinalText.Replace("\\n", "\n");
        }
        else
        {
            StartCoroutine(Utility.DelaySeconds(() => this.soundManager.BadEnd(), 1.5f));
            FinalScreenDescriptionTextObject.text = badFinalText.Replace("\\n", "\n");
        }

        string keyName = null;
        foreach (var binding in this.menuActions.AdvanceMenu.Bindings)
        {
            var keyBindingSource = binding as KeyBindingSource;
            if (keyBindingSource != null)
            {
                var keyCombo = keyBindingSource.Control;
                for (var keyIndex = 0; keyIndex < keyCombo.IncludeCount; keyIndex++)
                {
                    var key = keyCombo.GetInclude(keyIndex); // returns enum Key
                    var keyInfo = KeyInfo.KeyList[(int)key];
                    keyName = keyInfo.Name;
                    break;
                }

            }
        }

        FinalScreenAdvanceTextObject.text = $"Press {keyName} to restart";

        FinalScreen.SetActive(true);
    }



    public void TapingSound()
    {
        float currentFraction = minigameManager.CompleteFraction;
        int remainingCount = minigameManager.RemainingCount;
        if (currentFraction < 0.25f)
        {
            if (repairStage != 0)
            {
                currentObject.animationScript.PlayBackAnimation();
                repairStage = 0;
                soundManager.Crashing();
                Debug.Log("wrong button");
            }
        }

        if (remainingCount == 1 && repairStage != 3)
        {
            currentObject.animationScript.PlayToEnd();
            soundManager.Taping();
            repairStage = 3;
            return;
        }

        switch (repairStage)
        {

            case 0:
                if (currentFraction > 0.3f)
                {
                    PlayTapingSound();
                    repairStage++;
                }
                break;
            case 1:
                if (currentFraction > 0.6f)
                {
                    PlayTapingSound();
                    repairStage++;
                }
                break;
            case 2:
                if (currentFraction > 0.8f)
                {
                    PlayTapingSound();
                    repairStage++;
                }
                break;

            default:
                break;
        }
    }

    public void PlayTapingSound()
    {

        currentObject.animationScript.NextAnimation();
        soundManager.Taping();

    }
}

