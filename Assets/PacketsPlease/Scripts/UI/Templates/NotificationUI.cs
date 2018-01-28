using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationUI : MonoBehaviour {
    public Image m_bg;
    public TextMeshProUGUI m_title;
    public NotificationData m_data { get; protected set; }
    protected Button button;

    protected Color m_originalTitleColor;
    protected Color m_originalBGColor;

    protected void Start()
    {
        m_originalTitleColor = m_title.color;
        m_originalBGColor = m_bg.color;
    }

    public virtual void Init(NotificationData data)
    {
        m_data = data;
        Init(data.m_title);
    }

    public void Init(string title)
    {
        m_title.text = title;
        button = GetComponent<Button>();
        if(button != null)
        {
            // TODO: Hook button to event system but for now, quick-and-dirty
            button.onClick.AddListener(SendToMain);
        }
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

    protected void SendToMain()
    {
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
