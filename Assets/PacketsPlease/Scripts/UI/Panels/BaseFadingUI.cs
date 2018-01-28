using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseFadingUI : MonoBehaviour
{
    public Image m_bg;

    public void FadeIn(bool instant = false)
    {
        gameObject.SetActive(true);
        if (instant)
        {
            SetAlpha(1.0f);
        }
        else
        {
            LeanTween.value(gameObject, m_bg.color.a, 1.0f, 1.0f).setOnUpdate(SetAlpha);
        }
    }

    public void FadeOut(bool instant = false)
    {
        if (instant)
        {
            SetAlpha(0.0f);
        }
        else
        {
            LeanTween.value(gameObject, m_bg.color.a, 0.0f, 1.0f).setOnUpdate(SetAlpha).setOnComplete(OnFadeOutComplete);
        }
    }

    public virtual void SetAlpha(float alpha)
    {
        Color c = m_bg.color;
        c.a = alpha;
        m_bg.color = c;
    }

    protected virtual void OnFadeOutComplete()
    {
        gameObject.SetActive(false);
    }
}
