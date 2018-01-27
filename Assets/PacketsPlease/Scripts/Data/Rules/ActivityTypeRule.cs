using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityTypeRule : RuleData {

    public ActivityTypeRule(ActivityData.Activity.Type type, ActionType correctAction, int priority = RuleData.LOWEST_PRIORITY)
    : base(correctAction, priority)
    {
        AddActivityTypeConstraint(type);
    }

}
