using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IndividualRule", menuName = "Rules/Individual", order = 0)]
public class IndividualRule : RuleData
{
    [SerializeField]
    string m_name;

    public IndividualRule(string name, ActionData.ActionType correctResponse, float usageLimit = 0, int priority = 0) : base(correctResponse, usageLimit, priority)
    {
        m_name = name;
    }

    protected override void Init()
    {
        base.Init();
        m_type = Type.Individual;
    }

    public override ActionData.ActionType ActionRequired(CustomerData customer)
    {
        if(customer.m_name.FirstLast.ToLower() == m_name.ToLower())
        {
            return m_action;
        }
        return ActionData.ActionType.None;
    }

    public override void MakePass(CustomerData customer)
    {
        if (customer.m_name.FirstLast.ToLower() == m_name.ToLower())
        {
            if (m_usageLimit > 0 && Random.value < 0.5f)
            {
                customer.m_dataUsage = Random.Range(Mathf.Min(m_usageLimit, 0.1f), m_usageLimit);
            }
            else
            {
                customer.m_name = NameGen.GetName();
            }
        }
    }

    public override void MakeFail(CustomerData customer)
    {
        customer.m_name.Set(m_name);
        base.MakeFail(customer);
    }
}
