using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationPanelUI : MonoBehaviour {

    public NotificationDetailsUI m_notificationDetails;
    public NotificationUI m_sourceNotification = null;

	// Use this for initialization
	void Start () {
        m_notificationDetails.gameObject.SetActive(false);
        EventManager.OnNotificationSelected.Register(SetNotification);
	}
	

    public void SetNotification(NotificationUI sourceNotification)
    {
        m_sourceNotification = sourceNotification;
        if(sourceNotification == null)
        {
            m_notificationDetails.gameObject.SetActive(false);
        }
        else
        {
            m_notificationDetails.gameObject.SetActive(true);
            m_notificationDetails.Init(sourceNotification.m_data);
        }
    }

    public void Respond(int response)
    {
        if(m_sourceNotification != null)
        {
            if (m_sourceNotification.m_data.m_response != null)
            {
                if ((int)m_sourceNotification.m_data.m_response.m_correctResponse == response)
                {
                    Debug.Log("Good Job!");
                }
                else
                    Debug.Log("Wrong answer");
            }
            EventManager.OnNotificationResolved.Dispatch(m_sourceNotification);
        }
        SetNotification(null);
    }
}
