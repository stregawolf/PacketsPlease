using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DayData", menuName = "Data/DayData", order = 2)]

public class DayData : ScriptableObject {
    public List<RuleData> rules;
    public string m_dailyMessage;
    public struct PriorityRule
    {
        public RuleData rule;
        public NotificationData m_notification;
    }

    public List<PriorityRule> m_priortyRules;

    private NotificationData m_dailyNotification;

    public List<NotificationData> GetDailyNotifications()
    {
        List<NotificationData> dailyNotifications = new List<NotificationData>();
        if (m_dailyNotification == null)
        {
            NotificationData m_dailyNotification = ScriptableObject.CreateInstance<NotificationData>();
            m_dailyNotification.m_pinned = true;
            m_dailyNotification.m_sender = "policy@cosmocast.com";
            m_dailyNotification.m_title = string.Format("Company Policy Update {0}", TitleBarUI.GameDate.ToShortDateString());
            m_dailyNotification.m_message = m_dailyMessage;
            m_dailyNotification.m_message += "\n<b><u>Rules</u></b>";
            foreach (RuleData rule in rules)
            {
                m_dailyNotification.m_message += string.Format("\n•{0}", rule.ToString());
            }
        }
        dailyNotifications.Add(m_dailyNotification);
        foreach (PriorityRule pr in m_priortyRules)
        {
            dailyNotifications.Add(pr.m_notification);
        }
        return dailyNotifications;
    }
}
