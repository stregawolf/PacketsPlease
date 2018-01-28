using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TitleBarUI : MonoBehaviour {
    public TextMeshProUGUI m_date;
    private DateTime m_datetime;

    private void Start() {
        if (m_datetime == null) {
            m_datetime = new DateTime(2018, 1, 24, 9, 0, 0);
        }
    }

    private void Update() {
        m_datetime = m_datetime.AddSeconds(Time.deltaTime*96); //5 min irl = 8 hr in game
        m_date.text = string.Format("{0:ddd MMM dd hh:mm}", m_datetime);
    }

    public void SetTime(DateTime datetime) {
        m_datetime = datetime;
    }


}
