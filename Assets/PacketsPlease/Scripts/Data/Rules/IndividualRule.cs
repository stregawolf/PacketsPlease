using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IndividualRule", menuName = "Rules/Individual", order = 0)]
public class IndividualRule : RuleData
{
    public string m_name;
    public IndividualRule() { }

    public IndividualRule(string name, ActionType correctResponse, float usageLimit = 0, int priority = 0) : base(priority)
    {
        m_name = name;
        m_priority = priority;
        m_action = correctResponse;
    }

    protected override void Init()
    {
        base.Init();
        m_type = Type.Individual;
    }

    public override ActionType ActionRequired(CustomerData customer)
    {
        if(customer.m_name.FirstLast.ToLower() == m_name.ToLower())
        {
            return m_action;
        }
        return ActionType.None;
    }
}
