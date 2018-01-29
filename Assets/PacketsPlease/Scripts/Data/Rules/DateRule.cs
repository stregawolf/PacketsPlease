using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DateRule", menuName = "Rules/Date", order = 0)]
public class DateRule : RuleData
{
    public int m_daysActive;

    public DateRule(int daysActive, ActionData.ActionType correctResponse, float usageLimit, int priority = 0) : base(correctResponse, usageLimit, priority)
    {
        m_daysActive = daysActive;
        m_priority = priority;
        m_action = correctResponse;
        m_usageLimit = usageLimit;
    }

    public override void Init()
    {
        base.Init();
        m_type = Type.Date;
    }

    public override ActionData.ActionType ActionRequired(CustomerData customer)
    {
        bool dataRequirement = (m_usageLimit <= 0 || customer.m_dataUsage > m_usageLimit);
        if(customer.m_daysActive < m_daysActive && dataRequirement)
        {
            return m_action;
        }
        return ActionData.ActionType.None;
    }

    public override void MakePass(CustomerData customer)
    {
        if(customer.m_daysActive < m_daysActive)
        {
            if (m_usageLimit > 0 && Random.value < 0.5f)
            {
                customer.m_dataUsage = Random.Range(Mathf.Min(m_usageLimit, 0.1f), m_usageLimit);
            }
            else
            {
                customer.m_daysActive += m_daysActive;
            }
        }
    }

    public override void MakeFail(CustomerData customer)
    {
        customer.m_daysActive = Random.Range(0, m_daysActive);
        base.MakeFail(customer);
    }

    public override string TriggerReason(CustomerData customer)
    {
        string response = string.Format("Days active was <color=#FFAAAA>{0}</color>", customer.m_daysActive);
        if (m_usageLimit > 0)
        {
            response += string.Format(", Usage <color=#FFAAAA>{0} GBs </color> ", customer.m_dataUsage);
        }
        return response;
    }
}