using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TitleBarUI : MonoBehaviour {
    public TextMeshProUGUI m_date;
    public static DateTime GameDate { get; private set; }
    private static int START_HOUR = 9;
    private static int END_HOUR = 5;
    public static int MinutesSinceDayStart { get; private set; }

    private void Start()
    {
        if (GameDate == null)
        {
            GameDate = new DateTime(2018, 1, 24, START_HOUR, 0, 0);
        }
    }

    public void SetDay(int day)
    {
        GameDate = new DateTime(2018, 1, 24, START_HOUR, 0, 0);
        MinutesSinceDayStart = (GameDate.Hour - START_HOUR) * 60 + GameDate.Minute;
    }

    private void Update()
    {
        GameDate = GameDate.AddSeconds(Time.deltaTime*96); //5 min irl = 8 hr in game
        m_date.text = string.Format("{0:ddd MMM dd hh:mm}", GameDate);
        MinutesSinceDayStart = (GameDate.Hour - START_HOUR) * 60 + GameDate.Minute;
    }

    public void SetTime(DateTime dateTime)
    {
        GameDate = dateTime;
    }
}
