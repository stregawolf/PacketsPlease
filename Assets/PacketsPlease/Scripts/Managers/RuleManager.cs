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
        ActionType requiredAction = GetRequiredAction(actionTaken.customer);

        return requiredAction != actionTaken.actionType;
    }

    public ActionType GetRequiredAction(CustomerData customer)
    {
        ActionType requiredAction = ActionType.None;
        int currPriority = int.MinValue;
        foreach(RuleData rule in Rules)
        {
            ActionType thisReqAction = rule.ActionRequired(customer);
            if (thisReqAction != ActionType.None && rule.m_priority > currPriority)
            {
                requiredAction = thisReqAction;
                currPriority = rule.m_priority;
            }
        }
        return requiredAction;
    }

    public void BuildRuleNotifications()
    {
        NotificationData mainRules, promoted, restricted, prohibited;

        string mainBoost, mainThrottle, mainDisconnect, partners, promotions;
    }

    private void DumpRulesAndCrash(ActionData a, string msg)
    {
        Debug.Log(a.AllRulesToStr());
        throw new System.Exception(msg);
    }
}
