using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedIndicatorUI : MonoBehaviour {
    public Image m_bg;
    public Image m_arrow;

    public float m_arrowSpeed = 3.0f;
    public Color m_slowColor = Color.red;
    public Color m_fastColor = Color.green;

    public enum SpeedState
    {
        Regular,
        Throttled,
        Boosted,
        Disconnected,
    }

    public SpeedState m_currentState { get; protected set; }
    protected float m_currentAngle = 0.0f;
    protected float m_desiredAngle = 0.0f;

    public void Reset()
    {
        m_currentState = SpeedState.Regular;
    }

    public void Boost()
    {
        m_currentState = SpeedState.Boosted;
    }

    public void Throttle()
    {
        m_currentState = SpeedState.Throttled;
    }

    public void Disconnect()
    {
        m_currentState = SpeedState.Disconnected;
    }

    protected void Update()
    {
        switch(m_currentState)
        {
            case SpeedState.Regular:
                m_desiredAngle = Random.Range(-10.0f, 10.0f);
                break;
            case SpeedState.Boosted:
                m_desiredAngle = -80.0f + Random.Range(-10.0f, 50.0f);
                break;
            case SpeedState.Throttled:
                m_desiredAngle = 80.0f + Random.Range(-50.0f, 10.0f);
                break;
            case SpeedState.Disconnected:
                m_desiredAngle = 90.0f;
                break;
        }

        m_currentAngle = Mathf.Lerp(m_currentAngle, m_desiredAngle, Time.deltaTime * m_arrowSpeed);
        m_arrow.color = Color.Lerp(m_fastColor, m_slowColor, (m_currentAngle + 90.0f) / 180.0f);
        m_arrow.transform.localEulerAngles = new Vector3(0, 0, m_currentAngle);
    }
}
