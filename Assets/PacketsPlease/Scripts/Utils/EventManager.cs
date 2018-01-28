using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EventManager {
    public static readonly CallBack<NotificationUI> OnNotificationSelected = new CallBack<NotificationUI>();
    public static readonly CallBack<NotificationUI, bool> OnNotificationResolved = new CallBack<NotificationUI, bool>();
    public static readonly CallBack<int> OnStrike = new CallBack<int>();
    public static readonly CallBack OnEndOfDay = new CallBack();
}

public class CallBack
{
    protected Action m_callbacks;
    public void Register(Action callback)
    {
        m_callbacks += callback;
    }

    public void Unregister(Action callback)
    {
        m_callbacks -= callback;
    }

    public void Dispatch()
    {
        if (m_callbacks != null)
        {
            m_callbacks.Invoke();
        }
    }
}

public class CallBack<T>
{
    protected Action<T> m_callbacks;
    public void Register(Action<T> callback)
    {
        m_callbacks += callback;
    }

    public void Unregister(Action<T> callback)
    {
        m_callbacks -= callback;
    }

    public void Dispatch(T t)
    {
        if(m_callbacks != null)
        {
            m_callbacks.Invoke(t);
        }
    }
}

public class CallBack<T1, T2>
{
    protected Action<T1, T2> m_callbacks;
    public void Register(Action<T1, T2> callback)
    {
        m_callbacks += callback;
    }

    public void Unregister(Action<T1, T2> callback)
    {
        m_callbacks -= callback;
    }

    public void Dispatch(T1 t1, T2 t2)
    {
        if (m_callbacks != null)
        {
            m_callbacks.Invoke(t1, t2);
        }
    }
}