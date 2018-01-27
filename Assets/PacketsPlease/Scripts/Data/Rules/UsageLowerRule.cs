using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsageHigherRule : RuleData {

    public UsageHigherRule(float usage, ActionType correctAction, int priority = RuleData.LOWEST_PRIORITY)
    : base(correctAction, priority)
    {
        AddBandwidthHigherConstraint(usage);
    }

}
