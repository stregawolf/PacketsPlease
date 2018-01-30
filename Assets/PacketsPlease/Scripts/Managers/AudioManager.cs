using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

//[RequireComponent(typeof(FMODUnity.StudioEventEmitter))]
public class AudioManager : Singleton<AudioManager> {

    public float m_intensity;

    public float GetIntensity() {
        return m_intensity;
    }

    public AudioClip m_notificationSelectedClip;
    public AudioClip m_notificationClosedClip;
    public AudioClip m_keyboardClip;
    public AudioClip m_computerStartUp;
    public AudioClip m_computerShutDown;
    public AudioClip m_strikeClip;
    public AudioClip m_loseClip;
    public AudioClip m_buttonClip;
    public AudioClip m_boostClip;
    public AudioClip m_throttleClip;
    public AudioClip m_disconnectClip;

    public AudioClip m_lowIntensityClip;
    public AudioClip m_medIntensityClip;
    public AudioClip m_highIntensityClip;

    private AudioSource m_lowIntensityTrack;
    private AudioSource m_medIntensityTrack;
    private AudioSource m_highIntensityTrack;

    private AudioSource m_audioTrack1;
    private AudioSource m_audioTrack2;

#if UNITY_WEBGL

    private const float LERP_TIME = 1.0f;

    protected override void Awake() {
        base.Awake();

        m_audioTrack1 = gameObject.AddComponent<AudioSource>();
        m_audioTrack1.loop = false;
        m_audioTrack2 = gameObject.AddComponent<AudioSource>();
        m_audioTrack2.loop = false;

        m_lowIntensityTrack = gameObject.AddComponent<AudioSource>();
        m_lowIntensityTrack.clip = m_lowIntensityClip;
        m_lowIntensityTrack.loop = true;

        m_medIntensityTrack = gameObject.AddComponent<AudioSource>();
        m_medIntensityTrack.clip = m_medIntensityClip;
        m_medIntensityTrack.loop = true;

        m_highIntensityTrack = gameObject.AddComponent<AudioSource>();
        m_highIntensityTrack.clip = m_highIntensityClip;
        m_highIntensityTrack.loop = true;

        EventManager.OnNotificationSelected.Register(PlayNotificationSelected);
        EventManager.OnNotificationResolved.Register(PlayNotificationClosed);
        EventManager.OnStrike.Register(OnStrikeGiven);
        EventManager.OnStartOfDay.Register(PlayStartOfDay);
        EventManager.OnStartGameplay.Register(StartGameplayTrack);
        EventManager.OnEndOfDay.Register(PlayEndOfDay);
        EventManager.OnLose.Register(PlayEndOfDay);
        EventManager.OnButtonClick.Register(PlayButtonClick);
        EventManager.OnBoost.Register(PlayBoost);
        EventManager.OnThrottle.Register(PlayThrottle);
        EventManager.OnDisconnect.Register(PlayDisconnect);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.OnNotificationSelected.Unregister(PlayNotificationSelected);
        EventManager.OnNotificationResolved.Unregister(PlayNotificationClosed);
        EventManager.OnStrike.Unregister(OnStrikeGiven);
        EventManager.OnStartOfDay.Unregister(PlayStartOfDay);
        EventManager.OnStartGameplay.Unregister(StartGameplayTrack);
        EventManager.OnEndOfDay.Unregister(PlayEndOfDay);
        EventManager.OnLose.Unregister(PlayEndOfDay);
        EventManager.OnButtonClick.Unregister(PlayButtonClick);
        EventManager.OnBoost.Unregister(PlayBoost);
        EventManager.OnThrottle.Unregister(PlayThrottle);
        EventManager.OnDisconnect.Unregister(PlayDisconnect);

        Destroy(m_audioTrack1);
        Destroy(m_audioTrack2);
        Destroy(m_lowIntensityTrack);
        Destroy(m_medIntensityTrack);
        Destroy(m_highIntensityTrack);
    }

    public void PlayAudioClip(AudioClip clip)
    {
        if(clip == null) return;
        m_audioTrack1.clip = clip;
        m_audioTrack1.Play();
    }

    public void PlayAudioClip2(AudioClip clip)
    {
        if(clip == null) return;
        m_audioTrack2.clip = clip;
        m_audioTrack2.Play();
    }

    public void StartGameplayTrack()
    {
        m_lowIntensityTrack.Play();
        m_medIntensityTrack.Play();
        m_highIntensityTrack.Play();
        SetIntensity(0f);
    }

    private void FadeInTrack(AudioSource track, float fadeTime)
    {
        StartCoroutine(HandleFadeInTrack(track, fadeTime));
    }

    private IEnumerator HandleFadeInTrack(AudioSource track, float fadeTime)
    {
        float t = 0f;

        while(t <= 1f)
        {
            t += Time.deltaTime / fadeTime;
            track.volume = t;
            yield return new WaitForEndOfFrame();
        }

        track.volume = 1f;
    }
    
    private void PlayNotificationSelected(NotificationUI obj)
    {
        PlayAudioClip(m_notificationSelectedClip);
    }

    private void PlayNotificationClosed(NotificationUI obj, bool arg2)
    {
        PlayAudioClip(m_notificationClosedClip);
    }

    public void PlayStartOfDay()
    {
        PlayAudioClip(m_keyboardClip);
        PlayAudioClip2(m_computerStartUp);
    }

    public void PlayEndOfDay()
    {
        StopGameplayTrack();
        PlayAudioClip(m_keyboardClip);
        PlayAudioClip2(m_computerShutDown);
    }

    public void PlayLose()
    {
        PlayAudioClip(m_loseClip);
    }

    // Generic button click
    public void PlayButtonClick()
    {
        PlayAudioClip(m_buttonClip);
    }

    // Special button clicks
    public void PlayBoost()
    {
        PlayAudioClip(m_boostClip);
    }

    public void PlayThrottle()
    {
        PlayAudioClip(m_throttleClip);
    }

    public void PlayDisconnect()
    {
        PlayAudioClip(m_disconnectClip);
    }

    private void StopGameplayTrack()
    {
        m_lowIntensityTrack.Stop();
        m_medIntensityTrack.Stop();
        m_highIntensityTrack.Stop();
        SetIntensityInstant(0f);
    }

    public void OnStrikeGiven(int currStrike) {
        PlayAudioClip2(m_strikeClip);
        float intensity = (float)currStrike / PacketsPleaseMain.Instance.m_maxStrikes;
        SetIntensity(intensity);
    }

    private void SetIntensity(float intensity) {
        StartCoroutine(HandleSetIntensity(intensity, LERP_TIME));
    }

    private void SetIntensityInstant(float intensity)
    {
        m_intensity = intensity;
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
            float currIntensity = Mathf.Lerp(startIntensity, intensity, t);
            SetIntensityInstant(currIntensity);
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
