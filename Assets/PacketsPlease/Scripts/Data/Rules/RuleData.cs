using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    RuleData can be initialized in two ways:

    1. By using the subclasses such as MinUsageRuleData, etc.

    2. By using the RuleData base constructor and chaining AddXXXConstraint() methods to it
    example:
        new RuleData(0, ActivityType.Throttle).AddActivityConstraint(ActivityType.Streaming)

    will create a rule to throttle all streaming activities

 */
public class RuleData {

    // TODO: MAKE GETTERS FOR THESE
    public int m_priority; // Higher priority number is more important
    public float m_bandwidth; // Bandwidth of 0 or less = ignore
    public int m_daysActive;
    public string m_activityName;
    public ActivityData.Activity.Type m_activityType;
    public NameGen.Name m_customerName;
    public ActionType m_correctActionType;

    public const int HIGHEST_PRIORITY = 999;
    public const int LOWEST_PRIORITY = 0;

    public enum ConstraintType
    {
        MAX_USAGE_HIGHER,
        MAX_USAGE_LOWER,
        ACTIVITY_NAME,
        ACTIVITY_TYPE,
        CUSTOMER_NAME,
        CUSTOMER_START,
        CUSTOMER_CLASS,
        LOCATION
    }

    public HashSet<ConstraintType> Constraints { get { return m_constraints; } }
    private HashSet<ConstraintType> m_constraints;

    // Minimal constructor; constraints can be applied by chaining the Add***Constraint() functions
    // As is, it would say "apply this action to everyone"
    public RuleData(ActionType correctActionType, int priority = LOWEST_PRIORITY)
    {
        m_priority = priority;
        m_correctActionType = correctActionType;
        m_constraints = new HashSet<ConstraintType>();
    }

    // Check if action taken violates any constraints of the rule
    public bool IsViolated(CustomerData customer, ActionType actionTaken)
    {
        bool ruleApplies = true;
        foreach(ConstraintType constraint in m_constraints)
        {
            // Compare constraints to customer data
            switch(constraint)
            {
                case ConstraintType.ACTIVITY_NAME:
                    if(customer.m_activity.m_name != m_activityName)
                        ruleApplies = false;
                    break;
                case ConstraintType.ACTIVITY_TYPE:
                    if(customer.m_activity.m_type != m_activityType)
                        ruleApplies = false;
                    break;
                case ConstraintType.MAX_USAGE_HIGHER:
                    if(customer.m_dataUsage < m_bandwidth)
                        ruleApplies = false;
                    break;
                case ConstraintType.MAX_USAGE_LOWER:
                    if(customer.m_dataUsage >= m_bandwidth)
                        ruleApplies = false;
                    break;
                case ConstraintType.CUSTOMER_NAME:
                    if (customer.m_name != m_customerName)
                        ruleApplies = false;
                    break;
                case ConstraintType.CUSTOMER_START:
                    if (customer.m_daysActive > m_daysActive)
                        ruleApplies = false;
                    break;
                // TODO: Define addtl constraints
                case ConstraintType.CUSTOMER_CLASS:
                    throw new System.NotImplementedException();
                    break;
                case ConstraintType.LOCATION:
                    throw new System.NotImplementedException();
                    break;
                default:
                    throw new System.Exception("RuleData: Missing constraint types in IsViolatedBy()");
            }
        }

        // Check for violation
        if(ruleApplies)
        {
            if(actionTaken != m_correctActionType)
            {
                return true;
            }
        }

        return false;
    }

    // Helper functions for Constraint set
    public RuleData AddBandwidthHigherConstraint(float bandwidth) { this.m_bandwidth = bandwidth; AddConstraint(ConstraintType.MAX_USAGE_HIGHER); return this; }
    public RuleData AddBandwidthLowerConstraint(float bandwidth) { this.m_bandwidth = bandwidth; AddConstraint(ConstraintType.MAX_USAGE_LOWER); return this; }
    public RuleData AddActivityNameConstraint(string name) { this.m_activityName = name; AddConstraint(ConstraintType.ACTIVITY_NAME); return this; }
    public RuleData AddActivityTypeConstraint(ActivityData.Activity.Type type) { this.m_activityType = type; AddConstraint(ConstraintType.ACTIVITY_TYPE); return this; }
    public RuleData AddCustomerNameConstraint(NameGen.Name name) { this.m_customerName = name; AddConstraint(ConstraintType.CUSTOMER_NAME); return this; }
    // public RuleData AddCustomerClassConstraint(CustomerData customer) { this.m_customer = customer; AddConstraint(ConstraintType.CUSTOMER_CLASS); return this; }
    // public RuleData AddCustomerLocationConstraint(CustomerData customer) { this.m_customer = customer; AddConstraint(ConstraintType.LOCATION); return this; }

    private void AddConstraint(ConstraintType constraint) { m_constraints.Add(constraint); }

}
