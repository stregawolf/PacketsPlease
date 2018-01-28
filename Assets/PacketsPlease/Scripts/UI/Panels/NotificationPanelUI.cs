using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationPanelUI : MonoBehaviour {

    public NotificationDetailsUI m_notificationDetails;
    public NotificationUI m_sourceNotification = null;

    public Color m_activeBGColor = Color.green;
    public Color m_activeTitleColor = Color.gray;

    // Use this for initialization
    protected void Awake () {
        m_notificationDetails.gameObject.SetActive(false);
        EventManager.OnNotificationSelected.Register(SetNotification);
        gameObject.SetActive(false);
    }

    protected void OnDestroy()
    {
        EventManager.OnNotificationSelected.Unregister(SetNotification);
    }


    public void SetNotification(NotificationUI sourceNotification)
    {
        if(m_sourceNotification == sourceNotification)
        {
            return;
        }

        if(m_sourceNotification != null)
        {
            m_sourceNotification.ResetColors();
        }

        m_sourceNotification = sourceNotification;
        if(sourceNotification == null)
        {
            m_notificationDetails.gameObject.SetActive(false);
            LeanTween.scale(gameObject, Vector3.zero, 0.33f).setEaseInBack();
        }
        else
        {
            gameObject.SetActive(true);
            m_notificationDetails.gameObject.SetActive(true);
            m_notificationDetails.Init(sourceNotification.m_data);

            m_sourceNotification.SetBGColor(m_activeBGColor);
            m_sourceNotification.SetTitleColor(m_activeTitleColor);

            transform.localScale = Vector3.one * 0.5f;
            LeanTween.scale(gameObject, Vector3.one, 0.33f).setEaseOutBack();
        }

    }

    public void Respond(int response)
    {
        if(m_sourceNotification != null)
        {
            bool respondedCorrectly = true;
            if (m_sourceNotification.m_data.m_response != null)
            {
                if ((int)m_sourceNotification.m_data.m_response.m_correctResponse != response)
                {
                    respondedCorrectly = false;
                }
            }
            EventManager.OnNotificationResolved.Dispatch(m_sourceNotification, respondedCorrectly);
        }
        SetNotification(null);
    }
}
