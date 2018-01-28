using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActivityRule", menuName = "Rules/Activity", order = 0)]
public class ActivityRule : RuleData
{
    public string m_activityName;

    public ActivityRule(string name, ActionData.ActionType correctResponse, float usageLimit, int priority = 0) : base(correctResponse, usageLimit, priority)
    {
        m_activityName = name;
        m_priority = priority;
        m_action = correctResponse;
        m_usageLimit = usageLimit;
    }

    public override void Init()
    {
        base.Init();
        m_type = Type.Activity;
    }

    public override ActionData.ActionType ActionRequired(CustomerData customer)
    {
        bool dataRequirement = (m_usageLimit <= 0 || customer.m_dataUsage > m_usageLimit);
        if (customer.m_activity.m_name.ToLower() == m_activityName.ToLower() && dataRequirement)
        {
            return m_action;
        }
        return ActionData.ActionType.None;
    }

    public override void MakePass(CustomerData customer)
    {
        if(customer.m_activity.m_name == m_activityName)
        {
            if (m_usageLimit > 0 && Random.value < 0.5f)
            {
                customer.m_dataUsage = Random.Range(Mathf.Min(m_usageLimit, 0.1f), m_usageLimit);
            }
            else
            {
                customer.m_activity = ActivityData.GetActivity(customer.m_activity);
            }
        }
    }

    public override void MakeFail(CustomerData customer)
    {
        customer.m_activity = ActivityData.GetActivityByName(m_activityName);
        base.MakeFail(customer);
    }
}