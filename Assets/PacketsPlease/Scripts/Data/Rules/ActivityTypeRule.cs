using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Activity Type", menuName = "Rules/Activity Type", order = 0)]
public class ActivityTypeRule : RuleData {

    public ActivityData.Activity.Type m_activityType;

    public CustomerData.SpeedTier m_tier;

    public bool m_inverseOfActivity;

    private List<ActivityData.Activity> passingActivities;
    private List<ActivityData.Activity> failingActivities;
    
    public ActivityTypeRule(ActivityData.Activity.Type type, ActionData.ActionType correctResponse, CustomerData.SpeedTier tier = CustomerData.SpeedTier.NONE, bool inverseOfAction = false, float usageLimit = 0, int priority = 0) : base(correctResponse, usageLimit, priority)
    {
        m_activityType = type;
        m_tier = tier;
        m_inverseOfActivity = inverseOfAction;
    }

    private bool activityMatch(ActivityData.Activity activity) {
        if(m_inverseOfActivity) {
            return activity.m_type == m_activityType;
        }
        else {
            return activity.m_type != m_activityType;
        }
    }

    public override void Init()
    {
        base.Init();
        m_type = Type.ActivityType;

        passingActivities = new List<ActivityData.Activity>();
        failingActivities = new List<ActivityData.Activity>();

        foreach(ActivityData.Activity activity in ActivityData.Activities)
        {
            if(activityMatch(activity))
            {
                failingActivities.Add(activity);
            } else
            {
                passingActivities.Add(activity);
            }
        }
    }

    public override ActionData.ActionType ActionRequired(CustomerData customer)
    {
        bool dataRequirement = (m_usageLimit <= 0 || customer.m_dataUsage > m_usageLimit);
        if (activityMatch(customer.m_activity) && dataRequirement)
        {
            return m_action;
        }
        return ActionData.ActionType.None;
    }

    public override void MakePass(CustomerData customer)
    {
        if (activityMatch(customer.m_activity))
        {
            if(m_usageLimit > 0 && Random.value < 0.5f)
            {
                customer.m_dataUsage = Random.Range(Mathf.Min(m_usageLimit, 0.1f), m_usageLimit);
            }
            else
            {
                customer.m_activity = passingActivities[Random.Range(0, passingActivities.Count)];
            }
        }

        if (m_tier != CustomerData.SpeedTier.NONE) {
            customer.m_speedTier = (CustomerData.SpeedTier) Random.Range((int) CustomerData.SpeedTier.Bronze, (int) m_tier + 1);
        }
    }

    public override void MakeFail(CustomerData customer)
    {
        customer.m_activity = failingActivities[Random.Range(0, failingActivities.Count)];
        
        if(m_tier != CustomerData.SpeedTier.NONE) {
            customer.m_speedTier = (CustomerData.SpeedTier) Random.Range( (int) CustomerData.SpeedTier.Bronze, (int) m_tier + 1);
        }

        base.MakeFail(customer);
    }
}
