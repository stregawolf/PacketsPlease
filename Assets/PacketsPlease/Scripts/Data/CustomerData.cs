﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Data/CustomerData", order = 1)]
public class CustomerData : ScriptableObject {
    
    public const float MAX_DATA_USAGE = 100.0f;
    public const int MAX_DAYS_ACTIVE = 10000;
    public static readonly string[] LOCATION_NAMES = {
            "North America",
            "South America",
            "Europe",
            "Asia",
            "Africa",
            "Australia",
        };


    public NameGen.Name m_name;
    public float m_dataUsage;
    public int m_daysActive;
    public ActivityData.Activity m_activity;
    public bool m_ignoreRules;

    public RuleData m_failTrigger = null;

    [System.Serializable]
    public class ParentStory
    {
        [HideInInspector]
        public StoryData m_story;
        [System.Serializable]
        public class ResponseAction
        {
            public ActionData.ActionType m_action = ActionData.ActionType.None;
            public ResolutionAction resolution = ResolutionAction.None;
        }

        public List<ResponseAction> m_responseActions = new List<ResponseAction>();
        public bool m_ForcePassFail = true;
        public string m_activityName;
        public NotificationData m_strikeNotification;
    }


    public enum ResolutionAction
    {
        None = 0,
        TransitionDay,
        GameOver,
        EndStory,
        Strike,
        PostNotificationA,
        PostNotificationB,
        PostNotificationC,
        PostCustomerA,
        PostCustomerB,
        PostCustomerC
    }

    public ParentStory m_StoryParameters;

    public List<NotificationData> m_responseNotifications;
    public List<CustomerData> m_responseCustomers;
    public enum Location : int
    {
        NorthAmerica = 0,
        SouthAmerica,
        Europe,
        Asia,
        Africa,
        Australia,
        NUM_LOCATIONS,
    }
    public Location m_location;

    public enum Race : int
    {
        Bird = 0,
        Reptile,
        SmallMammal,
        LargeMammal,
        NUM_RACES,
        VIP,
    }
    public Race m_race;

    public enum SpeedTier: int
    {
        Bronze = 0, 
        Silver,
        Gold,
        NUM_TIERS,
        NONE,
    }
    public SpeedTier m_speedTier;

    public Sprite m_neutralSprite;
    public Sprite m_positiveSprite;
    public Sprite m_negativeSprite;

    // Generate new customer that somehow fits into one of the rule categories
    public void Generate()
    {
        GenerateTrueRandom();
        ForcePassFail();
    }

    public void ForcePassFail()
    {
        List<RuleData> rules = RuleManager.Instance.Rules;
        if (rules.Count == 0)
        {
            // If there are no rules available, just generate a totally random Customer
            return;
        }
        else
        {
            // Make character pass all rules
            foreach (RuleData rule in RuleManager.Instance.Rules)
            {
                rule.MakePass(this);
            }

            // Make character fail one rule.
            m_failTrigger = RuleManager.Instance.Rules[Random.Range(0, RuleManager.Instance.Rules.Count)];
            m_failTrigger.MakeFail(this);
        }
    }

    public void GenerateTrueRandom()
    {
        m_name = NameGen.GetName();
        m_dataUsage = Random.Range(0.0f, MAX_DATA_USAGE);
        m_daysActive = Random.Range(0, MAX_DAYS_ACTIVE);
        m_activity = ActivityData.GetActivity();
        m_location = (Location)Random.Range(0, (int)Location.NUM_LOCATIONS);
        m_race = (Race)Random.Range(0, (int)Race.NUM_RACES);
        m_speedTier = (SpeedTier)Random.Range(0, (int)SpeedTier.NUM_TIERS);
    }

    public override string ToString()
    {
        return string.Format(
            "Customer(Name {0} Male {1} DataUsg {2} DaysAct {3} ActType {4} Loc {5}",
            m_name, m_dataUsage, m_daysActive, m_activity.ToString(), m_location
        );
    }

}
