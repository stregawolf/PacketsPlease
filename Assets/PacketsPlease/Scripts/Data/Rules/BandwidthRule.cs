using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BandwidthRule", menuName = "Rules/Bandwidth", order = 0)]
public class BandwidthRule : RuleData
{

    public BandwidthRule(float usageLimit, ActionData.ActionType correctResponse, int priority = 0) : base(correctResponse, usageLimit, priority)
    {
        
        m_action = correctResponse;
    }

    public override void Init()
    {
        base.Init();
        m_type = Type.Bandwidth;
    }

    public override ActionData.ActionType ActionRequired(CustomerData customer)
    {
        if (customer.m_dataUsage > m_usageLimit)
        {
            return m_action;
        }
        return ActionData.ActionType.None;
    }

    public override void MakePass(CustomerData customer)
    {
        if (customer.m_dataUsage > m_usageLimit)
        {
            customer.m_dataUsage = Random.Range(Mathf.Min(m_usageLimit, 0.1f), m_usageLimit);
        }
    }

    public override void MakeFail(CustomerData customer)
    {
        base.MakeFail(customer);
    }
}
