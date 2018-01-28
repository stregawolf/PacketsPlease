using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
    Throttle,
    Boost,
    Disconnect
}

public class ActionData : ScriptableObject {

    public CustomerData customer;
    public ActionType actionType;

    // Passed/Violated rules lists, in descending order of priority
    // TODO: Make this SortedList
    public List<RuleData> passedRules;
    public List<RuleData> violatedRules;

    public RuleData HighestViolatedRule { get { return (violatedRules.Count == 0) ? null : violatedRules[0]; } }
    public RuleData HighestPassedRule { get { return (passedRules.Count == 0) ? null : passedRules[0]; } }

    public ActionData(CustomerData customer, ActionType actionType)
    {
        this.customer = customer;
        this.actionType = actionType;

        passedRules = new List<RuleData>();
        violatedRules = new List<RuleData>();
    }

    public void PrintAllRules()
    {
        Debug.Log("PASSED RULES");
        for(int i = 0; i < a.passedRules.Count; i++)
        {
            Debug.Log(passedRules[i].ToString());
        }
        Debug.Log("VIOLATED RULES");
        for(int i = 0; i < a.violatedRules.Count; i++)
        {
            Debug.Log(violatedRules[i].ToString());
        }
    }
}
