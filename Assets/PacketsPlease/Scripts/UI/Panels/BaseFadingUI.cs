using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BaseFadingUI : MonoBehaviour
{
    public Image m_bg;

    public void FadeIn(bool instant = false, float delay = 0.0f, Action onComplete = null)
    {
        gameObject.SetActive(true);
        if (instant)
        {
            SetAlpha(1.0f);
            if (onComplete != null)
            {
                onComplete.Invoke();
            }
        }
        else
        {
            LeanTween.value(gameObject, m_bg.color.a, 1.0f, 1.0f).setDelay(delay).setOnUpdate(SetAlpha).setOnComplete(()=>
            {
                if (onComplete != null)
                {
                    onComplete.Invoke();
                }
            });
        }
    }

    public void FadeOut(bool instant = false, float delay = 0.0f, Action onComplete = null)
    {
        if (instant)
        {
            SetAlpha(0.0f);
            if(onComplete != null)
            {
                onComplete.Invoke();
            }
        }
        else
        {
            LeanTween.value(gameObject, m_bg.color.a, 0.0f, 1.0f).setDelay(delay).setOnUpdate(SetAlpha).setOnComplete(()=> 
            {
                OnFadeOutComplete();
                if (onComplete != null)
                {
                    onComplete.Invoke();
                }
            });
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
