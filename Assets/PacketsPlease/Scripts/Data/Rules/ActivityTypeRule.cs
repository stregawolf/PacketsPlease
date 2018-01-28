using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Activity Type", menuName = "Rules/Activity Type", order = 0)]
public class ActivityTypeRule : RuleData {

    public ActivityData.Activity.Type m_activityType;
    public ActivityTypeRule() { }

    public ActivityTypeRule(ActivityData.Activity.Type type, ActionType correctResponse, float usageLimit = 0, int priority = 0) : base(priority)
    {
        m_activityType = type;
        m_priority = priority;
        m_action = correctResponse;
    }

    protected override void Init()
    {
        base.Init();
        m_type = Type.ActivityType;
    }

    public override ActionType ActionRequired(CustomerData customer)
    {
        bool dataRequirement = (m_usageLimit <= 0 || customer.m_dataUsage > m_usageLimit);
        if (customer.m_activity.m_type == m_activityType && dataRequirement)
        {
            return m_action;
        }
        return ActionType.None;
    }
}
