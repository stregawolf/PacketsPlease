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
    public AudioClip m_positiveFeedbackClip;

    public AudioClip m_menuClip;
    public AudioClip m_lowIntensityClip1;
    public AudioClip m_medIntensityClip1;
    public AudioClip m_highIntensityClip1;
    public AudioClip m_lowIntensityClip2;
    public AudioClip m_medIntensityClip2;
    public AudioClip m_highIntensityClip2;
    public AudioClip m_lowIntensityClip3;
    public AudioClip m_medIntensityClip3;
    public AudioClip m_highIntensityClip3;

    private AudioSource m_menuTrack;

    private AudioSource m_lowIntensityTrack;
    private AudioSource m_medIntensityTrack;
    private AudioSource m_highIntensityTrack;

    private AudioSource m_audioTrack1;
    private AudioSource m_audioTrack2;
    private AudioSource m_audioTrack3;

    private const float LERP_TIME = 1.0f;

    protected override void Awake() {
        base.Awake();

        m_audioTrack1 = gameObject.AddComponent<AudioSource>();
        m_audioTrack1.loop = false;
        m_audioTrack2 = gameObject.AddComponent<AudioSource>();
        m_audioTrack2.loop = false;
        m_audioTrack3 = gameObject.AddComponent<AudioSource>();
        m_audioTrack3.loop = false;

        m_lowIntensityTrack = gameObject.AddComponent<AudioSource>();
        m_lowIntensityTrack.clip = m_lowIntensityClip1;
        m_lowIntensityTrack.loop = true;

        m_medIntensityTrack = gameObject.AddComponent<AudioSource>();
        m_medIntensityTrack.clip = m_medIntensityClip1;
        m_medIntensityTrack.loop = true;

        m_highIntensityTrack = gameObject.AddComponent<AudioSource>();
        m_highIntensityTrack.clip = m_highIntensityClip1;
        m_highIntensityTrack.loop = true;

        m_menuTrack = gameObject.AddComponent<AudioSource>();
        m_menuTrack.clip = m_menuClip;
        m_menuTrack.loop = true;
        m_menuTrack.Play();

        EventManager.OnNotificationSelected.Register(PlayNotificationSelected);
        EventManager.OnNotificationResolved.Register(PlayNotificationClosed);
        EventManager.OnStrike.Register(OnStrikeGiven);
        EventManager.OnCorrectChoice.Register(OnCorrectChoice);
        EventManager.OnStartOfDay.Register(PlayStartOfDay);
        EventManager.OnStartGameplay.Register(StartGameplayTrack);
        EventManager.OnEndOfDayReport.Register(PlayEndOfDay);
        EventManager.OnLose.Register(PlayLose);
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
        EventManager.OnCorrectChoice.Unregister(OnCorrectChoice);
        EventManager.OnStartOfDay.Unregister(PlayStartOfDay);
        EventManager.OnStartGameplay.Unregister(StartGameplayTrack);
        EventManager.OnEndOfDayReport.Unregister(PlayEndOfDay);
        EventManager.OnLose.Unregister(PlayLose);
        EventManager.OnButtonClick.Unregister(PlayButtonClick);
        EventManager.OnBoost.Unregister(PlayBoost);
        EventManager.OnThrottle.Unregister(PlayThrottle);
        EventManager.OnDisconnect.Unregister(PlayDisconnect);

        Destroy(m_audioTrack1);
        Destroy(m_audioTrack2);
        Destroy(m_audioTrack3);
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

    public void PlayAudioClip3(AudioClip clip)
    {
        if(clip == null) return;
        m_audioTrack3.clip = clip;
        m_audioTrack3.Play();
    }

    public void StartGameplayTrack(int day)
    {
        // TODO(jcazamias): You hacked in the -1 to make this work. C'mon. Get it together.
        int groupNumber = ((day - 1) % 3) + 1;

        if(groupNumber == 1)
        {
            m_lowIntensityTrack.clip = m_lowIntensityClip1;
            m_medIntensityTrack.clip = m_medIntensityClip1;
            m_highIntensityTrack.clip = m_highIntensityClip1;
        }
        else if(groupNumber == 2)
        {
            m_lowIntensityTrack.clip = m_lowIntensityClip2;
            m_medIntensityTrack.clip = m_medIntensityClip2;
            m_highIntensityTrack.clip = m_highIntensityClip2;
        }
        else if(groupNumber == 3)
        {
            m_lowIntensityTrack.clip = m_lowIntensityClip3;
            m_medIntensityTrack.clip = m_medIntensityClip3;
            m_highIntensityTrack.clip = m_highIntensityClip3;
        }

        m_menuTrack.Stop();
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
        PlayNotificationClosed();
    }

    public void PlayNotificationClosed()
    {
        PlayAudioClip(m_notificationClosedClip);
    }

    public void PlayStartOfDay(int day)
    {
        if(!m_menuTrack.isPlaying)
        {
            m_menuTrack.Play();
        }
        PlayAudioClip2(m_keyboardClip);
        PlayAudioClip3(m_computerStartUp);
    }

    public void PlayEndOfDay(int day)
    {
        StopGameplayTrack();
        PlayAudioClip(m_keyboardClip);
        PlayAudioClip2(m_computerShutDown);
    }

    public void PlayLose()
    {
        StopGameplayTrack();
        PlayAudioClip2(m_loseClip);
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

    public void OnCorrectChoice()
    {
        PlayAudioClip2(m_positiveFeedbackClip);
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
}
