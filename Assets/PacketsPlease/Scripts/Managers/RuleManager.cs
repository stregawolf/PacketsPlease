using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RuleManager : Singleton<RuleManager> {

    public List<RuleData> Rules { get { return m_rules; } }
    private List<RuleData> m_rules;

    protected override void Awake()
    {
        base.Awake();
        m_rules = new List<RuleData>();
    }

    public void ClearAllRules()
    {
        m_rules.Clear();
    }

    public void AddRule(RuleData rule)
    {
        m_rules.Add(rule);
    }

    // Check action against all rules in place.
    // Return highest priority rule that's violated, if any.
    // Return null if no rules are violated.
    public RuleData GetHighestViolatedRule(CustomerData customer, ActionType actionTaken)
    {
        List<RuleData> violatedRules = GetViolatedRules(customer, actionTaken);

        if(violatedRules == null)
        {
            return null;
        }

        return violatedRules[0];
    }

    public List<RuleData> GetViolatedRules(CustomerData customer, ActionType actionTaken)
    {
        var violatedRules = new List<RuleData>();

        for(int i = 0; i < m_rules.Count; i++)
        {
            RuleData rule = m_rules[i];
            if(rule.IsViolated(customer, actionTaken))
            {
                violatedRules.Add(rule);
            }
        }

        if(violatedRules.Count == 0)
        {
            return null;
        }

        violatedRules.OrderByDescending((rule) => rule.m_priority);

        return violatedRules;
    }
}
