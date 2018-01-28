using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayDisplayUI : MonoBehaviour {
    public Image m_bg;
    public TextMeshProUGUI m_day;

    public void SetDay(int day)
    {
        m_day.text = string.Format("Day {0}", day);
    }

    public void FadeIn(bool instant = false)
    {
        gameObject.SetActive(true);
        if (instant)
        {
            SetAlpha(1.0f);
        }
        else
        {
            LeanTween.value(gameObject, m_day.alpha, 1.0f, 1.0f).setOnUpdate(SetAlpha);
        }
    }

    public void FadeOut(bool instant = false)
    {
        if(instant)
        {
            SetAlpha(0.0f);
        }
        else
        {
            LeanTween.value(gameObject, m_day.alpha, 0.0f, 1.0f).setOnUpdate(SetAlpha).setOnComplete(OnFadeOutComplete);
        }
    }

    public void SetAlpha(float alpha)
    {
        Color c = m_bg.color;
        c.a = alpha;
        m_bg.color = c;

        m_day.alpha = alpha;
    }

    protected void OnFadeOutComplete()
    {
        gameObject.SetActive(false);
    }
}
