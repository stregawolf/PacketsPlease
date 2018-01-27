using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityNameRule : RuleData {

    public ActivityNameRule(string name, ActionType correctAction, int priority = RuleData.LOWEST_PRIORITY)
    : base(correctAction, priority)
    {
        AddActivityNameConstraint(name);
    }

}
