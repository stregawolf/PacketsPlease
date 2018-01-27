using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsageLowerRule : RuleData {

    public UsageLowerRule(float usage, ActionType correctAction, int priority = RuleData.LOWEST_PRIORITY)
    : base(correctAction, priority)
    {
        AddBandwidthLowerConstraint(usage);
    }

}
