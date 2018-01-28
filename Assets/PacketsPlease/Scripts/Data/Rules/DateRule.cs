using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DateRule", menuName = "Rules/Date", order = 0)]
public class DateRule : RuleData
{
    int m_daysActive;
    public DateRule() { }

    public DateRule(int daysActive, ActionType correctResponse, float usageLimit, int priority = 0) : base(priority)
    {
        m_daysActive = daysActive;
        m_priority = priority;
        m_action = correctResponse;
        m_usageLimit = usageLimit;
    }

    protected override void Init()
    {
        base.Init();
        m_type = Type.Date;
    }

    public override ActionType ActionRequired(CustomerData customer)
    {
        bool dataRequirement = (m_usageLimit <= 0 || customer.m_dataUsage > m_usageLimit);
        if(customer.m_daysActive < m_daysActive && dataRequirement)
        {
            return m_action;
        }
        return ActionType.None;
    }
}