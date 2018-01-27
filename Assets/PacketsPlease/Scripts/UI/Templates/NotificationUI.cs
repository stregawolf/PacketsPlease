using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationUI : MonoBehaviour {
    public TextMeshProUGUI m_title;

    public NotificationData m_data { get; protected set; }

    public void Init(NotificationData data)
    {
        m_data = data;
        Init(data.m_title);
    }

    public void Init(string title)
    {
        m_title.text = title;
    }
}
