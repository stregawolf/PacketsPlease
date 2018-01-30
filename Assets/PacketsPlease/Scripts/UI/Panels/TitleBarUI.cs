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
        // number of minutes between 9 am to 5pm divided by real minutes per game day
        m_timeScaler = 480.0f / m_realMinutesPerGameDay;
    }

    public void SetDay(int day)
    {
        GameDate = new DateTime(2018, 1, 27, START_HOUR, 0, 0).AddDays(day);
        MinutesSinceDayStart = (GameDate.Hour - START_HOUR) * 60 + GameDate.Minute;
        m_date.text = string.Format("{0:M/dd/yyyy}", GameDate);
    }

    public void UpdateTime()
    {
        if (PacketsPleaseMain.Instance.IntroTime)
        {
            GameDate = GameDate.AddSeconds(Time.deltaTime * 30);
        }
        else
        { 
            GameDate = GameDate.AddSeconds(Time.deltaTime * m_timeScaler);
        }
        m_date.text = string.Format("{0:M/dd/yyyy}", GameDate);
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
