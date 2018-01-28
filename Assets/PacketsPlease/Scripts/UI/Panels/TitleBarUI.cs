using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TitleBarUI : MonoBehaviour {
    public TextMeshProUGUI m_date;
    private DateTime m_dateTime;
    private static int START_HOUR = 9;
    private static int END_HOUR = 5;

    // TODO: If we ever decide to make this real, don't do this, but fuck it
    public int MinutesSinceDayStart
    {
        get { return (m_dateTime.Hour - START_HOUR) * 60 + m_dateTime.Minute; }
    }

    private void Start() {
        if (m_dateTime == null) {
            m_dateTime = new DateTime(2018, 1, 24, START_HOUR, 0, 0);
        }
    }

    private void Update() {
        m_dateTime = m_dateTime.AddSeconds(Time.deltaTime*96); //5 min irl = 8 hr in game
        m_date.text = string.Format("{0:ddd MMM dd hh:mm}", m_dateTime);
    }

    public void SetTime(DateTime dateTime) {
        this.m_dateTime = dateTime;
    }
}
