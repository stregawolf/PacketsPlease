using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Activity Type", menuName = "Rules/Activity Type", order = 0)]
public class ActivityTypeRule : RuleData {

    public ActivityData.Activity.Type m_activityType;

    private List<ActivityData.Activity> passingActivites;
    private List<ActivityData.Activity> failingActivities;

    public ActivityTypeRule(ActivityData.Activity.Type type, ActionData.ActionType correctResponse, float usageLimit = 0, int priority = 0) : base(correctResponse, usageLimit, priority)
    {
        m_activityType = type;
    }

    protected override void Init()
    {
        base.Init();
        m_type = Type.ActivityType;

        passingActivites = new List<ActivityData.Activity>();
        failingActivities = new List<ActivityData.Activity>();

        foreach(ActivityData.Activity activity in ActivityData.Activities)
        {
            if(activity.m_type == m_activityType)
            {
                failingActivities.Add(activity);
            } else
            {
                passingActivites.Add(activity);
            }
        }
    }

    public override ActionData.ActionType ActionRequired(CustomerData customer)
    {
        bool dataRequirement = (m_usageLimit <= 0 || customer.m_dataUsage > m_usageLimit);
        if (customer.m_activity.m_type == m_activityType && dataRequirement)
        {
            return m_action;
        }
        return ActionData.ActionType.None;
    }

    public override void MakePass(CustomerData customer)
    {
        if (customer.m_activity.m_type == m_activityType)
        {
            if(m_usageLimit > 0 && Random.value < 0.5f)
            {
                customer.m_dataUsage = Random.Range(Mathf.Min(m_usageLimit, 0.1f), m_usageLimit);
            }
            else
            {
                customer.m_activity = passingActivites[Random.Range(0, passingActivites.Count)];
            }
        }
    }

    public override void MakeFail(CustomerData customer)
    {
        customer.m_activity = failingActivities[Random.Range(0, failingActivities.Count)];
        base.MakeFail(customer);
    }
}
