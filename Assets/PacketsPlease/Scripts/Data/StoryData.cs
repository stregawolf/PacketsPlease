using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "StoryData", menuName = "Data/StoryData", order = 3)]

public class StoryData : ScriptableObject {

    [System.Serializable]
    public struct ScheduledCustomer
    {
        public CustomerData m_data;
        public int m_time;
        public ScheduledCustomer(CustomerData data,int time)
        {
            m_data = data; m_time = time;
        }
    }

    [System.Serializable]
    public struct ScheduledNotification
    {
        public NotificationData m_data;
        public int m_time;
        public ScheduledNotification(NotificationData data, int time)
        {
            m_data = data; m_time = time;
        }
    }

    [System.Serializable]
    public class DailySchedule
    {
        public string m_name = "Day";
        public List<ScheduledCustomer> m_customers;
        public List<ScheduledNotification> m_notifications;
    }

    public int m_startDay = 1;
    public List<DailySchedule> m_days;
    
    private int m_currentDay = 0;
    private int m_lastMinutesPassed = -1;

    public List<CustomerData> m_customersToShow = new List<CustomerData>();
    public List<NotificationData> m_notificationsToShow = new List<NotificationData>();

    public void SetDay(int day)
    {
        m_notificationsToShow.Clear();
        m_customersToShow.Clear();

        m_currentDay = day;
        m_lastMinutesPassed = -1;
    }

    public bool Finished()
    {
        int dayIndex = m_currentDay - m_startDay;
        return m_days.Count <= dayIndex;
    }

    public void Update()
    {
        m_notificationsToShow.Clear();
        m_customersToShow.Clear();

        int dayIndex = m_currentDay - m_startDay;
        if (Finished())
            return;
        int minutesPassed = TitleBarUI.MinutesSinceDayStart;
        foreach (ScheduledNotification sn in m_days[dayIndex].m_notifications)
        {
            if (m_lastMinutesPassed < sn.m_time && minutesPassed >= sn.m_time)
            {
                sn.m_data.m_parentStory = this;
                if(sn.m_data.m_response.m_clearMe)
                {
                    Debug.Log("Clearing!");
                    sn.m_data.m_response = null;
                }
                m_notificationsToShow.Add(sn.m_data);
            }
        }

        foreach (ScheduledCustomer sc in m_days[dayIndex].m_customers)
        {
            if (m_lastMinutesPassed < sc.m_time && minutesPassed >= sc.m_time)
            {
                sc.m_data.m_StoryParameters.m_story = this;
                m_customersToShow.Add(sc.m_data);
            }
        }
        m_lastMinutesPassed = minutesPassed;
    }
        
}
