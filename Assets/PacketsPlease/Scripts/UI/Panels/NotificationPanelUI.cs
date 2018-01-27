using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationPanelUI : MonoBehaviour {

    public NotificationDetailsUI m_notificationDetails;
    public NotificationUI m_sourceNotification = null;

	// Use this for initialization
	void Start () {
        m_notificationDetails.gameObject.SetActive(false);
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
}
