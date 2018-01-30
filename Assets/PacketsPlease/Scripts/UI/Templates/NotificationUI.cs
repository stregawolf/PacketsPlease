using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationUI : MonoBehaviour {
    public Image m_bg;
    public Image m_icon;
    public TextMeshProUGUI m_title;
    public NotificationData m_data { get; protected set; }
    protected Button button;

    protected Color m_originalTitleColor;
    protected Color m_originalBGColor;

    protected void Awake()
    {
        m_originalTitleColor = m_title.color;
        m_originalBGColor = m_bg.color;
    }

    public virtual void Init(NotificationData data)
    {
        m_data = data;
        Init(m_data.m_title, m_data.m_iconColor);
        if(m_data.m_pinned)
        {
            SetIconColor(Color.cyan);
        }
    }

    public void Init(string title, Color iconColor)
    {
        m_title.text = title;
        SetIconColor(iconColor);
    }

    public void ResetColors()
    {
        SetTitleColor(m_originalTitleColor);
        SetBGColor(m_originalBGColor);
    }

    public void SetTitleColor(Color c)
    {
        m_title.color = c;
    }

    public void SetBGColor(Color c)
    {
        m_bg.color = c;
    }


    public void SetIconColor(Color c)
    {
        if(m_icon != null)
        {
            m_icon.color = c;
        }
    }

    public void SelectSelf()
    {
        StartCoroutine(SelectSelfDelay());
    }

    public void Select()
    {
        EventManager.OnNotificationSelected.Dispatch(this);
    }

    private IEnumerator SelectSelfDelay()
    {
        EventManager.OnNotificationSelected.Dispatch(null);
        yield return new WaitForSeconds(0.5f);
        EventManager.OnNotificationSelected.Dispatch(this);
    }

    public void DestroySelf()
    {
        StartCoroutine(HandleDestroySelf());
    }

    protected IEnumerator HandleDestroySelf()
    {
        LeanTween.moveLocalX(gameObject, Screen.width * 0.25f, 0.33f).setEaseInBack();
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
