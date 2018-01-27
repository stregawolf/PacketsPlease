﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationUI : MonoBehaviour {
    public TextMeshProUGUI m_title;
    public NotificationData m_data { get; protected set; }
    protected Button button;

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

    // tODO GET RID OF THIS
    void SendToMain()
    {
        GameObject main = GameObject.Find("[ Main ]");
        if(main != null)
        {
            PacketsPleaseMain mainComp = main.GetComponent<PacketsPleaseMain>();
            mainComp.UpdateNotification(this);
        }
    }
}
