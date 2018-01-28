using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//[RequireComponent(typeof(FMODUnity.StudioEventEmitter))]
public class AudioManager : Singleton<AudioManager> {

    public float m_intensity;

    public float GetIntensity() {
        return m_intensity;
    }

#if UNITY_WEBGL

    public AudioClip m_lowIntensityClip;
    public AudioClip m_medIntensityClip;
    public AudioClip m_highIntensityClip;


    private AudioSource m_lowIntensityTrack;
    private AudioSource m_medIntensityTrack;
    private AudioSource m_highIntensityTrack;

    private const float LERP_TIME = 1.0f;

    protected override void Awake() {
        base.Awake();

        m_lowIntensityTrack = gameObject.AddComponent<AudioSource>();
        m_lowIntensityTrack.clip = m_lowIntensityClip;
        m_lowIntensityTrack.loop = true;
        m_lowIntensityTrack.Play();


        m_medIntensityTrack = gameObject.AddComponent<AudioSource>();
        m_medIntensityTrack.clip = m_medIntensityClip;
        m_medIntensityTrack.loop = true;
        m_medIntensityTrack.Play();


        m_highIntensityTrack = gameObject.AddComponent<AudioSource>();
        m_highIntensityTrack.clip = m_highIntensityClip;
        m_highIntensityTrack.loop = true;
        m_highIntensityTrack.Play();

        SetIntensity(0f);

        EventManager.OnStrike.Register(OnStrikeGiven);
    }

    private void OnStrikeGiven(int currStrike) {
        float intensity = (float)currStrike / PacketsPleaseMain.Instance.m_maxStrikes;
        StartCoroutine(HandleSetIntensity(intensity, LERP_TIME));
    }

    private void SetIntensity(float intensity) {
        m_intensity = Mathf.Clamp01(intensity);
        m_lowIntensityTrack.volume = 1.0f;
        m_medIntensityTrack.volume = Mathf.Clamp01(intensity * 3f);
        m_highIntensityTrack.volume = Mathf.Clamp01((intensity * 3f) - 1f);

    }

    private IEnumerator HandleSetIntensity(float intensity, float lerpTime)
    {
        float t = 0f;
        float startIntensity = m_intensity;

        while(t < 1f)
        {
            t += Time.deltaTime;
            SetIntensity(Mathf.Lerp(startIntensity, intensity, t));
            yield return new WaitForEndOfFrame();
        }
    }

    public void Update() {
        
    }

#elif !UNITY_WEBGL
    private FMODUnity.StudioEventEmitter m_emitter;

    #region FMOD Project Parameters

    private const string INTENSITY_PARAM_NAME = "Intensity";

    private const float LOOP_WAIT_TIME = 10.0f;
    private const string LOSE_TRIGGER_NAME = "Lose";
    private const string MELODY_TRIGGER_NAME = "Melody";
    private const string START_GAME_TRIGGER_NAME = "StartGame";
    private const string HIGH_INTENSITY_TRIGGER_NAME = "HighIntensity";
    private const string LOW_INTENSITY_TRIGGER_NAME = "LowIntensity";

    public void SetIntensity() {
    //TODO
    }

    public void TriggerLose()
    {
        m_emitter.SetParameter(LOSE_TRIGGER_NAME, 1f);
    }

    public void TriggerMelody()
    {
        m_emitter.SetParameter(MELODY_TRIGGER_NAME, 1f);
    }

    public void TriggerStartGame()
    {
        m_emitter.SetParameter(START_GAME_TRIGGER_NAME, 1f);
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        m_emitter = AddComponent<FMODUnity.StudioEventEmitter>();
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
#endif
}
