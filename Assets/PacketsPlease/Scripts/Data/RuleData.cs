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
    public float m_daysActive;
    private ActivityData.Activity.Type m_activityType;
    private CustomerData m_customer;
    private ActionType m_correctActionType;

    public static readonly int HIGHEST_PRIORITY = 999;
    public static readonly int LOWEST_PRIORITY = 0;

    private enum ConstraintType
    {
        USAGE,
        ACTIVITY,
        ACTIVITY_TYPE,
        CUSTOMER_NAME,
        CUSTOMER_START,
        CUSTOMER_CLASS,
        LOCATION
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
                case ConstraintType.ACTIVITY:
                    if(actionTaken.customer.m_activity.m_type != m_activityType)
                        ruleApplies = false;
                    break;
                case ConstraintType.USAGE:
                    if(actionTaken.customer.m_dataUsage < m_bandwidth)
                        ruleApplies = false;
                    break;
                case ConstraintType.CUSTOMER_NAME:
                    if (actionTaken.customer.name == m_customer.name)
                        ruleApplies = false;
                    break;
                case ConstraintType.CUSTOMER_START:
                    if (actionTaken.customer.m_daysActive > m_daysActive)
                        ruleApplies = false;
                    break;
                // TODO: Define addtl constraints
                case ConstraintType.CUSTOMER_CLASS:
                    break;
                case ConstraintType.LOCATION:
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
    public RuleData AddBandwidthConstraint(float bandwidth) { this.m_bandwidth = bandwidth; AddConstraint(ConstraintType.USAGE); return this; }
    public RuleData AddActivityConstraint(ActivityData.Activity.Type activity) { this.m_activityType = activity; AddConstraint(ConstraintType.ACTIVITY); return this; }
    public RuleData AddCustomerConstraint(CustomerData customer) { this.m_customer = customer; AddConstraint(ConstraintType.CUSTOMER_NAME); return this; }

    private void AddConstraint(ConstraintType constraint) { m_constraints.Add(constraint); }

}
