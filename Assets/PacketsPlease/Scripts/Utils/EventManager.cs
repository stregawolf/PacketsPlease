using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EventManager {
    public static readonly CallBack<NotificationUI> OnNotificationSelected = new CallBack<NotificationUI>();
    public static readonly CallBack<NotificationUI> OnNotificationResolved = new CallBack<NotificationUI>();
}

public class CallBack<T>
{
    protected Action<T> m_callbacks;
    public void Register(Action<T> callback)
    {
        m_callbacks += callback;
    }

    public void Dispatch(T data)
    {
        if(m_callbacks != null)
        {
            m_callbacks.Invoke(data);
        }
    }
}