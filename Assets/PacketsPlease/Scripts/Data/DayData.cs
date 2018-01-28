using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DayData", menuName = "Data/DayData", order = 2)]

public class DayData : ScriptableObject {
    public List<RuleData> rules;
    [System.Serializable]
    public struct PriorityRule
    {
        public RuleData rule;
        public NotificationData m_notification;
    }

    public List<PriorityRule> m_priortyRules;

    public void ApplyRules()
    {
        foreach(RuleData rule in rules)
        {
            rule.Init();
            RuleManager.Instance.AddRule(rule);
        }
    }
}
