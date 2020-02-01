using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundManager : MonoBehaviour
{
    public StudioEventEmitter emitter;
    public string taping;

    public void PlayOneShotSfx(string _eventName)
    {
        if (CheckEventFMOD(_eventName))
        {
            RuntimeManager.PlayOneShot(_eventName, transform.position);
        }
        else
        {
            Debug.Log(_eventName + " is not found in FMOD Project");
        }
        
    }
    
    public void PlayLoopedSFX(string _eventName)
    {
        if (CheckEventFMOD(_eventName))
        {
            emitter.Event = _eventName;
            emitter.Play();
        }
        else
        {
            Debug.Log(_eventName + " is not found in FMOD Project");
        }
    }

    public bool CheckEventFMOD(string _eventName)
    {
        FMOD.Studio.EventDescription ed;
        RuntimeManager.StudioSystem.getEvent(_eventName, out ed);
        if (ed.isValid())
        {
            return true;
        }
        else return false;
    }

    public void StopLoopedSFX()
    {
        emitter.Stop();
    }

    private void OnDisable()
    {
        emitter.Stop();
    }

    public void PlayTape()
    {
        PlayOneShotSfx(taping);
    }
}
