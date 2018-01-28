using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoryData {

    public struct ScheduledCustomer
    {
        public CustomerData m_data;
        public int m_time;
        public ScheduledCustomer(CustomerData data,int time)
        {
            m_data = data; m_time = time;
        }
    }

    public struct ScheduledNotification
    {
        public NotificationData m_data;
        public int m_time;
        public ScheduledNotification(NotificationData data, int time)
        {
            m_data = data; m_time = time;
        }
    }

    public Dictionary<int, List<ScheduledCustomer>> customerScheduleByDay = new Dictionary<int, List<ScheduledCustomer>>();
    public Dictionary<int, List<ScheduledNotification>> notificationScheduleByDay = new Dictionary<int, List<ScheduledNotification>>();

    public StoryData(string path)
    {
        ParseStoryFile(path);
    }

    void ParseStoryFile(string storyFile)
    {
        TextAsset t = Resources.Load(storyFile) as TextAsset;
        if (t == null)
            return;
        string storyText = t.text.Replace("\n", "\r");
        string[] lines = storyText.Split(new Char[] { '\r'}, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            string[] split = line.Split(new Char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int day = 0;
            int.TryParse(split[1], out day);
            int time = 0;
            int.TryParse(split[2], out time);
            switch (split[0])
            {
                case "CUSTOMER":
                    {
                        CustomerData data = ScriptableObject.CreateInstance<CustomerData>();
                        data.m_name.Set(split[3]);
                        int.TryParse(split[4], out data.m_daysActive);
                        float.TryParse(split[5], out data.m_dataUsage);
                        data.m_activity = ActivityData.GetActivity(split[6]);

                        if (!customerScheduleByDay.ContainsKey(day))
                        {
                            customerScheduleByDay.Add(day, new List<ScheduledCustomer>());
                        }
                        customerScheduleByDay[day].Add(new ScheduledCustomer(data, time));
                    }
                    break;
                case "NOTIFICATION":
                    {
                        NotificationData data = ScriptableObject.CreateInstance<NotificationData>();
                        data.m_title = split[3];
                        data.m_sender = split[4];
                        data.m_message = split[5];
                        int pin = 0;
                        int.TryParse(split[6], out pin);
                        data.m_pinned = (pin == 1);
                        if (split.Length > 7)
                        {
                            data.m_response = new NotificationData.Response(split[7], split[8], (NotificationData.Response.CorrectResponse)(int.Parse(split[9])));
                        }

                        if (!notificationScheduleByDay.ContainsKey(day))
                        {
                            notificationScheduleByDay.Add(day, new List<ScheduledNotification>());
                        }
                        notificationScheduleByDay[day].Add(new ScheduledNotification(data, time));
                    }
                    break;
            }
        }
    }
}
