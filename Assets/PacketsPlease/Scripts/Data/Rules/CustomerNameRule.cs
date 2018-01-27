using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerNameRule : RuleData {

    public CustomerNameRule(string name, ActionType correctAction, int priority = RuleData.LOWEST_PRIORITY)
    : base(correctAction, priority)
    {
        AddCustomerNameConstraint(name);
    }

}
