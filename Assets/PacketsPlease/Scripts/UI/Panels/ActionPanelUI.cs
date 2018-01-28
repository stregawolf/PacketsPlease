using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanelUI : MonoBehaviour {

    public CustomerDetailsUI m_customerDetails;
    public SpeedIndicatorUI m_speedIndiciator;

    public CustomerUI m_currentCustomer { get; protected set; }
    public Color m_activeUIColor = Color.green;
    public Color m_activeNameColor = Color.gray;

    // Use this for initialization
    void Start () {
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
        }
        else
        {
            m_currentCustomer.SetNameColor(m_activeNameColor);
            m_currentCustomer.SetBGColor(m_activeUIColor);
            m_speedIndiciator.Reset();
            m_customerDetails.gameObject.SetActive(true);
            m_customerDetails.Init(m_currentCustomer.m_data);
        }
    }

    public void DoBoostFeedback()
    {
        m_speedIndiciator.Boost();
        m_customerDetails.OnPositiveChoice();
    }

    public void DoThrottleFeedback()
    {
        m_speedIndiciator.Throttle();
        m_customerDetails.OnNegativeChoice();
    }

    public void DoDisconnectFeedback()
    {
        m_speedIndiciator.Disconnect();
        m_customerDetails.OnNegativeChoice();
    }
}
