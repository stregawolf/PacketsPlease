using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationRule", menuName = "Rules/Location", order = 0)]
public class LocationRule : RuleData
{
    public CustomerData.Location m_location;
    public LocationRule() { }

    public LocationRule(CustomerData.Location location, ActionType correctResponse, float usageLimit = 0, int priority = 0) : base(priority)
    {
        m_location = location;
        m_priority = priority;
        m_action = correctResponse;
    }

    protected override void Init()
    {
        base.Init();
        m_type = Type.Location;
    }

    public override ActionType ActionRequired(CustomerData customer)
    {
        if(customer.m_location == m_location)
        {
            return m_action;
        }
        return ActionType.None;
    }
}
