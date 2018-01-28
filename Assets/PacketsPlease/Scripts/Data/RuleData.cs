using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RuleData : ScriptableObject {

    // TODO: MAKE GETTERS FOR THESE
    public int m_priority;
    public const int HIGHEST_PRIORITY = 999;
    public const int LOWEST_PRIORITY = 0;

    public RuleData(int priority = LOWEST_PRIORITY)
    {
        m_priority = priority;
    }

    protected virtual void Init()
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
    
    public ActionType m_action { get; protected set; }

    public float m_usageLimit = 0f;

    // Check if action taken violates any constraints of the rule
    public virtual ActionType ActionRequired(CustomerData customer)
    {
        return ActionType.None;
    }
   
    public override string ToString()
    {
        return string.Format("Base Rule: How Are You Using This?");
    }
}
