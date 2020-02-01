using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraLogic : MonoBehaviour
{

    [System.Serializable]
    public struct PlayableObject
    {
        public string objectID;
        public CinemachineVirtualCamera virtualCamera;
        public Animation animationScript;
    }

    public List<PlayableObject> playableObjects;
    private PlayableObject currentObject;

    [Header("SOUND STUFF")]
    public SoundManager soundManager;
    private bool wasOnTapeSFX = false;
    private float currentFraction = 0f;

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

    private float timeOnStart;
    private float timeFromStart;

    [Header("TIMER STUFF")]
    public Image TimerImage;

    [Header("MINIGAME STUFF")]
    public MinigameManager minigameManager;

    [Header("GRAND FINALE")]
    public GameObject FinalScreen;
    public Text FinalScreenTextObject;
    public string badFinalText;
    public string goodFinalText;

    public int objectsToSolveCount;
    public int solvedObjectsCount = 0;

    private bool onFinalScreen = false;
    private bool isBlendComplete = false;

    private int repairStage = 0;

    private void Start()
    {
        currentObject = playableObjects[0];
        currentObject.virtualCamera.Priority = 10;

        objectsToSolveCount = playableObjects.Count;
        timeOnStart = Time.time;
        TimerImage.fillAmount = 0f;
        FinalScreen.SetActive(false);
        currentCamera = startCamera;
        currentCamerasNoise = currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void GoNextObject()
    {
        repairStage = 0;

        if (playableObjects.Count > 1)
        {
            solvedObjectsCount++;
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
            solvedObjectsCount++;
            onFinalScreen = true;
            FinalScreenOn();
        }
    }

    private void Update()
    {
        
        if (!onFinalScreen)
        {
            timeFromStart = Time.time - timeOnStart;
            currentFraction = minigameManager.CompleteFraction;
            currentObject.animationScript.SetGameProgress(currentFraction);
            TapingSound();

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
        if (isBlendComplete && !this.isBlendComplete)
        {
            this.NextObjectReachedOn();
        }
        this.isBlendComplete = isBlendComplete;

        if (this.minigameManager.IsComplete)
        {
            if (!onFinalScreen)
            {
                this.minigameManager.EndGame();

                GoNextObject();
                
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }

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

    public void NextObjectReachedOn()
    {
        this.minigameManager.BeginGame();
    }

    public void FinalScreenOn()
    {

        this.minigameManager.EndGame();
        if (solvedObjectsCount == objectsToSolveCount)
        {
            FinalScreenTextObject.text = goodFinalText.Replace("\\n", "\n");
        }
        else
        {
            FinalScreenTextObject.text = badFinalText.Replace("\\n", "\n");
        }

        FinalScreen.SetActive(true);
    }


    public void TapingSound()
    {
        if (currentFraction < 0.25f)
        {
            repairStage = 0;
        }

       

        switch (repairStage)
        {

            case 0:
                if (currentFraction > 0.25f)
                {
                    PlayTapingSound();
                }
                break;
            case 1:
                if (currentFraction > 0.5f)
                {
                    PlayTapingSound();
                }
                break;
            case 2:
                if (currentFraction > 0.75f)
                {
                    PlayTapingSound();
                }
                break;
            case 3:
                if (currentFraction > 0.9f)
                {
                    PlayTapingSound();
                }
                break;

            default:
                break;
        }
    }

    public void PlayTapingSound()
    {
        repairStage++;
        soundManager.Taping();
    }
}
