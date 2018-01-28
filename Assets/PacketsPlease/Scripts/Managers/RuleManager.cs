using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class RuleManager : Singleton<RuleManager> {

    public const ActionData.ActionType DEFAULT_ACTION = ActionData.ActionType.Boost;
    public List<RuleData> Rules { get { return m_rules; } }
    [SerializeField]
    private List<RuleData> m_rules;

    public List<NotificationData> m_dailyNotifications;

    [System.Serializable]
    public struct RulesForRandomGeneration
    {
        public RuleData[] m_NormalRules;
        public RuleData[] m_PriorityRules;
    }

    [SerializeField]
    RulesForRandomGeneration rulesForRandomGeneration;

    protected override void Awake()
    {
        base.Awake();
        m_rules = new List<RuleData>();
        m_dailyNotifications = new List<NotificationData>();
    }

    public void ClearAllRules()
    {
        m_rules.Clear();
    }

    public void AddRule(RuleData rule)
    {
        m_rules.Add(rule);
    }


    // Determine whether the rules have been violated by an action`
    // Also populates actionTaken with sorted list of passed and violated rules
    public bool DoesViolateRules(ActionData actionTaken)
    {
        ActionData.ActionType requiredAction = GetRequiredAction(actionTaken.customer);

        return requiredAction != actionTaken.actionType;
    }

    public ActionData.ActionType GetRequiredAction(CustomerData customer)
    {
        ActionData.ActionType requiredAction = ActionData.ActionType.None;
        int currPriority = int.MinValue;
        foreach(RuleData rule in Rules)
        {
            ActionData.ActionType thisReqAction = rule.ActionRequired(customer);
            if (thisReqAction != ActionData.ActionType.None && rule.m_priority > currPriority)
            {
                requiredAction = thisReqAction;
                currPriority = rule.m_priority;
            }
        }
        return requiredAction;
    }
    
    public void ApplyRandomRules(int day)
    {
        List<RuleData> randomRules = new List<RuleData>();
        for (int i = 0; i < rulesForRandomGeneration.m_NormalRules.Length; i++)
            randomRules.Add(rulesForRandomGeneration.m_NormalRules[i]);

        while(Rules.Count < (day / 2) + 1 && randomRules.Count > 0)
        {
            RuleData draw = randomRules[Random.Range(0, randomRules.Count)];
            AddRule(draw);
            randomRules.Remove(draw);
        }
    }

    public void BuildRuleNotifications()
    {
        m_dailyNotifications.Clear();
        NotificationData companyPolicy = ScriptableObject.CreateInstance<NotificationData>();
        companyPolicy.m_pinned = true;
        companyPolicy.m_title = string.Format("Company Policy for {0}", TitleBarUI.GameDate.ToShortDateString());
        companyPolicy.m_sender = "policy@cosmocast.com";
        companyPolicy.m_message = "These are the latest active policies. Please follow them to ensure the best experience for our customers.\n\n";
        m_dailyNotifications.Add(companyPolicy);
        Dictionary<RuleData.Type, Dictionary<ActionData.ActionType,NotificationData>> checkoff = new Dictionary<RuleData.Type, Dictionary<ActionData.ActionType, NotificationData>>();

        foreach(RuleData rule in Rules)
        {
            if(rule.m_priority < RuleData.HIGHEST_PRIORITY)
            {
                switch (rule.m_type)
                {
                    case RuleData.Type.Bandwidth:
                        if (!checkoff.ContainsKey(RuleData.Type.Bandwidth))
                        {
                            #region BANDWIDTH
                            // Not actually used for anything other than single check here
                            checkoff.Add(RuleData.Type.Bandwidth, null);
                            BandwidthRule br = (BandwidthRule)rule;
                            NotificationData bandwidthMemo = ScriptableObject.CreateInstance<NotificationData>();
                            bandwidthMemo.m_pinned = true;
                            bandwidthMemo.m_title = string.Format("Bandwidth Memo", TitleBarUI.GameDate.ToShortDateString());
                            bandwidthMemo.m_sender = "policy@cosmocast.com";
                            bandwidthMemo.m_message = "At CosmoCast, we pride ourselves on offering service that can fit each and every customer. Here are the following <b>Usage Allotments</b> for our customer tiers.\n\n"
                                + string.Format("• <color=#FFD700>Gold</color> {0}\n• <color=#C0C0C0>Silver</color> {1}\n• <color=#CD7F32>Bronze</color> {2}", br.m_usageLimit * 4, br.m_usageLimit * 2, br.m_usageLimit);
                            companyPolicy.m_message += "• <color=red>Throttle</color> all users exceeding their usage limits\n";
                            m_dailyNotifications.Add(bandwidthMemo);
                            #endregion
                        }

                        break;
                    case RuleData.Type.Activity:
                        {
                            #region ACTIVITY
                            ActivityRule ar = (ActivityRule)rule;
                            if (!checkoff.ContainsKey(RuleData.Type.Activity))
                            {
                                checkoff.Add(RuleData.Type.Activity, new Dictionary<ActionData.ActionType, NotificationData>());
                            }
                            if (!checkoff[RuleData.Type.Activity].ContainsKey(rule.m_action))
                            {
                                NotificationData n = ScriptableObject.CreateInstance<NotificationData>();
                                switch (ar.m_action)
                                {
                                    case ActionData.ActionType.Disconnect:
                                        n.m_title = "Prohibited Services";
                                        n.m_sender = "policy@cosmocast.com";
                                        n.m_message = "The following are prohibited:\n";
                                        n.m_pinned = true;
                                        companyPolicy.m_message += "• <color=red>Disconnect</color> prohibited users and activities\n";
                                        break;
                                    case ActionData.ActionType.Throttle:
                                        n.m_title = "Restricted Services";
                                        n.m_sender = "policy@cosmocast.com";
                                        n.m_message = "The following activities are restricted:\n";
                                        n.m_pinned = true;
                                        companyPolicy.m_message += "• <color=red>Throttle</color> restricted users and activities\n";
                                        break;
                                    case ActionData.ActionType.Boost:
                                        n.m_title = "Promotions";
                                        n.m_sender = "policy@cosmocast.com";
                                        n.m_message = "The following are in promotion due to partnerships and acquisition!\n";
                                        n.m_pinned = true;
                                        companyPolicy.m_message += "• <color=#00FFFF>Boost</color> according to all promotions\n";
                                        break;
                                }
                                checkoff[RuleData.Type.Activity].Add(ar.m_action, n);
                                m_dailyNotifications.Add(n);
                            }
                            NotificationData notification = checkoff[RuleData.Type.Activity][rule.m_action];
                            if (rule.m_usageLimit > 0f)
                            {
                                notification.m_message += string.Format("• {0} above {1} GB\n", ar.m_activityName, ar.m_usageLimit);
                            }
                            else
                            {
                                notification.m_message += string.Format("• {0}\n", ar.m_activityName);
                            }
                            #endregion
                        }
                        break;
                    case RuleData.Type.ActivityType:
                        {
                            #region ACTIVITY TYPE
                            ActivityTypeRule at = (ActivityTypeRule)rule;
                            if (!checkoff.ContainsKey(RuleData.Type.Activity))
                            {
                                checkoff.Add(RuleData.Type.Activity, new Dictionary<ActionData.ActionType, NotificationData>());
                            }
                            // Still use activity here so they share the same notifications
                            if (!checkoff[RuleData.Type.Activity].ContainsKey(rule.m_action))
                            {
                                NotificationData n = ScriptableObject.CreateInstance<NotificationData>();
                                switch (at.m_action)
                                {
                                    case ActionData.ActionType.Disconnect:
                                        n.m_title = "Prohibited Services";
                                        n.m_sender = "policy@cosmocast.com";
                                        n.m_message = "The following are prohibited:\n";
                                        n.m_pinned = true;
                                        companyPolicy.m_message += "• <color=red>Disconnect</color> prohibited users and activities\n";
                                        break;
                                    case ActionData.ActionType.Throttle:
                                        n.m_title = "Restricted Services";
                                        n.m_sender = "policy@cosmocast.com";
                                        n.m_message = "The following are restricted:\n";
                                        n.m_pinned = true;
                                        companyPolicy.m_message += "• <color=red>Throttle</color> restricted users and activities\n";
                                        break;
                                    case ActionData.ActionType.Boost:
                                        n.m_title = "Promotions";
                                        n.m_sender = "policy@cosmocast.com";
                                        n.m_message = "The following are in promotion due to partnerships and acquisition!\n";
                                        n.m_pinned = true;
                                        companyPolicy.m_message += "• <color=#00FFFF>Boost</color> according to all promotions\n";
                                        break;
                                }
                                checkoff[RuleData.Type.Activity].Add(at.m_action, n);
                                m_dailyNotifications.Add(n);
                            }
                            NotificationData notification = checkoff[RuleData.Type.Activity][rule.m_action];

                            // Shitty name transpose
                            string nameForMessage = "";
                            switch (at.m_activityType)
                            {
                                case ActivityData.Activity.Type.GAME:
                                    nameForMessage = "Gaming services";
                                    break;
                                case ActivityData.Activity.Type.SITE:
                                    nameForMessage = "Standard browsing";
                                    break;
                                case ActivityData.Activity.Type.STREAM:
                                    nameForMessage = "Streaming";
                                    break;
                                case ActivityData.Activity.Type.SELF:
                                    nameForMessage = "CosmoCast";
                                    break;
                            }

                            if (at.m_tier != CustomerData.SpeedTier.NONE)
                            {
                                notification.m_message += string.Format("• {0}{1} for users in the {2} tier or below\n", 
                                    at.m_inverseOfActivity? "All services EXCEPT " : "", nameForMessage.ToString(), at.m_tier);
                            }
                            else if (rule.m_usageLimit > 0f)
                            {
                                notification.m_message += string.Format("• {0} above {1} GB\n", nameForMessage, at.m_usageLimit);
                            }
                            else
                            {
                                notification.m_message += string.Format("• {0}\n", nameForMessage.ToString());
                            }
                            #endregion
                        }
                        break;
                    case RuleData.Type.Date:
                        if (!checkoff.ContainsKey(RuleData.Type.Date))
                        {
                            checkoff.Add(RuleData.Type.Date, null);
                            companyPolicy.m_message += string.Format("• <color=#00FFFF>Boost</color> users who started in the last {0} days\n", ((DateRule)rule).m_daysActive);
                        }
                        break;
                    case RuleData.Type.Location:
                        {
                            #region Location
                            LocationRule lr = (LocationRule)rule;
                            if (!checkoff.ContainsKey(RuleData.Type.Activity))
                            {
                                checkoff.Add(RuleData.Type.Activity, new Dictionary<ActionData.ActionType, NotificationData>());
                            }
                            // Using Activity as shared key for these lists
                            if (!checkoff[RuleData.Type.Activity].ContainsKey(rule.m_action))
                            {
                                NotificationData n = ScriptableObject.CreateInstance<NotificationData>();
                                switch (lr.m_action)
                                {
                                    case ActionData.ActionType.Disconnect:
                                        n.m_title = "Prohibited Services";
                                        n.m_sender = "policy@cosmocast.com";
                                        n.m_message = "The following are prohibited:\n";
                                        n.m_pinned = true;
                                        companyPolicy.m_message += "• <color=red>Disconnect</color> prohibited users and activities\n";
                                        break;
                                    case ActionData.ActionType.Throttle:
                                        n.m_title = "Restricted Services";
                                        n.m_sender = "policy@cosmocast.com";
                                        n.m_message = "The following are restricted:\n";
                                        n.m_pinned = true;
                                        companyPolicy.m_message += "• <color=red>Throttle</color> restricted users and activities\n";
                                        break;
                                    case ActionData.ActionType.Boost:
                                        n.m_title = "Promotions";
                                        n.m_sender = "policy@cosmocast.com";
                                        n.m_message = "The following are in promotion due to partnerships and acquisition!\n";
                                        n.m_pinned = true;
                                        companyPolicy.m_message += "• <color=#00FFFF>Boost</color> according to all promotions\n";
                                        break;
                                }
                                checkoff[RuleData.Type.Activity].Add(lr.m_action, n);
                                m_dailyNotifications.Add(n);
                            }
                            NotificationData notification = checkoff[RuleData.Type.Activity][rule.m_action];
                            if (rule.m_usageLimit > 0f)
                            {
                                notification.m_message += string.Format("• Connections from {0} over {1}", CustomerData.LOCATION_NAMES[(int)lr.m_location], lr.m_usageLimit);
                            }
                            else
                            {
                                notification.m_message += string.Format("• All connections from {0}\n", CustomerData.LOCATION_NAMES[(int)lr.m_location]);
                            }
                            #endregion
                        }
                        break;
                    case RuleData.Type.Individual:
                        {
                            IndividualRule ir = (IndividualRule)rule;
                            NotificationData n = ScriptableObject.CreateInstance<NotificationData>();
                            m_dailyNotifications.Add(n);
                            switch(ir.m_action)
                            {
                                case ActionData.ActionType.Boost:
                                    {
                                        n.m_title = "Important VIP";
                                        n.m_sender = "partnerships@cosmocast.net";
                                        n.m_message = string.Format("Make sure {0} gets the best possible service CosmoCast has to offer, no ifs, ands, or buts. We value our VIPs here at CosmoCast, and want to keep them onboard for years to come.", ir.m_name);
                                    }
                                    break;
                                case ActionData.ActionType.Throttle:
                                    {
                                        n.m_title = "User Harm Mitigation";
                                        n.m_sender = "legal@cosmocast.net";
                                        n.m_message = string.Format("AS-yet-unsubstantiated rumors about the internet activies of {0} have lead us to request a temporary reduction of their services. Please reduce their connection speed until we can ascertain the full extent of any non-compliant activies.", ir.name); 
                                    }
                                    break;
                                case ActionData.ActionType.Disconnect:
                                    {
                                        n.m_title = "<color=red>SERVICE CESSATION</color>";
                                        n.m_sender = "discoBot@cosmocast.net";
                                        n.m_message = string.Format("<color=red>HIGH PRIORITY</color>: DISCONNECTION OF ALL SERVICES FOR {0} EFFECTIVE IMMEDIATELY.", ir.name.ToUpper());
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
        }
    }

    private void DumpRulesAndCrash(ActionData a, string msg)
    {
        Debug.Log(a.AllRulesToStr());
        throw new System.Exception(msg);
    }
}
