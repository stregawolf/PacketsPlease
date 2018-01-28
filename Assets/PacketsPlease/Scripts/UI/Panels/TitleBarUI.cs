using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TitleBarUI : MonoBehaviour {
    public TextMeshProUGUI m_date;
    public float m_realMinutesPerGameDay = 5.0f;
    public static DateTime GameDate { get; private set; }
    private static int START_HOUR = 9;
    private static int END_HOUR = 17;
    public static int MinutesSinceDayStart { get; private set; }

    protected float m_timeScaler;

    private void Start()
    {
        m_timeScaler = 180.0f / m_realMinutesPerGameDay;
    }

    public void SetDay(int day)
    {
        GameDate = new DateTime(2018, 1, 27 + day, START_HOUR, 0, 0);
        MinutesSinceDayStart = (GameDate.Hour - START_HOUR) * 60 + GameDate.Minute;
    }

    public void UpdateTime()
    {
        GameDate = GameDate.AddSeconds(Time.deltaTime* m_timeScaler);
        m_date.text = string.Format("{0:ddd MMM dd hh:mm tt}", GameDate);
        MinutesSinceDayStart = (GameDate.Hour - START_HOUR) * 60 + GameDate.Minute;
        if(GameDate.Hour >= END_HOUR)
        {
            EventManager.OnEndOfDay.Dispatch();
        }
    }

    public void SetTime(DateTime dateTime)
    {
        GameDate = dateTime;
    }
}
