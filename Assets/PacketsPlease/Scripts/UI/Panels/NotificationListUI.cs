using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationListUI : MonoBehaviour {
    public GameObject m_notificationPrefab;

    public List<NotificationUI> m_notificationUIs;

    public float m_separationDist = 10.0f;
    public float m_slideSpeed = 3.0f;

    protected void Update()
    {
        for (int i = 0; i < m_notificationUIs.Count; ++i)
        {
            m_notificationUIs[i].transform.localPosition = Vector3.Lerp(m_notificationUIs[i].transform.localPosition, new Vector3(0.0f, i * m_separationDist, 0.0f), Time.deltaTime * m_slideSpeed);
        }
    }

    public void ResetList()
    {
        EmptyList();
    }

    public void EmptyList()
    {
        for (int i = 0; i < m_notificationUIs.Count; ++i)
        {
            m_notificationUIs[i].DestroySelf();
        }
        m_notificationUIs.Clear();
    }

    public NotificationUI AddNotification(NotificationData data)
    {
        GameObject notificationUiObj = Instantiate(m_notificationPrefab, transform);
        NotificationUI ui = notificationUiObj.GetComponent<NotificationUI>();
        if (data.m_response != null && data.m_response.m_clearMe)
            data.m_response = null;
        ui.Init(data);
        m_notificationUIs.Add(ui);
        ui.transform.localPosition = Vector3.up * -Screen.height * 2;
        if(data.m_autoOpen)
        {
            ui.SelectSelf();
        }
        return ui;
    }

    public void RemoveNotification(NotificationUI notification)
    {
        m_notificationUIs.Remove(notification);
        notification.DestroySelf();
    }

    public NotificationUI AddStrikeNotification(int number)
    {
        GameObject notificationUiObj = Instantiate(m_notificationPrefab, transform);
        NotificationUI ui = notificationUiObj.GetComponent<NotificationUI>();
        NotificationData data = ScriptableObject.CreateInstance<NotificationData>();
        data.GenerateStrike(number);
        ui.Init(data);
        m_notificationUIs.Add(ui);
        ui.transform.localPosition = Vector3.up * -Screen.height * 2;
        if (data.m_autoOpen)
        {
            ui.SelectSelf();
        }
        return ui;
    }
}
