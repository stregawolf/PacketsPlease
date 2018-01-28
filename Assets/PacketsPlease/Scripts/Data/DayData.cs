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
}
