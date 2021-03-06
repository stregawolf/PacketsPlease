﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ActionData {

    [System.Serializable]
    public enum ActionType
    {
        None,
        Throttle,
        Boost,
        Disconnect
    }

    public CustomerData customer;
    public ActionType actionType;

    // Passed/Violated rules lists, in descending order of priority
    // TODO: Make this SortedList
    public List<RuleData> passedRules;
    public List<RuleData> violatedRules;

    public RuleData HighestViolatedRule { get { return (violatedRules.Count == 0) ? null : violatedRules[0]; } }
    public RuleData HighestPassedRule { get { return (passedRules.Count == 0) ? null : passedRules[0]; } }

    public ActionData(CustomerData customer, ActionType ActionType)
    {
        this.customer = customer;
        this.actionType = ActionType;

        passedRules = new List<RuleData>();
        violatedRules = new List<RuleData>();
    }

    public string AllRulesToStr()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("PASSED RULES\n");
        for(int i = 0; i < passedRules.Count; i++)
        {
            sb.Append(passedRules[i].ToString());
        }
        sb.Append("VIOLATED RULES\n");
        for(int i = 0; i < violatedRules.Count; i++)
        {
            sb.Append(violatedRules[i].ToString());
        }

        return sb.ToString();
    }
}
