using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PacketsPleaseMain : Singleton<PacketsPleaseMain> {

    public CustomerListUI m_customerListUI;
    public ActionPanelUI m_actionPanelUI;
    public NotificationListUI m_notificationUI;
    public NotificationPanelUI m_notificationPanelUI;
    public TitleBarUI m_titleBar;
    public DayDisplayUI m_dayDisplay;
    public float m_minTimeBetweenCustomers = 1f;
    public float m_maxTimeBetweenCustomers = 30f;
    public Shaker m_canvasShaker;

    public int m_maxStrikes = 3;
    public float m_timeBetweenCustomers = 2.0f;
    public float m_customerTimer = 0.0f;
    public int m_maxNumCustomer = 10;
    public float m_minTimeBetweenNotifications = 1.0f;
    public float m_notificationTimer = 0.0f;
    public float m_dayTransitionTime = 2.0f;

    public float m_actionFeedbackTime = 1.0f;

    protected bool m_isHandlingCustomer = false;
    protected int m_currentStrike = 0;
    protected int m_currentDay = 0;
    protected int m_numCorrectChoices = 0;

    protected StoryData testData;

    public enum GameState
    {
        Title,
        Transitioning,
        GameStarted,
        EndOfDay,
        GameOver,
    }

    protected GameState m_currentGameState = GameState.Transitioning;


    public DayData TEST_DAY;
    protected override void Awake()
    {
        base.Awake();
        EventManager.OnNotificationResolved.Register(HandleResolveNotification);
        EventManager.OnEndOfDay.Register(HandleEndOfDay);
    }

    protected void Start()
    {
        m_dayDisplay.FadeIn(true);
        TransitionDay();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.OnNotificationResolved.Unregister(HandleResolveNotification);
        EventManager.OnEndOfDay.Unregister(HandleEndOfDay);
    }

    public void HandleEndOfDay()
    {
        if(m_currentGameState == GameState.EndOfDay)
        {
            return;
        }
        
        m_currentGameState = GameState.EndOfDay;
        m_notificationUI.EmptyList();
        m_customerListUI.EmptyList();
        m_actionPanelUI.SetCustomer(null);

        NotificationData endOfDayData = ScriptableObject.CreateInstance<NotificationData>();
        endOfDayData.GenerateEndOfDay(m_currentDay, m_numCorrectChoices, m_customerListUI.m_totelCustomers);
        NotificationUI endOfDay = m_notificationUI.AddNotification(endOfDayData);
        endOfDay.SelectSelf();
    }

    public void TransitionDay()
    {
        EventManager.OnStartOfDay.Dispatch();
        StartCoroutine(HandleDayTransition());
    }

    public void GameOver()
    {
        EventManager.OnLose.Dispatch();
        m_currentGameState = GameState.GameOver;
    }

    protected IEnumerator HandleDayTransition()
    {
        m_currentGameState = GameState.Transitioning;

        // TODO: load generation stuff
        m_numCorrectChoices = 0;
        m_customerListUI.ResetList();
        m_notificationUI.ResetList();

        m_currentDay++;


        m_dayDisplay.SetDay(m_currentDay);
        m_titleBar.SetDay(m_currentDay);
        m_dayDisplay.FadeIn();
        yield return new WaitForSeconds(m_dayTransitionTime);
        GenerateDay();
        m_dayDisplay.FadeOut();

        EventManager.OnStartGameplay.Dispatch();
        m_currentGameState = GameState.GameStarted;
    }

    protected void GenerateDay()
    {
        testData = new StoryData("Story/TEST_DATA");
        // Kick out all TEST data as POC
        foreach (StoryData.ScheduledCustomer sc in testData.customerScheduleByDay[1])
        {
            m_customerListUI.AddCustomer(sc.m_data);
        }

        foreach (StoryData.ScheduledNotification sn in testData.notificationScheduleByDay[1])
        {
            m_notificationUI.AddNotification(sn.m_data);
        }

        //RuleManager.Instance.AddRule(new BandwidthRule(50f, ActionData.ActionType.Throttle));
        RuleManager.Instance.AddRule(new ActivityTypeRule(ActivityData.Activity.Type.GAME, ActionData.ActionType.Disconnect, 100.0f, 0));

        TEST_DAY.ApplyRules();
    }

    protected void Update()
    {
        switch(m_currentGameState)
        {
            case GameState.Title:
                break;
            case GameState.Transitioning:
                break;
            case GameState.GameStarted:
                UpdateGame();
                break;
            case GameState.EndOfDay:
                break;
            case GameState.GameOver:
                break;
        }
    }

    protected void UpdateGame()
    {
        m_titleBar.UpdateTime();

        m_customerTimer += Time.deltaTime;
        m_notificationTimer += Time.deltaTime;
        if (m_customerTimer >= 0)
        {
            m_customerTimer -= m_minTimeBetweenCustomers + UnityEngine.Random.Range(0f, m_maxTimeBetweenCustomers);
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
        EventManager.OnStrike.Dispatch(m_currentStrike);
    }

    public void ThrottleCustomer()
    {
        if (m_actionPanelUI.m_currentCustomer == null || m_isHandlingCustomer)
        {
            return;
        }
        StartCoroutine(HandleChoice(m_actionPanelUI.m_currentCustomer.m_data, ActionData.ActionType.Throttle));
    }

    public void BoostCustomer()
    {
        if (m_actionPanelUI.m_currentCustomer == null || m_isHandlingCustomer)
        {
            return;
        }
        StartCoroutine(HandleChoice(m_actionPanelUI.m_currentCustomer.m_data, ActionData.ActionType.Boost));
    }

    public void DisconnectCustomer()
    {
        if (m_actionPanelUI.m_currentCustomer == null || m_isHandlingCustomer)
        {
            return;
        }
        StartCoroutine(HandleChoice(m_actionPanelUI.m_currentCustomer.m_data, ActionData.ActionType.Disconnect));
    }

    protected IEnumerator HandleChoice(CustomerData data, ActionData.ActionType actionType)
    {
        m_isHandlingCustomer = true;
        ActionData action = new ActionData(data, actionType);
        if (RuleManager.Instance.DoesViolateRules(action))
        {
            GiveStrike();
        }
        else
        {
            m_numCorrectChoices++;
        }

        switch(actionType)
        {
            case ActionData.ActionType.Boost:
                m_actionPanelUI.DoBoostFeedback();
                break;
            case ActionData.ActionType.Throttle:
                m_actionPanelUI.DoThrottleFeedback();
                break;
            case ActionData.ActionType.Disconnect:
                m_actionPanelUI.DoDisconnectFeedback();
                break;
        }
        yield return new WaitForSeconds(m_actionFeedbackTime);
        m_customerListUI.RemoveCustomerTopCustomer();
        m_isHandlingCustomer = false;
    }

    public void HandleResolveNotification(NotificationUI notification, bool respondedCorrectly)
    {
        if (notification == null)
        {
            return;
        }

        if (respondedCorrectly)
        {
            switch (notification.m_data.m_correctResponseAction)
            {
                case NotificationData.ResolutionAction.TransitionDay:
                    TransitionDay();
                    break;
                case NotificationData.ResolutionAction.GameOver:
                    GameOver();
                    break;
            }
        }
        else
        {
            switch (notification.m_data.m_incorrectResponseAction)
            {
            }
        }

        if (!notification.m_data.m_pinned)
        {
            m_notificationUI.RemoveNotification(notification);
        }
    }
}
