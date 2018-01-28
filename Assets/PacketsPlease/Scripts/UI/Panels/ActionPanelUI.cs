using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanelUI : MonoBehaviour {

    public CustomerDetailsUI m_customerDetails;
    public SpeedIndicatorUI m_speedIndiciator;

    protected CustomerUI m_currentCustomer;
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

        if(customer == null)
        {
            m_customerDetails.gameObject.SetActive(false);
        }
        else
        {
            customer.SetNameColor(m_activeNameColor);
            customer.SetBGColor(m_activeUIColor);
            m_customerDetails.gameObject.SetActive(true);
            m_customerDetails.Init(customer.m_data);
            m_speedIndiciator.Reset();
        }

        m_currentCustomer = customer;
    }

    public void DoBoostFeedback()
    {
        m_speedIndiciator.Boost();
    }

    public void DoThrottleFeedback()
    {
        m_speedIndiciator.Throttle();
    }

    public void DoDisconnectFeedback()
    {
        m_speedIndiciator.Disconnect();
    }
}
