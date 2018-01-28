using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerDetailsUI : CustomerUI {
    public TextMeshProUGUI m_customerStartDate;
    public Image m_activityImg;
    public TextMeshProUGUI m_activityName;
    public Image m_locationImg;
    public TextMeshProUGUI m_locationName;

    public Sprite[] m_activitySprites;

    public override void Init(CustomerData data)
    {
        base.Init(data);
        m_dataUsage.text = string.Format("{0:N2} GB used", data.m_dataUsage);

        System.DateTime dateStarted = System.DateTime.Now.AddDays(-1 * data.m_daysActive);
        m_customerStartDate.text = string.Format("Custer Since:\n{0}/{1}/{2}", dateStarted.Month, dateStarted.Day, dateStarted.Year);

        m_activityImg.sprite = m_activitySprites[(int)data.m_activity.m_type];
        m_activityName.text = data.m_activity.m_name;
    }

}
