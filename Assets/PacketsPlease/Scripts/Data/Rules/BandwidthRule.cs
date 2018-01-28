using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BandwidthRule", menuName = "Rules/Bandwidth", order = 0)]
public class BandwidthRule : RuleData
{
    public BandwidthRule() { }

    public BandwidthRule(float usageLimit, ActionType correctResponse, int priority = 0) : base(priority)
    {
        
        m_usageLimit = usageLimit;
        m_priority = priority;
        m_action = correctResponse;
    }

    protected override void Init()
    {
        base.Init();
        m_type = Type.Bandwidth;
    }

    public override ActionType ActionRequired(CustomerData customer)
    {
        bool dataRequirement = (m_usageLimit <= 0 || customer.m_dataUsage > m_usageLimit);
        if (customer.m_dataUsage > m_usageLimit && dataRequirement)
        {
            return m_action;
        }
        return ActionType.None;
    }
}
