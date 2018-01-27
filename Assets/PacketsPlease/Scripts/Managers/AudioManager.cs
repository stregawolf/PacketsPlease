using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(FMODUnity.StudioEventEmitter))]
public class AudioManager : Singleton<AudioManager> {

    private FMODUnity.StudioEventEmitter m_emitter;
    private const string INTENSITY_PARAM_NAME = "Intensity";
    private const string TONE_PARAM_NAME = "Tone";

    #region FMOD Project Parameters
    public float Intensity
    {
        get { return m_intensity; }
        set
        { 
            m_intensity = value;
            m_emitter.SetParameter(INTENSITY_PARAM_NAME, value);
        }
    }
    private float m_intensity;

    public float Tone
    {
        get { return m_tone; }
        set 
        { 
            m_tone = value;
            m_emitter.SetParameter(TONE_PARAM_NAME, value);
        }
    }
    private float m_tone;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        m_emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    public void PlayEvent(FMODUnity.EmitterGameEvent playEvent)
    {
        m_emitter.PlayEvent = playEvent;
        m_emitter.Play();
    }

    public void Stop()
    {
        m_emitter.Stop();
    }
}
