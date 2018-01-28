using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketsPleaseMain : Singleton<PacketsPleaseMain> {

    public CustomerListUI m_customerListUI;
    public ActionPanelUI m_actionPanelUI;
    public NotificationListUI m_notificationUI;
    public NotificationPanelUI m_notificationPanelUI;
    public DayDisplayUI m_dayDispay;
    public float m_minTimeBetweenCustomers = 1f;
    public float m_maxTimeBetweenCustomers = 30f;
    public Shaker m_canvasShaker;

    public int m_maxStrikes = 3;
    public float m_timeBetweenCustomers = 2.0f;
    public float m_customerTimer = 0.0f;
    public int m_maxNumCustomer = 10;
    public float m_minTimeBetweenNotifications = 1.0f;
    public float m_notificationTimer = 0.0f;
    public float m_dayTransitionTime = 1.0f;

    public float m_actionFeedbackTime = 1.0f;

    public System.Action<int> OnStrike;

    protected bool m_isHandlingCustomer = false;
    protected int m_currentStrike = 0;
    protected int m_currentDay;

    protected StoryData testData;

    public enum GameState
    {
        Transitioning,
        GameStarted,
        GameOver,
    }

    protected GameState m_currentGameState = GameState.Transitioning;

    protected override void Awake()
    {
        base.Awake();

        testData = new StoryData("Story/TEST_DATA");
        // Kick out all TEST data as POC
        foreach(StoryData.ScheduledCustomer sc in testData.customerScheduleByDay[1])
        {
            m_customerListUI.AddCustomer(sc.m_data);
        }

        foreach(StoryData.ScheduledNotification sn in testData.notificationScheduleByDay[1])
        {
            m_notificationUI.AddNotification(sn.m_data);
        }

        RuleManager.Instance.AddRule(new BandwidthRule(50f, ActionType.Throttle));

        EventManager.OnNotificationResolved.Register(HandleResolveNotification);
        m_dayDispay.FadeIn(true);
        TransitionDay();
    }

    public void TransitionDay()
    {
        StartCoroutine(HandleDayTransition());
    }

    protected IEnumerator HandleDayTransition()
    {
        m_currentGameState = GameState.Transitioning;

        // TODO: load generation stuff
        m_currentDay++;
        m_dayDispay.SetDay(m_currentDay);
        m_dayDispay.FadeIn();
        yield return new WaitForSeconds(m_dayTransitionTime);
        m_dayDispay.FadeOut();

        m_currentGameState = GameState.GameStarted;
    }

    protected void Update()
    {
        switch(m_currentGameState)
        {
            case GameState.Transitioning:
                break;
            case GameState.GameStarted:
                UpdateGame();
                break;
            case GameState.GameOver:
                break;
        }
    }

    protected void UpdateGame()
    {
        m_customerTimer += Time.deltaTime;
        m_notificationTimer += Time.deltaTime;
        if (m_customerTimer >= 0)
        {
            m_customerTimer -= m_minTimeBetweenCustomers + Random.Range(0f, m_maxTimeBetweenCustomers);
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

        CustomerUI topCustomer = m_customerListUI.GetTopCustomer();
        if (m_actionPanelUI.m_currentCustomer != topCustomer)
        {
            if (topCustomer == null || topCustomer.transform.localPosition.y < 5.0f)
            {
                UpdateCustomerDisplay(topCustomer);
            }
        }

        if (m_notificationTimer >= m_minTimeBetweenNotifications)
        {
            m_notificationTimer = 0;
            if (UnityEngine.Random.value < 0.1f)
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

    protected void GiveStrike()
    {
        m_currentStrike++;
        m_notificationUI.AddStrikeNotification(m_currentStrike);
        m_canvasShaker.Shake();
        OnStrike(m_currentStrike);
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
        ActionData action = new ActionData(m_customerListUI.GetTopCustomer().m_data, ActionType.Throttle);
        if(RuleManager.Instance.DoesViolateRules(action))
        {
            GiveStrike();
        }
        m_actionPanelUI.DoThrottleFeedback();
        yield return new WaitForSeconds(m_actionFeedbackTime);
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
        ActionData action = new ActionData(m_customerListUI.GetTopCustomer().m_data, ActionType.Boost);
        if(RuleManager.Instance.DoesViolateRules(action))
        {
            GiveStrike();
        }
        m_actionPanelUI.DoBoostFeedback();
        yield return new WaitForSeconds(m_actionFeedbackTime);
        m_customerListUI.RemoveCustomerTopCustomer();
        m_isHandlingCustomer = false;
    }

    public void HandleResolveNotification(NotificationUI notification)
    {
        if (!notification.m_data.m_pinned)
        {
            m_notificationUI.RemoveNotification(notification);
        }
    }

    public void DisconnectCustomer()
    {
        if (m_isHandlingCustomer || m_customerListUI.GetNumCustomers() <= 0)
        {
            return;
        }
        StartCoroutine(HandleDisconnectTopCustomer());
    }

    protected IEnumerator HandleDisconnectTopCustomer()
    {
        m_isHandlingCustomer = true;
        ActionData action = new ActionData(m_customerListUI.GetTopCustomer().m_data, ActionType.Disconnect);
        if(RuleManager.Instance.DoesViolateRules(action))
        {
            GiveStrike();
        }
        m_actionPanelUI.DoDisconnectFeedback();
        yield return new WaitForSeconds(m_actionFeedbackTime);
        m_customerListUI.RemoveCustomerTopCustomer();
        m_isHandlingCustomer = false;
    }
}
