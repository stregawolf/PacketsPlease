using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationListUI : MonoBehaviour {
    public GameObject m_notificationPrefab;
    public GameObject m_strikeNoficationPrefab;

    public List<NotificationUI> m_notificationUIs;

    public float m_separationDist = 10.0f;
    public float m_slideSpeed = 3.0f;

    protected void Update()
    {
        int i = 0;
        foreach (var ui in m_notificationUIs)
        {
            ui.transform.localPosition = Vector3.Lerp(ui.transform.localPosition, new Vector3(0.0f, i * m_separationDist, 0.0f), Time.deltaTime * m_slideSpeed);
            i++;
        }
    }

    public void AddNotification(NotificationData data)
    {
        GameObject notificationUiObj = Instantiate(m_notificationPrefab, transform);
        NotificationUI ui = notificationUiObj.GetComponent<NotificationUI>();
        ui.Init(data);
        m_notificationUIs.Add(ui);
        ui.transform.localPosition = Vector3.up * -Screen.height * 2;
    }

    public void AddStrikeNotification(int number)
    {
        GameObject notificationUiObj = Instantiate(m_strikeNoficationPrefab, transform);
        NotificationUI ui = notificationUiObj.GetComponent<NotificationUI>();
        NotificationData data = ScriptableObject.CreateInstance<NotificationData>();
        data.GenerateStrike(number);
        ui.Init(data);
        m_notificationUIs.Add(ui);
        ui.transform.localPosition = Vector3.up * -Screen.height * 2;
    }
}
