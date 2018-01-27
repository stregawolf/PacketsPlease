using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    RuleData is initialized by using the minimal constructor and chaining AddXXXConstraint() functions:

    example:
        new RuleData(0, ActivityType.Throttle).AddActivityConstraint(ActivityType.Streaming)

    will create a rule to throttle all streaming activities

 */
public class RuleData : ScriptableObject {

    public int m_priority; // Higher priority number is more important
    public float m_bandwidth; // Bandwidth of 0 or less = ignore
    private ActivityData.Activity.Type m_activityType;
    private CustomerData m_customer;
    private ActionType m_correctActionType;

    public static readonly int HIGHEST_PRIORITY = 999;
    public static readonly int LOWEST_PRIORITY = 0;

    private enum ConstraintType
    {
        Bandwidth,
        Activity,
        Customer
    }

    private HashSet<ConstraintType> m_constraints;

    // Minimal constructor; constraints can be applied by chaining the Add***Constraint() functions
    // As is, it would say "apply this action to everyone"
    public RuleData(int priority, ActionType correctActionType)
    {
        m_priority = priority;
        m_correctActionType = correctActionType;
    }

    // Check if action taken violates any constraints of the rule
    public bool IsViolatedBy(ActionData actionTaken)
    {
        bool ruleApplies = true;
        foreach(ConstraintType constraint in m_constraints)
        {
            // Compare constraints to customer data
            switch(constraint)
            {
                case ConstraintType.Activity:
                    if(actionTaken.customer.m_activity.m_type != m_activityType)
                    ruleApplies = false;
                    break;
                case ConstraintType.Bandwidth:
                    if(actionTaken.customer.m_dataUsage < m_bandwidth)
                    ruleApplies = false;
                    break;
                case ConstraintType.Customer:
                    if(actionTaken.customer != m_customer)
                    ruleApplies = false;
                    break;
                default:
                    throw new System.Exception("RuleData: Missing constraint types in IsViolatedBy()");
            }
        }

        // Check for violation
        if(ruleApplies)
        {
            if(actionTaken.actionType != m_correctActionType)
            {
                return true;
            }
        }

        return false;
    }

    // Helper functions for Constraint set
    public RuleData AddBandwidthConstraint(float bandwidth) { this.m_bandwidth = bandwidth; AddConstraint(ConstraintType.Bandwidth); return this; }
    public RuleData AddActivityConstraint(ActivityData.Activity.Type activity) { this.m_activityType = activity; AddConstraint(ConstraintType.Activity); return this; }
    public RuleData AddCustomerConstraint(CustomerData customer) { this.m_customer = customer; AddConstraint(ConstraintType.Customer); return this; }

    private void AddConstraint(ConstraintType constraint) { m_constraints.Add(constraint); }

}
