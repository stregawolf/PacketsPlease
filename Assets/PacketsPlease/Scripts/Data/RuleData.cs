using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RuleData : ScriptableObject {

    // TODO: MAKE GETTERS FOR THESE
    public int m_priority;
    public const int HIGHEST_PRIORITY = 999;
    public const int LOWEST_PRIORITY = 0;
    public bool m_skipAutoNotice = false;

    public RuleData(ActionData.ActionType correctResponse, float usageLimit = 0f, int priority = LOWEST_PRIORITY)
    {
        m_usageLimit = usageLimit;
        m_priority = priority;
        m_action = correctResponse;
        Init();
    }

    public virtual void Init()
    {

    }

    public enum Type
    {
        Activity,
        ActivityType,
        Bandwidth,
        Date,
        Individual,
        Location
    }

    public Type m_type { get; protected set; }

    public ActionData.ActionType m_action;

    public float m_usageLimit = 0f;

    // Check if action taken violates any constraints of the rule
    public virtual ActionData.ActionType ActionRequired(CustomerData customer)
    {
        return ActionData.ActionType.None;
    }

    public virtual void MakePass(CustomerData customer)
    {

    }

    public virtual void MakeFail(CustomerData customer)
    {
        if (m_usageLimit > 0)
        {
            customer.m_dataUsage += m_usageLimit + Random.Range(0.1f, 10f);
        }
    }
   
    public override string ToString()
    {
        return string.Format("Base Rule: How Are You Using This?");
    }
}
