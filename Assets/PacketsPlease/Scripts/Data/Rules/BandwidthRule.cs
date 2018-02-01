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

    private float GetBandwidthForTier(CustomerData.SpeedTier tier)
    {
        float multiplier = 1f;
        switch (tier)
        {
            case CustomerData.SpeedTier.Gold:
                multiplier = 4;
                break;
            case CustomerData.SpeedTier.Silver:
                multiplier = 2;
                break;
        }
        return m_usageLimit * multiplier;
    }

    public override ActionData.ActionType ActionRequired(CustomerData customer)
    {
        if (customer.m_dataUsage > GetBandwidthForTier(customer.m_speedTier))
        {
            return m_action;
        }
        return ActionData.ActionType.None;
    }

    public override void MakePass(CustomerData customer)
    {
        float limit = GetBandwidthForTier(customer.m_speedTier);
        if (customer.m_dataUsage > limit)
        {
            customer.m_dataUsage = Random.Range(Mathf.Min(m_usageLimit, 0.1f), limit);
        }
    }

    public override void MakeFail(CustomerData customer)
    {
        if (m_usageLimit > 0)
        {
            float limit = GetBandwidthForTier(customer.m_speedTier);
            customer.m_dataUsage += limit + Random.Range(0.1f, 10f);
        }
    }

    public override string TriggerReason(CustomerData customer)
    {
        return string.Format("Usage was <color=#FFAAAA>{0}</color>/{1} GB for <sprite={2}> {3} tier", 
            (int)customer.m_dataUsage, 
            GetBandwidthForTier(customer.m_speedTier), 
            (int)customer.m_speedTier,
            customer.m_speedTier);
    }
}
