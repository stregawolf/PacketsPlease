using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Data/CustomerData", order = 1)]
public class CustomerData : ScriptableObject {
    public NameGen.Name m_name;
    public float m_dataUsage;
    public int m_daysActive;
    public ActivityData.Activity m_activity;
    public bool m_male = false; // Deprecated

    public static float noRulesWeight = 0.5f;
    public const float MAX_DATA_USAGE = 999.99f;
    public const int MAX_DAYS_ACTIVE = 999;

    // Generate new customer that somehow fits into one of the rule categories
    public void Generate()
    {
        List<RuleData> rules = RuleManager.Instance.Rules;

        GenerateTrueRandom();

        if(rules.Count == 0)
        {
            // If there are no rules available, just generate a totally random Customer
            return;
        }
        else
        {
            // Decide if a rule should apply to this Customer
            if(Random.Range(0f,1f) < noRulesWeight)
            {
                return;
            }

            // Pick a random rule and make sure the Customer fits the constraints
            RuleData rule = rules[Random.Range(0, rules.Count)];
            HashSet<RuleData.ConstraintType> constraints = rule.Constraints;

            foreach(RuleData.ConstraintType constraint in constraints)
            {
                //TODO: FIX THIS
                switch(constraint)
                {
                    case RuleData.ConstraintType.ACTIVITY_NAME:
                        m_activity = ActivityData.GetActivityByName(rule.m_activityName);
                        break;
                    case RuleData.ConstraintType.ACTIVITY_TYPE:
                        m_activity = ActivityData.GetActivityByType(rule.m_activityType);
                        break;
                    case RuleData.ConstraintType.MAX_USAGE_HIGHER:
                        m_dataUsage = Random.Range(rule.m_bandwidth, MAX_DATA_USAGE);
                        break;
                    case RuleData.ConstraintType.MAX_USAGE_LOWER:
                        m_dataUsage = Random.Range(0f, rule.m_bandwidth);
                        break;
                    case RuleData.ConstraintType.CUSTOMER_NAME:
                        m_name = rule.m_customerName;
                        break;
                    case RuleData.ConstraintType.CUSTOMER_START:
                        m_daysActive = rule.m_daysActive;
                        break;
                    // TODO: Define addtl constraints
                    case RuleData.ConstraintType.CUSTOMER_CLASS:
                        throw new System.NotImplementedException();
                        break;
                    case RuleData.ConstraintType.LOCATION:
                        throw new System.NotImplementedException();
                        break;
                    default:
                        throw new System.Exception("CustomerData: Missing constraint types in IsViolatedBy()");
                }
            }

        }
    }

    private void GenerateTrueRandom()
    {
        m_male = Random.value > 0.5f;
        m_name = NameGen.GetName(m_male);
        m_dataUsage = Random.Range(0.0f, MAX_DATA_USAGE);
        m_daysActive = Random.Range(0, MAX_DAYS_ACTIVE);
        m_activity = ActivityData.GetActivity();
    }

}
