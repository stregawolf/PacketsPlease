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
}
