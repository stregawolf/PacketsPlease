using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class RuleManager : Singleton<RuleManager> {

    public const ActionType DEFAULT_ACTION = ActionType.Boost;
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


    // Determine whether the rules have been violated by an action`
    // Also populates actionTaken with sorted list of passed and violated rules
    public bool DoesViolateRules(ActionData actionTaken)
    {
        var violatedRules = new List<RuleData>();
        var passedRules = new List<RuleData>();

        for(int i = 0; i < m_rules.Count; i++)
        {
            RuleData rule = m_rules[i];

            // Check that rule applies and is violated
            if(!rule.DoesApply(actionTaken))
            {
                continue;
            }

            if(rule.IsViolated(actionTaken))
            {
                violatedRules.Add(rule);
            }
            else
            {
                passedRules.Add(rule);
            }
        }

        violatedRules.OrderByDescending((rule) => rule.m_priority);
        passedRules.OrderByDescending((rule) => rule.m_priority);

        actionTaken.violatedRules = violatedRules;
        actionTaken.passedRules = passedRules;

        // Check if violated rules or passed rules take priority
        if(violatedRules.Count == 0)
        {
            return false;
        }
        else if(passedRules.Count == 0)
        {
            return true;
        }
        else
        {
            int vp = violatedRules[0].m_priority;
            int pp = passedRules[0].m_priority;

            if(vp == pp) 
            {
                DumpRulesAndCrash(actionTaken, "RuleManager: Violated and Passed rules have equal priority");
                return false;
            }
            else
            {
                return (violatedRules[0].m_priority > passedRules[0].m_priority);
            }
        } 
    }

    private void DumpRulesAndCrash(ActionData a, string msg)
    {
        Debug.Log(a.AllRulesToStr());
        throw new System.Exception(msg);
    }
}
