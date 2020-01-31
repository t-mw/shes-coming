using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraLogic : MonoBehaviour
{
    [Header("CAMERAS STUFF")]
    public CinemachineBrain cameraBrain;
    public List<CinemachineVirtualCamera> virtualCameras;
    public CinemachineVirtualCamera startCamera;

    private CinemachineVirtualCamera currentCamera;
    private CinemachineBasicMultiChannelPerlin currentCamerasNoise;

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


    [Header("GRAND FINALE")]
    public GameObject FinalScreen;
    public Text FinalScreenTextObject;
    public string badFinalText;
    public string goodFinalText;

    private int objectsToSolveCount;
    private int solvedObjectsCount = 0;
  
    private bool onFinalScreen = false;

    private void Start()
    {
        objectsToSolveCount = virtualCameras.Count;
        timeOnStart = Time.time;
        TimerImage.fillAmount = 0f;
        FinalScreen.SetActive(false);
        currentCamera = startCamera;
        currentCamerasNoise = currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
    }

    public void GoNextObject()
    {
        if (virtualCameras.Count > 1)
        {
            virtualCameras.Remove(currentCamera);
            cameraBrain.ActiveVirtualCamera.Priority = 0;

            int nextCameraIndex = Random.Range(0, virtualCameras.Count);
            virtualCameras[nextCameraIndex].Priority = 10;

            currentCamera = virtualCameras[nextCameraIndex];
            currentCamerasNoise = currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        else
        {
            FinalScreenOn();
        }
    }

    private void Update()
    {

        timeFromStart = Time.time - timeOnStart;

        if (timeFromStart <= 20f)
        {
            TimerImage.fillAmount = timeFromStart / 20f;
        }
        else
        {
            FinalScreenOn();
        }


        if (Input.anyKeyDown)
        {
            if (!onFinalScreen)
            {
                GoNextObject();
                solvedObjectsCount++;
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

    public void FinalScreenOn()
    {
        onFinalScreen = true;

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


}
