using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayDisplayUI : BaseFadingUI {
    public TextMeshProUGUI m_day;

    public void SetDay(int day)
    {
        m_day.text = string.Format("Day {0}", day);
    }
   
    public override void SetAlpha(float alpha)
    {
        base.SetAlpha(alpha);
        m_day.alpha = alpha;
    }
}
