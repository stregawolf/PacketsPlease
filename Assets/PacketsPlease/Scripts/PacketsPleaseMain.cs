using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketsPleaseMain : Singleton<PacketsPleaseMain> {

    public CustomerListUI m_customerListUI;
    public ActionPanelUI m_actionPanelUI;
    public NotificationListUI m_notificationUI;
    public NotificationPanelUI m_notificationPanelUI;
    public CustomerUI m_customerDisplay;

    public float m_timeBetweenCustomers = 2.0f;
    public float m_customerTimer = 0.0f;
    public int m_maxNumCustomer = 10;
    public float m_minTimeBetweenNotifications = 1.0f;
    public float m_notificationTimer = 0.0f;

    protected bool m_isHandlingCustomer = false;
    protected int m_currentStrike = 0;
    protected CustomerData currentCustomer;

    protected void Start()
    {
        RuleData throttleOver50 = new UsageHigherRule(50f, ActionType.Throttle, RuleData.HIGHEST_PRIORITY);
        RuleData boostUnder50 = new UsageLowerRule(50f, ActionType.Boost, RuleData.HIGHEST_PRIORITY);

        RuleManager.Instance.ClearAllRules();
        RuleManager.Instance.AddRule(throttleOver50);
        RuleManager.Instance.AddRule(boostUnder50);
    }

    protected void Update()
    {
        m_customerTimer += Time.deltaTime;
        m_notificationTimer += Time.deltaTime;
        if(m_customerTimer >= m_timeBetweenCustomers)
        {
            m_customerTimer -= m_timeBetweenCustomers;
            if (m_customerListUI.GetNumCustomers() < m_maxNumCustomer)
            {
                CustomerData data = ScriptableObject.CreateInstance<CustomerData>();
                data.Generate();
                m_customerListUI.AddCustomer(data);
            }
            else
            {
                // give strike for max customers reached
                GiveStrike();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            m_customerListUI.RemoveCustomerTopCustomer();
        }

        if(currentCustomer != m_customerListUI.GetTopCustomer())
        {
            UpdateCustomerDisplay(m_customerListUI.GetTopCustomer());
        }

        if (m_notificationTimer >= m_minTimeBetweenNotifications)
        {
            m_notificationTimer = 0;
            if (Random.value < 0.1f)
            {
                NotificationData testData = ScriptableObject.CreateInstance<NotificationData>();
                testData.Generate();
                m_notificationUI.AddNotification(testData);
            }
        }

    }

    protected void UpdateCustomerDisplay(CustomerUI newCustomer)
    {
        m_actionPanelUI.SetCustomer(newCustomer);
    }

    public void UpdateNotification(NotificationUI newNotification)
    {
        m_notificationPanelUI.SetNotification(newNotification);
    }

    protected void GiveStrike()
    {
        m_currentStrike++;
        m_notificationUI.AddStrikeNotification(m_currentStrike);
    }

    public void ThrottleCustomer()
    {
        if (m_isHandlingCustomer || m_customerListUI.GetNumCustomers() <= 0)
        {
            return;
        }
        StartCoroutine(HandleThrottlingTopCustomer());
    }

    protected IEnumerator HandleThrottlingTopCustomer()
    {
        m_isHandlingCustomer = true;
        if(RuleManager.Instance.GetHighestViolatedRule(m_customerListUI.GetTopCustomer().m_data, ActionType.Throttle) != null)
        {
            GiveStrike();
        }
        yield return new WaitForSeconds(0.33f);
        m_customerListUI.RemoveCustomerTopCustomer();
        m_isHandlingCustomer = false;
    }

    public void BoostCustomer()
    {
        if(m_isHandlingCustomer || m_customerListUI.GetNumCustomers() <= 0)
        {
            return;
        }
        StartCoroutine(HandleBoostingTopCustomer());
    }

    protected IEnumerator HandleBoostingTopCustomer()
    {
        m_isHandlingCustomer = true;
        if (RuleManager.Instance.GetHighestViolatedRule(m_customerListUI.GetTopCustomer().m_data, ActionType.Boost) != null)
        {
            GiveStrike();
        }
        yield return new WaitForSeconds(0.33f);
        m_customerListUI.RemoveCustomerTopCustomer();
        m_isHandlingCustomer = false;
    }

    public void DisconnectCustomer()
    {
        if (m_isHandlingCustomer || m_customerListUI.GetNumCustomers() <= 0)
        {
            return;
        }
    }
}
