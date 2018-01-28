using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanelUI : MonoBehaviour {

    public CustomerDetailsUI m_customerDetails;
    public SpeedIndicatorUI m_speedIndiciator;

    public CustomerUI m_currentCustomer { get; protected set; }
    public Color m_activeUIColor = Color.green;
    public Color m_activeNameColor = Color.gray;

    // Use this for initialization
    void Start ()
    {
        transform.parent.localPosition = Vector3.up * -Screen.height * 0.5f;
        m_customerDetails.gameObject.SetActive(false);
	}
	
    public void SetCustomer(CustomerUI customer)
    {
        if(customer == m_currentCustomer)
        {
            return;
        }

        m_currentCustomer = customer;

        if (m_currentCustomer == null)
        {
            m_customerDetails.gameObject.SetActive(false);
            LeanTween.moveLocalY(transform.parent.gameObject, -Screen.height * 0.5f, 0.5f).setEaseInBack();
        }
        else
        {
            m_currentCustomer.SetNameColor(m_activeNameColor);
            m_currentCustomer.SetBGColor(m_activeUIColor);
            m_speedIndiciator.Reset();
            m_customerDetails.gameObject.SetActive(true);
            m_customerDetails.Init(m_currentCustomer.m_data);
            transform.parent.localPosition = Vector3.up * -Screen.height * 0.25f;
            LeanTween.moveLocalY(transform.parent.gameObject, 0.0f, 0.5f).setEaseOutBack();
        }
    }

    public void DoBoostFeedback()
    {
        m_speedIndiciator.Boost();
        m_customerDetails.OnBoostChoice();
        EventManager.OnBoost.Dispatch();
    }

    public void DoThrottleFeedback()
    {
        m_speedIndiciator.Throttle();
        m_customerDetails.OnThrottleChoice();
        EventManager.OnThrottle.Dispatch();
    }

    public void DoDisconnectFeedback()
    {
        m_speedIndiciator.Disconnect();
        m_customerDetails.OnDisconnectChoice();
        EventManager.OnDisconnect.Dispatch();
    }

}
