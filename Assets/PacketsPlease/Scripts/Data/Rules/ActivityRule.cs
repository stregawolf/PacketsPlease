using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActivityRule", menuName = "Rules/Activity", order = 0)]
public class ActivityRule : RuleData
{
    string m_activityName;
    public ActivityRule() { }

    public ActivityRule(string name, ActionType correctResponse, float usageLimit, int priority = 0) : base(priority)
    {
        m_activityName = name;
        m_priority = priority;
        m_action = correctResponse;
        m_usageLimit = usageLimit;
    }

    protected override void Init()
    {
        base.Init();
        m_type = Type.Activity;
    }

    public override ActionType ActionRequired(CustomerData customer)
    {
        bool dataRequirement = (m_usageLimit <= 0 || customer.m_dataUsage > m_usageLimit);
        if (customer.m_activity.m_name.ToLower() == m_activityName.ToLower() && dataRequirement)
        {
            return m_action;
        }
        return ActionType.None;
    }
}