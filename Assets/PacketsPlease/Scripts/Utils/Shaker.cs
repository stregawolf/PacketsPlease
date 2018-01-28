using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour {

    protected float m_shakeTime = 0.0f;
    protected float m_shakeIntensity;

    protected Vector3 m_originalPos;

    protected void Awake()
    {
        m_originalPos = transform.localPosition;
    }

    protected void Update()
    {
        if(m_shakeTime > 0.0f)
        {
            m_shakeTime -= Time.deltaTime;

            Vector3 toPos = m_originalPos;

            if (m_shakeTime <= 0.0f)
            {
                transform.localPosition = m_originalPos;
            }
            else
            {
                toPos.x += Random.Range(-m_shakeIntensity, m_shakeIntensity);
                toPos.y += Random.Range(-m_shakeIntensity, m_shakeIntensity);

                transform.localPosition = Vector3.Lerp(transform.localPosition, toPos, Time.deltaTime * 5.0f);
            }
        }
    }

    public void Shake(float duration = 0.5f, float intensity = 50.0f)
    {
        m_shakeTime = duration;
        m_shakeIntensity = intensity;
    }
}
