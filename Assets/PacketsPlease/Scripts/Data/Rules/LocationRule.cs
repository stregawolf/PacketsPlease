using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationRule", menuName = "Rules/Location", order = 0)]
public class LocationRule : RuleData
{
    public CustomerData.Location m_location;

    public LocationRule(CustomerData.Location location, ActionData.ActionType correctResponse, float usageLimit = 0, int priority = 0) : base(correctResponse, usageLimit, priority)
    {
        m_location = location;
    }

    public override void Init()
    {
        base.Init();
        m_type = Type.Location;
    }

    public override ActionData.ActionType ActionRequired(CustomerData customer)
    {
        if(customer.m_location == m_location)
        {
            return m_action;
        }
        return ActionData.ActionType.None;
    }

    public override void MakePass(CustomerData customer)
    {
        if (customer.m_location == m_location)
        {
            if (m_usageLimit > 0 && Random.value < 0.5f)
            {
                customer.m_dataUsage = Random.Range(Mathf.Min(m_usageLimit, 0.1f), m_usageLimit);
            }
            else
            {
                customer.m_location = (CustomerData.Location)((int)customer.m_location + 1 == (int)CustomerData.Location.NUM_LOCATIONS || Random.value > 0.5f ? Random.Range(0, (int)customer.m_location) : Random.Range((int)customer.m_location + 1, (int)CustomerData.Location.NUM_LOCATIONS));
            }
        }
    }

    public override void MakeFail(CustomerData customer)
    {
        customer.m_location = m_location;
        base.MakeFail(customer);
    }
}
