using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleManager : Singleton<RuleManager> {

    private List<RuleData> m_rules;

    public void ClearAllRules()
    {
        throw new System.NotImplementedException();
    }

    public void AddRule(RuleData rule)
    {
        throw new System.NotImplementedException();
    }

    // Check action against all rules in place.
    // Return highest priority rule that's violated, if any.
    // Return null if no rules are violated.
    public RuleData DoesActionViolateRule(ActionData actionTaken)
    {
        throw new System.NotImplementedException();
    }

}
