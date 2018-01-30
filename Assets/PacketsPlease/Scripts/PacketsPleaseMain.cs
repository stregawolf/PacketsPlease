using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PacketsPleaseMain : Singleton<PacketsPleaseMain> {

    public DayData[] m_days;
    public List<StoryData> m_stories;
    public CustomerListUI m_customerListUI;
    public ActionPanelUI m_actionPanelUI;
    public NotificationListUI m_notificationUI;
    public NotificationPanelUI m_notificationPanelUI;
    public TitleBarUI m_titleBar;
    public DayDisplayUI m_dayDisplay;
    public TitleUI m_title;
    public float m_minTimeBetweenCustomers = 1f;
    public float m_maxTimeBetweenCustomers = 30f;
    public Shaker m_canvasShaker;
    public UnityEngine.UI.Button m_loginButton;

    public float m_readRulesGracePeriod = 15f;
    public int m_maxStrikes = 3;
    public float m_timeBetweenCustomers = 2.0f;
    public float m_customerTimer = 0.0f;
    public int m_maxNumCustomer = 10;
    public float m_minTimeBetweenNotifications = 1.0f;
    public float m_notificationTimer = 0.0f;
    public float m_dayTransitionTime = 2.0f;

    public float m_actionFeedbackTime = 1.0f;

    public float m_customerTimeReductionPerDay = 0.5f;

    protected bool m_isHandlingCustomer = false;
    protected int m_currentStrike = 0;
    protected int m_currentDay = 0;
    protected int m_numCorrectChoices = 0;
    protected List<StoryData> m_activeStories = new List<StoryData>();

    protected StoryData testData;

    public enum GameState
    {
        Title,
        Transitioning,
        GameStarted,
        EndOfDay,
        EndOfDayReport,
        GameOver,
        GameOverShown,
    }

    protected GameState m_currentGameState = GameState.Title;


    public DayData TEST_DAY;
    protected override void Awake()
    {
        base.Awake();
        m_title.gameObject.SetActive(true);
        EventManager.OnNotificationResolved.Register(HandleResolveNotification);
        EventManager.OnEndOfDay.Register(HandleEndOfDay);
    }

    public void StartGame()
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

    public void HandleGameOver()
    {
        if(m_currentGameState == GameState.GameOver || m_currentGameState == GameState.GameOverShown)
        {
            return;
        }

        m_currentGameState = GameState.GameOver;
    }
    
    public void ShowGameOver()
    {
        if(m_currentGameState == GameState.GameOverShown)
        {
            return;
        }

        m_currentGameState = GameState.GameOverShown;

        m_notificationUI.EmptyList();
        m_customerListUI.EmptyList();
        m_actionPanelUI.SetCustomer(null);

        NotificationData gameOverData = ScriptableObject.CreateInstance<NotificationData>();
        gameOverData.GenerateGameOver();
        m_notificationUI.AddNotification(gameOverData);

        NotificationData creditsData = ScriptableObject.CreateInstance<NotificationData>();
        creditsData.GenerateCredits();
        m_notificationUI.AddNotification(creditsData);

        EventManager.OnLose.Dispatch();
    }

    public void HandleEndOfDay()
    {
        if (m_currentGameState == GameState.EndOfDay || m_currentGameState == GameState.EndOfDayReport)
        {
            return;
        }
        m_currentGameState = GameState.EndOfDay;
    }

    public void ShowGiveEndOfDayReport()
    {
        if (m_currentGameState == GameState.EndOfDayReport)
        {
            return;
        }

        m_currentGameState = GameState.EndOfDayReport;

        m_notificationUI.EmptyList();
        m_customerListUI.EmptyList();
        m_actionPanelUI.SetCustomer(null);

        NotificationData endOfDayData = ScriptableObject.CreateInstance<NotificationData>();
        endOfDayData.GenerateEndOfDay(m_currentDay, m_numCorrectChoices, m_customerListUI.m_totelCustomers);
        m_notificationUI.AddNotification(endOfDayData);
    }

    public void TransitionDay()
    {
        EventManager.OnStartOfDay.Dispatch();
        StartCoroutine(HandleDayTransition());
    }

    public void RestartGame()
    {
        LeanTween.delayedCall(1.0f, () =>
        {
            SceneManager.LoadScene(0);
        });
    }

    protected IEnumerator HandleDayTransition()
    {
        m_currentGameState = GameState.Transitioning;

        // TODO: load generation stuff
        m_currentStrike = 0;
        m_numCorrectChoices = 0;
        m_customerListUI.ResetList();
        m_notificationUI.ResetList();

        m_currentDay++;

        for(int i=0; i<m_activeStories.Count; i++)
        {
            if(m_activeStories[i].Finished())
            {
                m_activeStories.Remove(m_activeStories[i]);
                i--;
            }
        }
        
        foreach(StoryData story in m_stories)
        {
            if(story != null && story.m_startDay == m_currentDay)
            {
                m_activeStories.Add(story);
            }
        }

        foreach(StoryData story in m_activeStories)
        {
            story.SetDay(m_currentDay);
        }
        m_loginButton.gameObject.SetActive(true);
        m_dayDisplay.SetDay(m_currentDay);
        m_titleBar.SetDay(m_currentDay);
        m_dayDisplay.FadeIn();
        yield return new WaitForSeconds(m_dayTransitionTime);
        SetupDay();
        m_dayDisplay.FadeOut();
    }

    public void StartDay()
    {
        // TODO: Make this less shitty looking
        m_loginButton.gameObject.SetActive(false);
        EventManager.OnStartGameplay.Dispatch();
        m_currentGameState = GameState.GameStarted;
        m_customerTimer = 0f;
    }

    protected void SetupDay()
    {
        RuleManager.Instance.ClearAllRules();
        if(m_currentDay < m_days.Length+1)
        {
            m_days[m_currentDay-1].ApplyRules();
        }
        else
        {
            RuleManager.Instance.ApplyRandomRules(m_currentDay);
        }

        RuleManager.Instance.BuildRuleNotifications();
        foreach(NotificationData n in RuleManager.Instance.m_dailyNotifications)
        {
            m_notificationUI.AddNotification(n);
        }

        foreach (StoryData story in m_activeStories)
        {
            story.Update();
            foreach (NotificationData nd in story.m_notificationsToShow)
            {
                m_notificationUI.AddNotification(nd);
            }
            story.m_notificationsToShow.Clear();
        }
    }

    protected void Update()
    {
        switch(m_currentGameState)
        {
            case GameState.GameStarted:
                UpdateGame();
                break;
            case GameState.EndOfDay:
                UpdateEndOfDay();
                break;
            case GameState.GameOver:
                UpdateGameOver();
                break;
        }

        if(Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.Period))
            {
                TransitionDay();
            }
        }
        
    }
    private bool fastTrack = false;
    protected void UpdateGame()
    {
        m_titleBar.UpdateTime();
        CustomerUI topCustomer = m_customerListUI.GetTopCustomer();


        if(m_customerListUI.GetNumCustomers() >= UnityEngine.Random.Range(3,5))
        {
            fastTrack = false;
        }
        if (topCustomer == null)
        {
            fastTrack = true;
        }


        m_customerTimer += Time.deltaTime * (fastTrack ? 4.0f : 1f);

        m_notificationTimer += Time.deltaTime;
        if (m_customerTimer >= 0)
        {
            m_customerTimer -= m_minTimeBetweenCustomers + UnityEngine.Random.Range(0f, Mathf.Max(0.0f, Mathf.Lerp(m_maxTimeBetweenCustomers, 1f, m_currentDay/20.0f)));
            if (m_customerListUI.GetNumCustomers() < m_maxNumCustomer)
            {
                CustomerData data = ScriptableObject.CreateInstance<CustomerData>();
                data.Generate();
                m_customerListUI.AddCustomer(data);
            }
            else
            {
                // give strike for max customers reached
                GiveStrike(NotificationData.StrikeReason.QueueFull);
            }
        }

        UpdateCustomerDisplay(topCustomer);

        foreach (StoryData story in m_activeStories)
        {
            foreach(CustomerData cd in story.m_customersToShow)
            {
                cd.m_activity = ActivityData.GetActivityByName(cd.m_StoryParameters.m_activityName);
                if(cd.m_StoryParameters.m_ForcePassFail)
                {
                    cd.ForcePassFail();
                }
                m_customerListUI.AddCustomer(cd);
            }
            foreach(NotificationData nd in story.m_notificationsToShow)
            {
                m_notificationUI.AddNotification(nd);
            }
            story.Update();
        }
    }

    protected void UpdateEndOfDay()
    {
        CustomerUI topCustomer = m_customerListUI.GetTopCustomer();
        UpdateCustomerDisplay(topCustomer);

        if (topCustomer == null && !m_isHandlingCustomer)
        {
            // no more customers in the queue
            ShowGiveEndOfDayReport();
        }
    }

    protected void UpdateGameOver()
    {
        if (!m_isHandlingCustomer)
        {
            ShowGameOver();
        }
    }

    protected void UpdateCustomerDisplay(CustomerUI newCustomer)
    {
        if (m_actionPanelUI.m_currentCustomer != newCustomer)
        {
            if (newCustomer == null || newCustomer.transform.localPosition.y < 5.0f)
            {
                m_actionPanelUI.SetCustomer(newCustomer);
            }
        }
    }

    protected void GiveStrike(NotificationData.StrikeReason reason = NotificationData.StrikeReason.None, string customMessage = "")
    {
        m_currentStrike++;
        if(m_currentStrike >= m_maxStrikes)
        {
            HandleGameOver();
        }
        else
        {
            m_notificationUI.AddStrikeNotification(m_currentStrike, reason, customMessage);
            m_canvasShaker.Shake();
            EventManager.OnStrike.Dispatch(m_currentStrike);
        }
    }
    
    protected void GiveStrike(NotificationData data)
    {
        m_currentStrike++;
        if (m_currentStrike >= m_maxStrikes)
        {
            HandleGameOver();
        }
        else
        {
            data.m_iconColor = Color.red;
            m_notificationUI.AddNotification(data);
            m_canvasShaker.Shake();
            EventManager.OnStrike.Dispatch(m_currentStrike);
        }
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

    protected IEnumerator HandleChoice(CustomerData data, ActionData.ActionType actionTaken)
    {
        m_isHandlingCustomer = true;
        RuleResponse rule = RuleManager.Instance.GetRuleForCustomer(data);
        if (!data.m_ignoreRules && rule.m_requiredResponse != actionTaken)
        {
            string policyIdentifier;
            if (rule.m_rule.m_subIndex >= 0)
            {
                policyIdentifier = string.Format("{0}{1}", rule.m_rule.m_policyIndex, (char)(rule.m_rule.m_subIndex + (int)'a'));
            }
            else
                policyIdentifier = rule.m_rule.m_policyIndex.ToString();
            GiveStrike(NotificationData.StrikeReason.WrongAction,
                string.Format("<b>{0}</b> <color=#CCCCCC><i>(see {1})</i></color>\n<b>Customer</b>: {2}\n<b>Required action</b>: {3}\n<b>Performed action</b>: <color=#FFAAAA>{4}</color>",
                rule.m_rule.TriggerReason(data),
                policyIdentifier,
                data.m_name.FirstLast,
                rule.m_requiredResponse.ToString(),
                actionTaken.ToString()));
        }
        else
        {
            m_numCorrectChoices++;
        }

        switch(actionTaken)
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

        if(data.m_StoryParameters != null && data.m_StoryParameters.m_story != null)
        {
            foreach(CustomerData.ParentStory.ResponseAction response in data.m_StoryParameters.m_responseActions)
            {
                if(response.m_action == actionTaken)
                {
                    switch (response.resolution)
                    {
                        case CustomerData.ResolutionAction.Strike:
                            if (data.m_StoryParameters.m_strikeNotification != null)
                            {
                                GiveStrike(data.m_StoryParameters.m_strikeNotification);
                            }
                            else
                            {
                                GiveStrike(NotificationData.StrikeReason.HRViolation);
                            }
                            break;
                        case CustomerData.ResolutionAction.TransitionDay:
                            TransitionDay();
                            break;
                        case CustomerData.ResolutionAction.GameOver:
                            HandleGameOver();
                            break;
                        case CustomerData.ResolutionAction.EndStory:
                            m_activeStories.Remove(data.m_StoryParameters.m_story);
                            break;
                        case CustomerData.ResolutionAction.PostNotificationA:
                            if (data.m_responseNotifications.Count >= 1)
                            {
                                m_notificationUI.AddNotification(data.m_responseNotifications[0]);
                            }
                            break;
                        case CustomerData.ResolutionAction.PostNotificationB:
                            if (data.m_responseNotifications.Count >= 2)
                            {
                                m_notificationUI.AddNotification(data.m_responseNotifications[1]);
                            }
                            break;
                        case CustomerData.ResolutionAction.PostNotificationC:
                            if (data.m_responseNotifications.Count >= 3)
                            {
                                m_notificationUI.AddNotification(data.m_responseNotifications[2]);
                            }
                            break;
                        case CustomerData.ResolutionAction.PostCustomerA:
                            if (data.m_responseCustomers.Count >= 1)
                            {
                                m_customerListUI.AddCustomer(data.m_responseCustomers[0]);
                            }
                            break;
                        case CustomerData.ResolutionAction.PostCustomerB:
                            if (data.m_responseCustomers.Count >= 2)
                            {
                                m_customerListUI.AddCustomer(data.m_responseCustomers[1]);
                            }
                            break;
                        case CustomerData.ResolutionAction.PostCustomerC:
                            if (data.m_responseCustomers.Count >= 3)
                            {
                                m_customerListUI.AddCustomer(data.m_responseCustomers[2]);
                            }
                            break;
                    }

                }
            }
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
                case NotificationData.ResolutionAction.Strike:
                    GiveStrike();
                    break;
                case NotificationData.ResolutionAction.TransitionDay:
                    TransitionDay();
                    break;
                case NotificationData.ResolutionAction.GameOver:
                    RestartGame();
                    break;
                case NotificationData.ResolutionAction.EndStory:
                    m_activeStories.Remove(notification.m_data.m_parentStory);
                    break;
                case NotificationData.ResolutionAction.PostNotificationA:
                    if (notification.m_data.m_responseNotifications.Count >= 1)
                    {
                        m_notificationUI.AddNotification(notification.m_data.m_responseNotifications[0]);
                    }
                    break;
                case NotificationData.ResolutionAction.PostNotificationB:
                    if (notification.m_data.m_responseNotifications.Count >= 2)
                    {
                        m_notificationUI.AddNotification(notification.m_data.m_responseNotifications[1]);
                    }
                    break;
                case NotificationData.ResolutionAction.PostNotificationC:
                    if (notification.m_data.m_responseNotifications.Count >= 3)
                    {
                        m_notificationUI.AddNotification(notification.m_data.m_responseNotifications[2]);
                    }
                    break;
                case NotificationData.ResolutionAction.PostCustomerA:
                    if (notification.m_data.m_responseCustomers.Count >= 1)
                    {
                        m_customerListUI.AddCustomer(notification.m_data.m_responseCustomers[0]);
                    }
                    break;
                case NotificationData.ResolutionAction.PostCustomerB:
                    if (notification.m_data.m_responseCustomers.Count >= 2)
                    {
                        m_customerListUI.AddCustomer(notification.m_data.m_responseCustomers[1]);
                    }
                    break;
                case NotificationData.ResolutionAction.PostCustomerC:
                    if (notification.m_data.m_responseCustomers.Count >= 3)
                    {
                        m_customerListUI.AddCustomer(notification.m_data.m_responseCustomers[2]);
                    }
                    break;
            }
        }
        else
        {
            if(notification.m_data.m_response != null && notification.m_data.m_response.m_strikeOnIncorrect)
            {
                GiveStrike(NotificationData.StrikeReason.HRViolation);
            }

            switch (notification.m_data.m_incorrectResponseAction)
            {
                case NotificationData.ResolutionAction.EndStory:
                    m_activeStories.Remove(notification.m_data.m_parentStory);
                    break;
            }
        }

        if (!notification.m_data.m_pinned)
        {
            m_notificationUI.RemoveNotification(notification);
        }
    }
}
